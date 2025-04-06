namespace pycckuu;

partial class Parser
{
	private ICompilable Parentheses()
	{
		ICompilable result = CompilableExpression();
		Match(TokenType.RIGHTSCOB);
		return result;
	}

	private ICompilable AsType()
	{
		ICompilable result = Primary();
		if (Match(TokenType.AS)) {
			Token type = Consume(Current.Type);
			return new AsTypeExpression(result, type);
		}
		return result;
	}

    private ICompilable Ptrness()
    {
        ICompilable ptr;
        if (Match(TokenType.LCUBSCOB)) {
            ptr = new PtrExpression(Consume(TokenType.WORD).Value!.ToString()!);
            Consume(TokenType.RCUBSCOB);
        } else {
            Consume(TokenType.PTR);
            ptr = new PtrExpression(Consume(TokenType.WORD).Value!.ToString()!);
        }
        return ptr;
    }

	private ICompilable Primary()
	{
		Token current = Current;

		if (Match(TokenType.INTEGER))
			return new IntegerExpression(current);
		if (Match(TokenType.DOUBLE))
			return new DoubleExpression(current);
		if (Match(TokenType.LEFTSCOB))
			return Parentheses();
        if (Match(TokenType.STRING))
            return new StringExpression(current);
        if (Match(TokenType.WORD))
            return new VarExpression(current);
        if (current.Type == TokenType.PTR || current.Type == TokenType.LCUBSCOB)
            return Ptrness();

		throw new Exception("ЧЕ ЗА ТИП");
	}

	private ICompilable Unary()
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
			return sign < 0 ? new UnaryExpression(last, AsType()) : AsType();
		}
		if (Match(TokenType.MINUS, TokenType.PLUS))
			return new UnaryExpression(current, AsType());
		return AsType();
	}

	private ICompilable ModDiv()
	{
		ICompilable result = Unary();
		while (true)
		{
			Token current = Current;
			if (Match(TokenType.MOD, TokenType.DIV))
				result = new BinaryExpression(result, current, Unary());
			else
				break;
		}
		return result;
	}

	private ICompilable MulDivision()
	{
		ICompilable result = ModDiv();
		while (true)
		{
			Token current = Current;
			if (Match(TokenType.MULTIPLICATION, TokenType.DIVISION))
				result = new BinaryExpression(result, current, ModDiv());
			else
				break;
		}
		return result;
	}

	private ICompilable PlusMinus()
	{
		ICompilable result = MulDivision();
		while (true)
		{
			Token current = Current;
			if (Match(TokenType.PLUS, TokenType.MINUS))
				result = new BinaryExpression(result, current, MulDivision());
			else
				break;
		}
		return result;
	}
}