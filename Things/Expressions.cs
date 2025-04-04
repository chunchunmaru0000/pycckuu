namespace pycckuu
{
	public sealed class IntegerExpression(Token token): ICompilableExpression
	{
		public long Value { get; } = Convert.ToInt64(token.Value);

		public object Evaluate() => Value;

		public Instruction Compile() => new(EvaluatedType.INT,
            Comp.Str([
			   $"    mov r8, {Value} ; ЦЕЛОЕ ЧИСЛО {Value}",
				"    push r8",
				"",
			]));

		public override string ToString() => token.Type.View();
	}

	public sealed class DoubleExpression(Token token) : ICompilableExpression
	{
		public double Value { get; } = Convert.ToDouble(token.Value);

		public object Evaluate() => Value;

		public Instruction Compile() => new(EvaluatedType.XMM, 
			Comp.Str([
			   $"    mov r8, {Value} ; ВЕЩЕСТВЕННОЕ ЧИСЛО {Value}", //  для вставления в стэк чисел с плавающей точкой
				"    push r8",
				"",
			]));

		public override string ToString() => token.Type.View();
	}

	public sealed class UnaryExpression(Token op, ICompilableExpression value) : ICompilableExpression
	{
		private Token Op = op;
		private ICompilableExpression Value = value;

		public object Evaluate()
		{
			throw new Exception("НЕ ВЫЧИСЛИМОЕ ЗНАЧЕНИЕ");
		}

		public Instruction Compile()
		{
			Instruction instruction = Value.Compile();

			return Op.Type switch {
				TokenType.PLUS => instruction,
				TokenType.MINUS => instruction.Type switch {
                    EvaluatedType.INT => new(EvaluatedType.INT, Comp.Str([
                        instruction.Code,
						"    pop r8",
						"    neg r8 ; ПОМЕНЯТЬ ЗНАК",
						"    push r8",
						""])),
                    EvaluatedType.XMM => new(EvaluatedType.XMM, Comp.Str([
						instruction.Code,
						"	pop r8",
						"	movq xmm6, r8",
                        "	mulsd xmm6, [MINUS_ONE]",
						"	movq r8, xmm6",
						"	push r8",
						""])),
                    EvaluatedType.BOOL => throw U.YetCantEx("BOOL", "UnaryExpression"),
                    EvaluatedType.STR => throw U.YetCantEx("STR", "UnaryExpression, реверсить строку будет наверно чебы нет"),
                    _ => throw new Exception("НЕ ЧИСЛОВОЙ ТИП ПРИ ПОПЫТКЕ ПОМЕНЯТЬ ЗНАК")
                },
				_ => throw new Exception("НЕ КОМПИЛИРУЕМОЕ БИНАРНОЕ ДЕЙСТВИЕ")
			};
        }

		public override string ToString() => $"{Op.Type.View()}{Value}";
	}

	public sealed class BinaryExpression(ICompilableExpression left, Token op, ICompilableExpression right) : ICompilableExpression
	{
		private ICompilableExpression Left = left;
		private Token Op = op;
		private ICompilableExpression Right = right;

		public object Evaluate()
		{
			object left = Left.Evaluate();
			object right = Right.Evaluate();

			if (left is long && right is long)
			{
				long lleft = Convert.ToInt64(left);
				long lright = Convert.ToInt64(right);
				switch (Op.Type)
				{
					case TokenType.PLUS:
						return lleft + lright;
					case TokenType.MINUS:
						return lleft - lright;
					default:
						break;
				}
			}
			throw new Exception("НЕ ВЫЧИСЛИМОЕ ЗНАЧЕНИЕ");
		}

		public Instruction Compile() => Op.Type switch 
		{
			TokenType.PLUS => Comp.Str([
				Left.Compile(),
				Right.Compile(),
				$"; {ToString()}",
				"    pop r8",
				"    pop r9",
				"    add r8, r9 ; ПЛЮС",
				"    push r8",
				"",
			]),
			TokenType.MINUS => Comp.Str([
				Left.Compile(),
				Right.Compile(),
				$"; {ToString()}",
				"    pop r8",
				"    pop r9",
				"    sub r8, r9 ; МИНУС",
				"    push r8",
				"",
			]),
			TokenType.MULTIPLICATION => Comp.Str([
				Left.Compile(),
				Right.Compile(),
				$"; {ToString()}",
				"    pop r8",
				"    pop r9",
				"    imul r8, r9 ; УМНОЖЕНИЕ",
				"    push r8",
				"",
			]),
			TokenType.DIVISION => Comp.Str([
				Left.Compile(),
				Right.Compile(),
				$"; {ToString()}",
				"    pop r8",
				"    pop rax",
				"    xor rdx, rdx",
				"    cqo ; РАСШИРЯЕТ ЗНАК С RAX В RDX",
				"    idiv r8 ; ДЕЛЕНИЕ ИСПОЛЬЗУЕТ ЧИСЛО 128БИТ RDX:RAX ЗНАКОВОЕ",
				"    push rax",
				"",
			]),
			TokenType.DIV => Comp.Str([
				Left.Compile(),
				Right.Compile(),
				"    pop r8",
				"    pop rax",
				"    xor rdx, rdx",
				"    cqo ; РАСШИРЯЕТ ЗНАК С RAX В RDX",
				"    idiv r8 ; ЦЕЛОЕ ИСПОЛЬЗУЕТ ЧИСЛО 128БИТ RDX:RAX ЗНАКОВОЕ",
				"    push rax",
			]),
			TokenType.MOD => Comp.Str([
				Left.Compile(),
				Right.Compile(),
				"    pop r8",
				"    pop rax",
				"    xor rdx, rdx",
				"    cqo ; РАСШИРЯЕТ ЗНАК С RAX В RDX",
				"    idiv r8 ; ДЕЛЕНИЕ ИСПОЛЬЗУЕТ ЧИСЛО 128БИТ RDX:RAX ЗНАКОВОЕ",
				"    push rdx",
			]),
			_ => throw new Exception("НЕ КОМПИЛИРУЕМОЕ БИНАРНОЕ ДЕЙСТВИЕ")
		};

		public override string ToString() => $"{Left} {Op.Type.View()} {Right}";
	}

	public sealed class AsTypeExpression(ICompilableExpression value, Token type) : ICompilableExpression
	{
		public ICompilableExpression Value { get; } = value;
		public Token Type = type;

		public object Evaluate() => throw new Exception("ПРЕОБРАЗОВАНИЕ ТИПОВ ДЛЯ ИНИТЕРПРЕТАТОРА НЕ СДЕЛАНО");

		public string Compile() => Type.Type switch
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

		public override string ToString() => $"{Value} КАК {Type.Type.View()}";
	}
}