namespace pycckuu;

public class AndOrExpression(ICompilable left, TokenType op, ICompilable right): ICompilable
{
    private ICompilable Left { get; set; } = left;
    private TokenType Op { get; set; } = op;
    private ICompilable Right { get; set; } = right;

    public Instruction Compile()
    {
        Instruction left = Left.Compile();
        Instruction right = Right.Compile();
        Exception e = new($"НЕВЕРНЫЙ ОПЕРАТОР [{Op.Log()}] ДЛЯ СРАВНЕНИЯ [{left.Type}] и [{right.Type}]");

        return new(EvaluatedType.INT, Comp.Str([
            left.Code,
            right.Code,
            "    pop r9",
            "    pop r8",
            "    xor r10, r10",
            "    xor r11, r11",
            $"    ; {ToString()}",
            "    cmp r8, 0",
            "    setne r10b",
            "    cmp r9, 0",
            "    setne r11b",
            Op switch{
                TokenType.AND => "    and r10b, r11b",
                TokenType.OR =>  "    or  r10b, r11b",
                _ => throw e
            },
            "    push r10",
        ""]));
    }
}
