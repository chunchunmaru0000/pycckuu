namespace pycckuu;

public class DeclareFunctionStatement(string name, bool typed, List<Token> args, BlockStatement body) : ICompilable
{
    public string Name { get; } = name;
    public bool Typed { get; } = typed;
    public List<Token> Args { get; } = args;
    public BlockStatement Body { get; } = body;

    public Instruction Compile()
    {
        Compiler.AddLibImports(new("?", Name, Name, false, Typed ? EvaluatedType.INT : EvaluatedType.VOID));
        List<Variable> vars = Compiler.ReplaceVariables([]);

        Variable[] args = [.. Args.Select(a => Compiler.AddVariable(
            new(a.Value!.ToString()!, Compiler.GetLastVarOffset() + EvaluatedType.INT.Size(), EvaluatedType.INT)
        ).Value)];
        int extraArgs = args.Length - 4;
        int argsOffset = Compiler.GetLastVarOffset();

        string code = Comp.Str([
            $"{Name}: ; ФУНКЦИЯ {Name}",
            $"    push rbp ; ВЫРАВНИВАНИЕ СТЭКА ПО 16 ТАК КАК ПРИ call ПРОИСХОДИТ push rip ТО ЕСТЬ СТЭК СМЕЩАЕТСЯ НА 8",
            $"    mov rbp, rsp",
            Comp.StrER(args.Length, i =>
                i >= U.Registers.Length
                ? Comp.Str([
                    $"    mov r10, [rsp + {8 + 8 * extraArgs--}]",
                    $"    mov qword[rbp - {args[i].Offset}], r10",
                ])
                :   $"    mov qword[rbp - {args[i].Offset}], {U.Registers[i]}"
            ),
            $"    sub rsp, {argsOffset} ; ПАРАМЕТРЫ НА СТЭКЕ ПОЭТОМУ ЕГО НАДО ТОЖЕ СДВИНУТЬ ЧТОБЫ НЕ ПОВРЕДИТЬ ПЕРЕМЕННЫЕ",
            Body.Compile().Code,
            $"    add rsp, {argsOffset} ; ОЧИЩЕНИЕ СТЕКА ОТ ПАРАМЕТРОВ",
            $"    pop rbp ; ВЫКРИВЛЕНИЕ СТЭКА НАЗАД ЧТОБЫ ПОТОМ ОН БЫЛ ВОССТАНОВЛЕН САМ ЧЕРЕЗ pop rip ПРИ ret",
            $"    ret ; КОНЕЦ ФУНКЦИИ {Name} КОТОРЫЙ НЕ ДОЛЖЕН БЫТЬ ДОСЯГАЕМ ЕСЛИ ФУНКЦИЯ ЧТОТО ВОЗВРАЩАЕТ",
        ]);

        Compiler.ReplaceVariables(vars);
        Compiler.DeclareFunction(code);
        return new(EvaluatedType.VOID, "");
    }
}
