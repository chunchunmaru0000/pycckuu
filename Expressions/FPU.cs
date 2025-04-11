namespace pycckuu;

public class FPUExpression(TokenType op, ICompilable value) : ICompilable
{
    public TokenType Op { get; } = op;
    public ICompilable Value { get; } = value;

    public Instruction Compile()
    {
        Instruction value = Value.Compile();
        return new(EvaluatedType.XMM, Comp.Str([
            value.Code,
            "    pop r8",
            value.Type switch {
                EvaluatedType.XMM => 
                    "    mov qword[FOR_FPU_RESERVED], r8",
                EvaluatedType.INT or EvaluatedType.BYTE or EvaluatedType.DBYTE => Comp.Str([
                    "    cvtsi2sd xmm6, r8",
                    "    movsd qword[FOR_FPU_RESERVED], xmm6",
                ]),
                _ => throw new($"ПОКА НЕ СДЕЛАНО FPU ДЛЯ {value.Type.Log()}"),
            },
            "    fld qword[FOR_FPU_RESERVED]",
           $"    {Op.Log()} ; {Op.Log()}",
            "    fst qword[FOR_FPU_RESERVED]",
            "    mov r8, qword[FOR_FPU_RESERVED]",
            "    push r8",
        ]));
    }
}
