namespace pycckuu;

public class Variable(Token name, EvaluatedType type)
{
    public Token Name { get; set; } = name;
    public EvaluatedType Type { get; set; } = type;
}
