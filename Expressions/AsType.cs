namespace pycckuu;

public sealed class AsTypeExpression(ICompilable value, Token type) : ICompilable
{
	public ICompilable Value { get; } = value;
	public Token Type { get; } = type;

	public Instruction Compile() => new(Type.Type switch {
        TokenType.BYTE => EvaluatedType.BYTE,
        TokenType.DBYTE => EvaluatedType.DBYTE,
        TokenType.CHBYTE => EvaluatedType.CHBYTE,
        TokenType.VBYTE => EvaluatedType.VBYTE,
        TokenType.INTEGER => U.SizeToType[Convert.ToInt64(Type.Value!)],
        _ => throw new($"НЕИЗВЕСТНЫЙ ТИП ДЛЯ ПРЕОБРАЗОВАНИЯ {Type.Type.Log()}<{Type.Value}>")
    }, Value.Compile().Code);

    public override string ToString() => $"{Value} КАК {Type.Type.View()}";
}
