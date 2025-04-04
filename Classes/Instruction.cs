namespace pycckuu
{
    public class Instruction(string code, EvaluatedType type)
    {
        public string Code { get; set; } = code;
        public EvaluatedType Type { get; set; } = type;
    }
}
