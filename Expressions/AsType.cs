namespace pycckuu;

public sealed class AsTypeExpression(ICompilable value, Token type) : ICompilable
{
	public ICompilable Value { get; } = value;
	public Token Type { get; } = type;

    public Instruction Compile() 
    {
        EvaluatedType type = Type.Type switch {
            TokenType.BYTE => EvaluatedType.BYTE,
            TokenType.DBYTE => EvaluatedType.DBYTE,
            TokenType.CHBYTE => EvaluatedType.CHBYTE,
            TokenType.VBYTE => EvaluatedType.VBYTE,
            TokenType.INTEGER => U.SizeToType[Convert.ToInt64(Type.Value!)],
            TokenType.INT => EvaluatedType.INT,
            TokenType.DOUBLEPRECISION => EvaluatedType.XMM,
            _ => throw new($"НЕИЗВЕСТНЫЙ ТИП ДЛЯ ПРЕОБРАЗОВАНИЯ {Type.Type.Log()}<{Type.Value}>")
        };

        Instruction value = Value.Compile();
        return new(type, ConvertCode(value.Type, Type.Type, value.Code));
    }

    private string ConvertCode(EvaluatedType from, TokenType to, string code) =>
        to switch {
            TokenType.BYTE => code,
            TokenType.DBYTE => code,
            TokenType.CHBYTE => code,
            TokenType.VBYTE => code,
            TokenType.INTEGER => code,
            TokenType.INT => from switch {
                EvaluatedType.XMM => Comp.Str([
                    code,
                    "    pop r8",
                    "    movq xmm6, r8",
                    "    cvtsd2si r8, xmm6",
                    "    push r8",
                ]),
                _ => code
            },
            TokenType.DOUBLEPRECISION => from switch {
                EvaluatedType.INT => Comp.Str([
                    code,
                    "    pop r8",
                    "    cvtsi2sd xmm6, r8",
                    "    movq r8, xmm6",
                    "    push r8",
                ]),
                _ => throw new($"КОНВЕРТАЦИЯ ИЗ [{from.Log()}] В [{to.Log()}] НАПРЯМУЮ НЕВОЗМОЖНО, ПОПРОБУЙТЕ СНАЧАЛА СКОНВЕРТИРОВАТЬ В [ВЕЩ]")
            },
            _ => throw new($"ТИП [{to.Log()}]{Type.Location} НЕ МОЖЕТ ИСПОЛЬЗОВАТЬСЯ ДЛЯ КОНВЕРТИРОВАНИЯ В НЕГО")
        };

    public override string ToString() => $"{Value} КАК {Type.Type.View()}";
}
