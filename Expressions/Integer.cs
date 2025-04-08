namespace pycckuu;

public sealed class IntegerExpression(Token token) : ICompilable
{
    public long Value { get; } = Convert.ToInt64(token.Value);

    public object Evaluate() => Value;

    public Instruction Compile() => new(EvaluatedType.INT,Comp.Str([
        $"    push qword {Value} ; ЦЕЛОЕ ЧИСЛО {Value}",
    ]));

    public override string ToString() => token.Type.View();
}
