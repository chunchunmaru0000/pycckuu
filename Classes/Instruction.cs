namespace pycckuu;

public class Instruction(EvaluatedType type, string code)
{
    public string Code { get; set; } = code;
    public EvaluatedType Type { get; set; } = type;
}
