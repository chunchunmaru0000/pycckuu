namespace pycckuu;

public class RbpExpression : ICompilable
{
    public Instruction Compile() => new(EvaluatedType.INT, "    push rbp ; РБП");
}
