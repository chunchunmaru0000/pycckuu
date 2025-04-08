namespace pycckuu;

class BoolExpression(TokenType value) : ICompilable
{
    private bool Value = value == TokenType.WORD_TRUE;

    public Instruction Compile() => new(EvaluatedType.INT, Comp.Str([
        $"    push qword {(Value ? "1" : "0")} ; {this}",
    ]));

    public override string ToString() => Value ? "истина" : "ложь"; 
}
