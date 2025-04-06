namespace pycckuu;

public sealed class CallStatement(Token func, ICompilable[] parameters) : ICompilable
{
    public Token Func { get; set; } = func;
    public ICompilable[] Parameters { get; set; } = parameters;

    private Dictionary<int, string> Registers { get; } = new(){
        { 0, "rcx" },
        { 1, "rdx" },
        { 2, "r8" },
        { 3, "r9" },
    };

    private static int MIN_OFFSET { get; } = 32;

    public Instruction Compile()
    {
        Instruction[] parameters = [.. Parameters.Reverse().Select(p => p.Compile())];
        ImportedFunction func = Compiler.GetFuncImportedAs(Func.Value!.ToString()!);
        if (!func.VariableArguments || parameters.Length <= 4) {
            string[] prs = [.. Enumerable.Range(0, parameters.Length).Select(i =>
                true // если тип помещается на стэк иначе его надо оствить на стеке
                ? Registers.ContainsKey(i) 
                    ? $"    pop {Registers[i]}"
                    : $"    ; {parameters[i].Type}" // тут может надо оставшиеся параметры переворачивать
                : $"    ; {parameters[i].Type}"
            )];

            int extraOffset = parameters.Skip(4).Sum(p => 8);// p.Type.Size());;

            return new(func.ReturnType, Comp.Str([
                $"    sub rsp, {MIN_OFFSET + extraOffset} ; ТЕНЕВОЕ ПРОСТРАНСТВО ПЕРЕД ВЫЗОВОМ",
                Comp.Str([.. parameters.Select(p => p.Code)]), // compile all params and they are on stack
                Comp.Str(prs),
                $"    call [{Func.Value}]",
                $"    add rsp, {MIN_OFFSET + extraOffset * 2} ; ВОЗВРАЩЕНИЕ СТЭКА", //  * 2 потому что вот оно на стэк положило это во первых память нужна, а во вторых со стека оно очищено не было by callee поэтому вручную надо
                func.ReturnType == EvaluatedType.INT
                ?"    push rax"
                :"",
            ""])); 
        } else {
            List<string> prs = [.. Enumerable.Range(0, 4).Select(i =>
                $"    pop {Registers[i]}"
            )];

            int extraParams = parameters.Length - 4;

            prs.AddRange(Enumerable.Range(0, extraParams).Select(i => Comp.Str([
                $"    pop r10",
                $"    mov qword [rsp + {32 + (--extraParams * 8) + 8 * i}], r10"
            ])));

            return new(func.ReturnType, Comp.Str([
                $"    sub rsp, {MIN_OFFSET} ; ТЕНЕВОЕ ПРОСТРАНСТВО ПЕРЕД ВЫЗОВОМ",
                Comp.Str([.. parameters.Select(p => p.Code)]), // compile all params and they are on stack
                Comp.Str([.. prs]),
                $"    call [{Func.Value}]",
                $"    add rsp, {MIN_OFFSET} ; ВОЗВРАЩЕНИЕ СТЭКА",
                func.ReturnType == EvaluatedType.INT
                ?"    push rax"
                :"",
            ""]));
        }
    }

    public override string ToString() =>
        $"{TokenType.CALL.Log()} {Func.Value} ...;";
}
