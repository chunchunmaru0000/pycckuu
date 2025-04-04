﻿namespace pycckuu;

partial class Parser
{
	public Token[] Tokens;
	private int Pos;
	private readonly int Lenght;

	public Parser(Token[] tokens)
	{
		Tokens = tokens;
		Pos = 0;
		Lenght = Tokens.Length;
	}

	private Token Get(int offset)
	{
		int offPos = Pos + offset;
		if (offPos < Lenght && offPos > -1)
			return Tokens[offPos];
		return Tokens.Last();
	}

	private Token Current => Get(0);

	private string Near(int range)
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine(Current.Location);
		Console.Write(string.Join("|", Enumerable.Range(-range, range).Select(g => Get(g).Type.View())));
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.Write('|' + Current.Type.View() + '|');
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine(string.Join("|", Enumerable.Range(1, range).Select(g => Get(g).Type.View())));
		Console.ResetColor();
		return "";
	}

	private Token Consume(TokenType type)
	{
		Token current = Current;
		if (Current.Type != type)
			throw new Exception($"{Near(6)}" +
								$"СЛОВО НЕ СОВПАДАЕТ С ОЖИДАЕМЫМ\n" +
								$"ОЖИДАЛСЯ: <{type.View()}>\n" +
								$"ТЕКУЩИЙ: <{Current.Type.View()}>");
		Pos++;
		return current;
	}

	private Token Consume(TokenType type0, TokenType type1)
	{
		Token current = Current;
		if (Current.Type != type0 && Current.Type != type1)
			throw new Exception($"{Near(6)}" +
								$"СЛОВО НЕ СОВПАДАЕТ С ОЖИДАЕМЫМ\n" +
								$"ОЖИДАЛСЯ: <{type0.View()}> ИЛИ <{type1.View()}>\n" +
								$"ТЕКУЩИЙ: <{Current.Type.View()}>");
		Pos++;
		return current;
	}

	private bool Match(TokenType type)
	{
		if (Current.Type != type)
			return false;
		Pos++;
		return true;
	}

	private bool Match(TokenType type0, TokenType type1)
	{
		if (Current.Type != type0 && Current.Type != type1)
			return false;
		Pos++;
		return true;
	}

	private bool Match(TokenType type0, TokenType type1, TokenType type2)
	{
		if (Current.Type != type0 && Current.Type != type1 && Current.Type != type2)
			return false;
		Pos++;
		return true;
	}

	private IExpression ExpressionTree() => PlusMinus();

	private ICompilable ExpressionInstructions() => PlusMinus();

	private ICompilableExpression CompilableExpression() => PlusMinus();

	public IExpression ParseTree()
	{
		return ExpressionTree();
	}

	public ICompilable ParseInstructions()
	{
		return ExpressionInstructions();
	}
}