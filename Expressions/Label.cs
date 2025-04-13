namespace pycckuu;

public class LabelExpression(Token name) : ICompilable
{
    public Token Name { get; } = name;

    public Instruction Compile() =>
        new(EvaluatedType.LABEL, $"    push qword {Name.Value}");
}
