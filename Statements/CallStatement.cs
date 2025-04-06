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

    public Instruction Compile()
    {
        Instruction[] parameters = [.. Parameters.Reverse().Select(p => p.Compile())];
        ImportedFunction func = Compiler.GetFuncImportedAs(Func.Value!.ToString()!);

        string[] prs = [.. Enumerable.Range(0, parameters.Length).Select(i =>
            true // если тип помещается на стэк иначе его надо оствить на стеке
            ? Registers.ContainsKey(i) 
                ? $"    pop {Registers[i]}"
                : $"    ; {parameters[i].Type}" // тут может надо оставшиеся параметры переворачивать
            : $"    ; {parameters[i].Type}"
        )];

        int extraSize = 40 + parameters.Skip(4).Sum(p => p.Type.Size());

        return new(EvaluatedType.CALL, Comp.Str([
            $"    sub rsp, {extraSize} ; ТЕНЕВОЕ ПРОСТРАНСТВО ПЕРЕД ВЫЗОВОМ",
            Comp.Str([.. parameters.Select(p => p.Code)]), // compile all params and they are on stack
            Comp.Str(prs),
            $"    call [{Func.Value}]",
            $"    add rsp, {extraSize} ; ВОЗВРАЩЕНИЕ СТЭКА",
        ""])); 
    }

    public override string ToString() =>
        $"{TokenType.CALL.Log()} {Func.Value} ...;";
}
