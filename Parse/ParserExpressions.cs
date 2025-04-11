namespace pycckuu;

partial class Parser
{
	private ICompilable Parentheses()
	{
		ICompilable result = CompilableExpression();
		Match(TokenType.RIGHTSCOB);
		return result;
	}

	private ICompilable After()
	{
		ICompilable result = Primary();

		if (Match(TokenType.AS))
			return new AsTypeExpression(result, Consume(Current.Type));
        /*
		if (Match(TokenType.WORD_THEN)) {
            ICompilable tru = CompilableExpression();
            Match(TokenType.WORD_ELSE);
            ICompilable fal = CompilableExpression();
            return new TernaryExpression(result, tru, fal);
        }
         */

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
        if (Match(TokenType.BYTE, TokenType.DBYTE))
            return new BytesExpression(current);
        if (Match(TokenType.WORD))
            return new VarExpression(current);
        if (current.Type == TokenType.PTR || current.Type == TokenType.LCUBSCOB)
            return Ptrness();
        if (Match(TokenType.YO))
            return new YoExpression(CompilableExpression());
        if (Match(TokenType.WORD_TRUE, TokenType.WORD_FALSE))
            return new BoolExpression(current.Type);
        if ((int)current.Type >= (int)TokenType.FSINCOS && (int)current.Type <= (int)TokenType.FLOG10)
            return new FPUExpression(Consume(current.Type).Type, CompilableExpression());

        //throw new Exception("ЧЕ ЗА ТИП");
        return CompilableStatement();
	}

	private ICompilable Unary()
	{
		Token current = Current;
		Token last = current;
		int sign = -1;
		if (Match(TokenType.NOT)) {
			while (true) {
				current = Current;
				if (Match(TokenType.NOT)) {
					sign *= -1;
					last = current;
					continue;
				}
				break;
			}
			return sign < 0 ? new UnaryExpression(last, After()) : After();
		}
		if (Match(TokenType.MINUS, TokenType.PLUS))
			return new UnaryExpression(current, After());
		return After();
	}

	private ICompilable ModDiv()
	{
		ICompilable result = Unary();
		while (true) {
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
		while (true) {
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
		while (true) {
			Token current = Current;
			if (Match(TokenType.PLUS, TokenType.MINUS))
				result = new BinaryExpression(result, current, MulDivision());
			else
                break;
		}
		return result;
	}

    private ICompilable Compare()
    {
        ICompilable result = PlusMinus();
        while (true){ // EQUALITY NOTEQUALITY MORE LESS MOREEQ LESSEQ
            Token current = Current;
            Token next = Get(1);
            if (Match(TokenType.EQUALITY, TokenType.BE) || Match(TokenType.DO_EQUAL, TokenType.ARROW))
                result = new CompareExpression(result, TokenType.EQUALITY, PlusMinus());
            else if (
                    Match(TokenType.NOTEQUALITY, TokenType.MORE, TokenType.LESS) ||
                    Match(TokenType.MOREEQ, TokenType.LESSEQ))
                result = new CompareExpression(result, current.Type, PlusMinus());
            else if (current.Type == TokenType.NOT && 
                    (next.Type == TokenType.BE || next.Type == TokenType.EQUALITY ||
                     next.Type == TokenType.DO_EQUAL || next.Type == TokenType.ARROW)) {
                Consume(TokenType.NOT);
                if (!Match(TokenType.DO_EQUAL, TokenType.ARROW))
                    Consume(TokenType.BE, TokenType.EQUALITY);
                result = new CompareExpression(result, TokenType.NOTEQUALITY, PlusMinus());
            } else if (current.Type == TokenType.NOT && next.Type == TokenType.MORE) {
                Consume(TokenType.NOT);
                Consume(TokenType.MORE);
                result = new CompareExpression(result, TokenType.LESSEQ, PlusMinus());
            } else if (current.Type == TokenType.NOT && next.Type == TokenType.LESS) {
                Consume(TokenType.NOT);
                Consume(TokenType.LESS);
                result = new CompareExpression(result, TokenType.MOREEQ, PlusMinus());
            }
            else
                break;
        }
        return result;
    }

    private ICompilable And()
    {
        ICompilable result = Compare();
        while (true) {
            Token current = Current;
            if (Match(TokenType.AND))
                result = new AndOrExpression(result, current.Type, Compare());
            else
                break;
        }
        return result;
    }

    private ICompilable Or()
    {
        ICompilable result = And();
        while (true) {
            Token current = Current;
            if (Match(TokenType.OR, TokenType.XOR))
                result = new AndOrExpression(result, current.Type, And());
            else
                break;
        }
        return result;
    }

    private ICompilable Ternary()
    {
        ICompilable result = Or();

        while (true) { 
            if (Match(TokenType.WORD_THEN)) {
                ICompilable tru = Or();
                Match(TokenType.WORD_ELSE);
                ICompilable fal = Or();
                result = new TernaryExpression(result, tru, fal);
            }
            else
                break;
        }
        return result;
    }
}