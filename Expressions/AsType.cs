namespace pycckuu;

public sealed class AsTypeExpression(ICompilable value, Token type) : ICompilable
{
	public ICompilable Value { get; } = value;
	public Token Type = type;

	public object Evaluate() => throw new Exception("ПРЕОБРАЗОВАНИЕ ТИПОВ ДЛЯ ИНИТЕРПРЕТАТОРА НЕ СДЕЛАНО");

	public Instruction Compile()
	{
		throw new Exception("123123123123123123123123123123");
        /*
			=> Type.Type switch
	{
		TokenType.DIV or TokenType.INT => Comp.Str([

		]),
		TokenType.DOUBLEPRECISION => Comp.Str([
			Value.Compile(),
			"    pop r8",
			"    cvtsi2sd xmm0, r8 ; ПЕРЕВОД В ВЕЩЕСТВЕННОЕ",
			"    movq r8, xmm0",
			"    push r8",
			"",
		]),
		_ => throw new Exception("НЕ КОМПИЛИРУЕМОЕ БИНАРНОЕ ДЕЙСТВИЕ")
	};
			*/
    }

    public override string ToString() => $"{Value} КАК {Type.Type.View()}";
}
