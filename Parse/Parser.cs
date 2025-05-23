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

    #region TOOLS

    private Token Get(int offset)
	{
		int offPos = Pos + offset;
		if (offPos < Lenght && offPos > -1)
			return Tokens[offPos];
		return Tokens.Last();
	}

	private Token Current => Get(0);

	private static string TStr(Token t) =>
        t.Value == null ? t.Type.Log() : t.Value!.ToString()!;


    private string Near(int range)
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine(Current.Location);
		Console.Write(string.Join("|", Enumerable.Range(-range, range).Select(g => TStr(Get(g)))));
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.Write('|' + Current.Type.View() + '|');
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine(string.Join("|", Enumerable.Range(1, range).Select(g => TStr(Get(g)))));
		Console.ResetColor();
		return "";
	}

	private Token Consume(TokenType type)
	{
		Token current = Current;
		if (Current.Type != type)
			throw new Exception($"{Near(6)}" +
								$"СЛОВО НЕ СОВПАДАЕТ С ОЖИДАЕМЫМ\n" +
								$"ОЖИДАЛСЯ: <{type.Log()}>\n" +
								$"ТЕКУЩИЙ: <{TStr(Current)}>");
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

    #endregion TOOLS

    private ICompilable CompilableExpression() => Ternary();

    private ICompilable CompilableStatement()
    {
        Token current = Current;
        Token next = Get(1);

        return current.Type switch {
            TokenType.FROM =>  Import(false),
            TokenType.OUTOF => Import(true),
            TokenType.CALL => Call(),
            TokenType.LET => Let(),
            TokenType.WORD_IF => If(),
			TokenType.LOOP => Loop(),
			TokenType.BREAK => Break(),
			TokenType.CONTINUE => Continue(),
			TokenType.WORD_WHILE => While(),
			TokenType.GLORY => DeclareFunction(),
			TokenType.RETURN => Return(),
            TokenType.LEFTSCOB => Block(false),
            _ => throw U.YetCantEx($"{current.Type.Log()}[{current.Value}]", $"ICompilable StatementInstructions()\n{Near(6)}")
        };
    }

	public string ParseInstructions()
	{
        List<string> statements = [];
        while (!Match(TokenType.EOF))
            statements.Add(CompilableStatement().Compile().Code);
        return Comp.Str([.. statements]);
	}
}