using System.Diagnostics;

namespace pycckuu;

public class SetVarStatement(Token name, ICompilable value, int exclamations) : ICompilable
{
    private Token Name { get; set; } = name;
    private ICompilable Value { get; set; } = value;
    private int Exclamations { get; set; } = exclamations;

    public Instruction Compile()
    {
        Instruction value = Value.Compile();
        int s = value.Type.Size();

        int lastOffset = Compiler.GetLastVarOffset();
        Variable var = new(Name.Value!.ToString()!, lastOffset + s, value.Type);
        string subRsp;
        KeyValuePair<bool, Variable> hasVar = Compiler.AddVariable(var);
        if (hasVar.Key) {
            subRsp = $"    ; {var.Name} УЖЕ ОБЪЯВЛЕНА ПОЭТОМУ СДВИГ СТЭКА НЕ ТРЕБУЕТСЯ";
            var = hasVar.Value;
        }
        else
            subRsp = $"    sub rsp, {s} ; ПРИ ОБЪЯВЛЕНИИ ПЕРЕМЕННОЙ ВЫЧЕСТЬ rsp ЧТОБЫ СДВИНУТЬ СТЭК ДО ПЕРЕМЕНОЙ";

        return new(Exclamations switch
        {
            0 => EvaluatedType.VOID,
            1 => EvaluatedType.INT,
            2 => value.Type,
            _ => throw new UnreachableException()
        }, Comp.Str([
            value.Code,
            $"    pop r8",
            //$"    mov {U.Sizes[s]} [{name}], r8{U.RRegs[s]}",
            $"    mov {U.Sizes[s]}[rbp - {var.Offset}], r8{U.RRegs[s]} ; {var.Name}",
            subRsp,
            Exclamations switch {
                0 => $"    ; НОЛЬ !",
                //1 => $"    push {name} ; ! УКАЗАТЕЛЬ", // $"    push {U.Sizes[s]}[rbp - {var.Offset}]",
                1 => throw new("ТАК БОЛЬШЕ НЕЛЬЗЯ ПОЛУЧАТЬ УКАЩАТЕЛЬ НА ОБЪЯВЛЕННУЮ ПЕРЕМЕННУЮ"),
                2 => $"    push r8 ; !! ЗНАЧЕНИЕ",
                _ => throw new UnreachableException()
            }
        ]));
    }
}

public class SetPtrStatement(ICompilable ptr, ICompilable value) : ICompilable
{
    private ICompilable Ptr { get; set; } = ptr;
    private ICompilable Value { get; set; } = value;

    public Instruction Compile()
    {
        Instruction value = Value.Compile();
        int s = value.Type.Size();

        return new(EvaluatedType.VOID, Comp.Str([
            Ptr.Compile().Code,
            Value.Compile().Code,
            "    pop r9",
            "    pop r8",
           $"    mov {U.Sizes[s]}[r8], r9{U.RRegs[s]}",
        ]));
    }
}

