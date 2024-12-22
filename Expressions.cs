namespace Pycckuu
{
	public sealed class IntegerExpression(Token token): ICompilableExpression
	{
		public long Value { get; } = Convert.ToInt64(token.Value);

		public object Evaluate() => Value;

		public string Compile()
		{
			return string.Join("\r\n", [
				$"mov r8, {Value}",
				"push r8",
				]);
		}

		public override string ToString() => token.View!;
	}

	public sealed class DoubleExpression(Token token) : ICompilableExpression
	{
		public double Value { get; } = Convert.ToDouble(token.Value);

		public object Evaluate() => Value;

		public string Compile()
		{
			return string.Join("\r\n", [
				$"mov , {Value}", //  для вставления в стэк чисел с плавающей точкой
				"push r9",
				]);
		}

		public override string ToString() => token.View!;
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

			throw new Exception("asdf");
		}

		public string Compile()
		{
			
		}

		public override string ToString() => $"{Left} {Op.View} {Right}";
	}
}