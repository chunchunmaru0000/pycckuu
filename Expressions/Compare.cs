namespace pycckuu;

public class CompareExpression(ICompilable left, TokenType op, ICompilable right) : ICompilable
{
    private ICompilable Left { get; set; } = left;
    private TokenType Op { get; set; } = op;
    private ICompilable Right { get; set; } = right;

    public Instruction Compile() 
    {
        Instruction left = Left.Compile();
        Instruction right = Right.Compile();
        Exception e = new($"НЕВЕРНЫЙ ОПЕРАТОР [{Op.Log()}] ДЛЯ СРАВНЕНИЯ [{left.Type}] и [{right.Type}]");
        Exception un = new($"НЕВЕРНЫЙ ОПЕРАТОР [{Op.Log()}] ДЛЯ СРАВНЕНИЯ");

        return new(EvaluatedType.INT, Comp.Str([
            left.Code,
            right.Code,
            "    pop r9",
            "    pop r8",
            $"    ; {ToString()}",
            left.Type switch {
                EvaluatedType.INT => right.Type switch {
                    EvaluatedType.INT =>
                        "    ; INT и INT",
                    EvaluatedType.XMM => Comp.Str([
                        "    cvtsi2sd xmm6, r8",
                        "    movq r8, xmm6",
                    ]),
                    _ => throw e
                },
                EvaluatedType.XMM => right.Type switch {
                    EvaluatedType.INT => Comp.Str([
                        "    cvtsi2sd xmm6, r9",
                        "    movq r9, xmm6",
                    ]),
                    EvaluatedType.XMM =>
                        "    ; XMM и XMM",
                    _ => throw e
                },
                _ => throw e
            },
            "    xor r10, r10",
            "    cmp r8, r9",
            Op switch {
                TokenType.EQUALITY =>    "    sete r10b",
                TokenType.NOTEQUALITY => "    setne r10b",
                TokenType.MORE =>        "    setg r10b",
                TokenType.LESS =>        "    setl r10b",
                TokenType.MOREEQ =>      "    setge r10b",
                TokenType.LESSEQ =>      "    setle r10b",
                _ => throw un
            },
            "    push r10",
        ]));
    }

    public override string ToString() => $"лево {Op.Log()} право";
}
