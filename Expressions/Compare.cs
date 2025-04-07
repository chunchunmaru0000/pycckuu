namespace pycckuu;

public class CompareExpression(ICompilable left, TokenType op, ICompilable right) : ICompilable
{
    private ICompilable Left { get; set; } = left;
    private TokenType Op { get; set; } = op;
    private ICompilable Right { get; set; } = right;

    private Exception un { get; } = new($"НЕВЕРНЫЙ ОПЕРАТОР [{op.Log()}] ДЛЯ СРАВНЕНИЯ");

    public Instruction Compile() => Op switch {
        // EQUALITY NOTEQUALITY MORE LESS MOREEQ LESSEQ
        //TokenType.EQUALITY =>
        //_ => throw un
    };

    public override string ToString() => $"лево {Op.Log()} право";
}
