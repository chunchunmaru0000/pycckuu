namespace pycckuu;

public sealed class Call(Token func, ICompilable[] parameters) : ICompilable
{
    public Token Func { get; set; } = func;
    public ICompilable[] Parameters { get; set; } = parameters;

    public Instruction Compile()
    {
        return new(EvaluatedType.CALL, Comp.Str([
            $"    call [{Func.Value}]"
        ]));
    }

    public override string ToString() =>
        $"{TokenType.CALL.Log()} {Func.Value} ...;";
}
