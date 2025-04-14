namespace pycckuu;

public class AsmInstruction(InstType type, Operand[] ops, string quote = "")
{
    public InstType Type { get; } = type;
    public Operand[] Ops { get; } = ops;
    public string Quote { get; } = quote;
}
