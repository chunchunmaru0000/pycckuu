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
        string pushStr = func.ReturnType == EvaluatedType.INT 
            ? "    push rax" 
            : "    ; НЕЧЕГО ВОЗВРАЩАТЬ";

        if (!imported) {
            string[] setPrs = [.. Enumerable.Range(0, parameters.Length).Select(i =>
                true // если тип помещается на стэк иначе его надо оствить на стеке
                ? i < U.Registers.Length
                    ? $"    pop {U.Registers[i]}"
                    : $"    ; {parameters[i].Type}" // тут может надо оставшиеся параметры переворачивать
                : $"    ; {parameters[i].Type}"
            )];
            int clearSize = Math.Max(0, parameters.Length - 4) * 8; // 8 bytes per param
            string clearStr =
                clearSize > 0
                ? $"    add rsp, {clearSize} ; ОЧИСТКА СТЕКА ПОСЛЕ ВЫЗОВА {name}"
                :  "    ; НЕЧЕГО ОЧИЩАТЬ СО СТЕКА";

            return new(func.ReturnType, Comp.Str([
                Comp.Str([.. parameters.Select(p => p.Code)]), // compile all params and they are on stack
                Comp.Str(setPrs), // положить параметры в регистры и на стек
                $"    call {name} ; ВЫЗОВ ОБЪЯВЛЕННОЙ ФУНКЦИИ",
                clearStr, // очистить стек от параметров которые были на него положена перед вызовом
                pushStr,
            ]));
        } else if (!func.VariableArguments || parameters.Length <= 4) {
            string[] prs = [.. Enumerable.Range(0, parameters.Length).Select(i =>
                true // если тип помещается на стэк иначе его надо оствить на стеке
                ? i < U.Registers.Length 
                    ? $"    pop {U.Registers[i]}"
                    : $"    ; {parameters[i].Type}" // тут может надо оставшиеся параметры переворачивать
                : $"    ; {parameters[i].Type}"
            )];

            int extraOffset = parameters.Skip(4).Sum(p => 8);// p.Type.Size());;

            return new(func.ReturnType, Comp.Str([
                $"    sub rsp, {MIN_OFFSET + extraOffset} ; ТЕНЕВОЕ ПРОСТРАНСТВО ПЕРЕД ВЫЗОВОМ {name}",
                Comp.Str([.. parameters.Select(p => p.Code)]), // compile all params and they are on stack
                Comp.Str(prs),
                $"    call [{name}] ; ВЫЗОВ ИМПОРТИРОВАННОЙ ФУНКЦИИ",
                $"    add rsp, {MIN_OFFSET + extraOffset * 2} ; ВОЗВРАЩЕНИЕ СТЭКА {name}", //  * 2 потому что вот оно на стэк положило это во первых память нужна, а во вторых со стека оно очищено не было by callee поэтому вручную надо
                pushStr,
            ])); 
        } 
        else {
            List<string> prs = [.. Enumerable.Range(0, 4).Select(i =>
                $"    pop {U.Registers[i]}"
            )];

            int extraParams = parameters.Length - 4;

            prs.AddRange(Enumerable.Range(0, extraParams).Select(i => Comp.Str([
                $"    pop r10",
                $"    mov qword [rsp + {32 + (--extraParams * 8) + 8 * i}], r10"
            ])));

            return new(func.ReturnType, Comp.Str([
                $"    sub rsp, {MIN_OFFSET} ; ТЕНЕВОЕ ПРОСТРАНСТВО ПЕРЕД ВЫЗОВОМ {name}",
                Comp.Str([.. parameters.Select(p => p.Code)]), // compile all params and they are on stack
                Comp.Str([.. prs]),
                $"    call [{name}] ; ВЫЗОВ ИМПОРТИРОВАННОЙ ФУНКЦИИ",
                $"    add rsp, {MIN_OFFSET} ; ВОЗВРАЩЕНИЕ СТЭКА {name}",
                pushStr,
            ]));
        }
    }

    public override string ToString() =>
        $"ЗОВ {Func.Value} ...;";
}
