namespace pycckuu;

public class Variable(string name, int offset, EvaluatedType type)
{
    public string Name { get; } = name;
    public int Offset { get; } = offset;
    public EvaluatedType Type { get; } = type;
}