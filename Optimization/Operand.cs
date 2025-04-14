namespace pycckuu;

public class Operand(OpType type, EvaluatedType Size, string value = null)
{
    public OpType Type { get; } = type;
    public EvaluatedType Size { get; } = Size;
    public string? Value { get; } = value;
}
