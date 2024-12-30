using System.Linq.Expressions;

namespace Pycckuu
{
	partial class Parser
	{
		private ICompilableExpression Parentheses()
		{
			ICompilableExpression result = CompilableExpression();
			Match(TokenType.RIGHTSCOB);
			return result;
		}

		private ICompilableExpression Primary()
		{
			Token current = Current;

			if (Match(TokenType.INTEGER))
				return new IntegerExpression(current);
			if (Match(TokenType.DOUBLE))
				return new DoubleExpression(current);
			if (Match(TokenType.LEFTSCOB))
				return Parentheses();

			throw new Exception("EOF");
		}

		private ICompilableExpression Unary()
		{
			Token current = Current;
			Token last = current;
			int sign = -1;
			if (Match(TokenType.NOT))
			{
				while (true)
				{
					current = Current;
					if (Match(TokenType.NOT))
					{
						sign *= -1;
						last = current;
						continue;
					}
					break;
				}
				return sign < 0 ? new UnaryExpression(last, Primary()) : Primary();
			}
			if (Match(TokenType.MINUS, TokenType.PLUS))
				return new UnaryExpression(current, Primary());
			return Primary();
		}

		private ICompilableExpression MulDiv()
		{
			ICompilableExpression result = Unary();
			while (true)
			{
				Token current = Current;
				if (Match(TokenType.MULTIPLICATION, TokenType.DIVISION))
					result = new BinaryExpression(result, current, Primary());
				else
					break;
			}
			return result;
		}

		private ICompilableExpression PlusMinus()
		{
			ICompilableExpression result = MulDiv();
			while (true)
			{
				Token current = Current;
				if (Match(TokenType.PLUS, TokenType.MINUS))
					result = new BinaryExpression(result, current, MulDiv());
				else
					break;
			}
			return result;
		}
	}
}