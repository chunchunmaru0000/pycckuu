namespace Pycckuu
{
	partial class Parser
	{
		private ICompilableExpression Primary()
		{
			Token current = Current;

			if (Match(TokenType.INTEGER))
				return new IntegerExpression(current);
			if (Match(TokenType.DOUBLE))
				return new DoubleExpression(current);

			throw new Exception("EOF");
		}

		private ICompilableExpression PlusMinus()
		{
			ICompilableExpression result = Primary();
			while (true)
			{
				Token current = Current;
				if (Match(TokenType.PLUS, TokenType.MINUS))
				{
					result = new BinaryExpression(result, current, Primary());
					continue;
				}
				break;
			}
			return result;
		}
	}
}