namespace pycckuu;

public sealed class CallStatement(Token func, ICompilable[] parameters) : ICompilable
{
    public Token Func { get; set; } = func;
    public ICompilable[] Parameters { get; set; } = parameters;

    private static int MIN_OFFSET { get; } = 32;

    public Instruction Compile()
    {
        Instruction[] parameters = [.. Parameters.Reverse().Select(p => p.Compile())];
        string name = Func.Value!.ToString()!;
        bool imported = Compiler.HaveImpFunc(name);
        ImportedFunction func = Compiler.GetFuncImportedAs(name);

        string returned = func.ReturnType == EvaluatedType.INT 
            ? "    push rax" 
            : "    ; НЕЧЕГО ВОЗВРАЩАТЬ";

        int stackParams = Math.Max(0, parameters.Length - 4);
        int extraParams = stackParams;
        string call = imported
            ? $"    call [{name}] ; ВЫЗОВ ИМПОРТИРОВАННОЙ ФУНКЦИИ"
            : $"    call  {name}  ; ВЫЗОВ   ОБЪЯВЛЕННОЙ   ФУНКЦИИ";

        string args, sub, add;
        if (!func.VariableArguments) {
            int subBytes = MIN_OFFSET + stackParams * 8;
            int addBytes = subBytes + stackParams * 8;

            args = Comp.StrE(parameters.Length, i =>
                i >= U.Registers.Length
                ? $"    ; {parameters[i].Type.Log()}"
                : $"    pop {U.Registers[i]}"
            );
            if (!imported) {
                sub = $"    ; ПЕРЕД ОБЪЯВЛЕННОЙ ФУНКЦИЕЙ SHADOW SPACE НЕ ТРЕБУЕТСЯ";
                add = $"    add rsp, {stackParams * 8} ; ОЧИСТИТЬ СТЭК ОТ ПАРАМЕТРОВ НА СТЭКЕ {stackParams} * 8";
            } else {
                sub = $"    sub rsp, {subBytes} ; = SHADOW SPACE {MIN_OFFSET} + ПАРАМЕТРОВ НА СТЭКЕ {stackParams} * 8";
                add = $"    add rsp, {addBytes} ; = sub {subBytes} + ПАРАМЕТРОВ НА СТЭКЕ {stackParams} * 8";
            }
        } else {
            int subBytes = 8 * parameters.Length;
            int addBytes = subBytes + stackParams * 8;

            return new(func.ReturnType, Comp.Str([
                Comp.Str([.. parameters.Select(p => p.Code)]), // compile all params and push on stack
                Comp.StrE(Math.Min(4, parameters.Length), i =>
                $"    pop {U.Registers[i]}"),
                $"    sub rsp, {subBytes} ; = ПАРАМЕТРОВ {parameters.Length} * 8",
                Comp.StrE(Math.Max(0, parameters.Length - 4), i => Comp.Str([
                $"    mov r10, qword[rsp + {subBytes + i * 8}]",
                $"    mov qword[rsp + {MIN_OFFSET + i * 8}], r10"])),
                call,
                $"    add rsp, {addBytes} ; = sub {subBytes} + ПАРАМЕТРОВ НА СТЭКЕ {stackParams} * 8 ВОЗВРАЩЕНИЕ СТЭКА {name}",
                returned
            ]));
        }
        return new(func.ReturnType, Comp.Str([
            Comp.Str([.. parameters.Select(p => p.Code)]), // compile all params and push on stack
            args,
            sub,
            call,
            add,
            returned
        ]));
    }

    public override string ToString() =>
        $"ЗОВ {Func.Value} ...;";
}
