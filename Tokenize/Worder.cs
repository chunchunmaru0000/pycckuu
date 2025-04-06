namespace pycckuu;

public class Worder
{
	public static Token ChangeType(in Token token, TokenType tokenType)
	{
		token.Type = tokenType;
		return token;
	}

	public static Token Word(Token word, string str) => str switch 
	{ 
		"вещ" => ChangeType(word, TokenType.DOUBLEPRECISION),
		"цел" => ChangeType(word, TokenType.INT),

		"целое" => ChangeType(word, TokenType.DIV),
		"остаток" => ChangeType(word, TokenType.MOD),

        "from" => ChangeType(word, TokenType.FROM),
        "из" => ChangeType(word, TokenType.FROM),

        "import" => ChangeType(word, TokenType.IMPORT),
        "черпать" => ChangeType(word, TokenType.IMPORT),
        "взять" => ChangeType(word, TokenType.IMPORT),

        "as" => ChangeType(word, TokenType.AS),
        "как" => ChangeType(word, TokenType.AS),

        "call" => ChangeType(word, TokenType.CALL),
        "зов" => ChangeType(word, TokenType.CALL),

        "type" => ChangeType(word, TokenType.TYPE),
        "типа" => ChangeType(word, TokenType.TYPE),
        "vararg" => ChangeType(word, TokenType.VARARG),
        "некого" => ChangeType(word, TokenType.VARARG),

        "let" => ChangeType(word, TokenType.LET),
        "пусть" => ChangeType(word, TokenType.LET),

        "be" => ChangeType(word, TokenType.BE),
        "это" => ChangeType(word, TokenType.BE),
        "будет" => ChangeType(word, TokenType.BE),
        "равно" => ChangeType(word, TokenType.BE),
        "равен" => ChangeType(word, TokenType.BE),
        "равна" => ChangeType(word, TokenType.BE),
        "например" => ChangeType(word, TokenType.BE),

        "PTR" => ChangeType(word, TokenType.PTR),
        "ptr" => ChangeType(word, TokenType.PTR),
        "УК" => ChangeType(word, TokenType.PTR),
        "ук" => ChangeType(word, TokenType.PTR),

        _ => word
    };
}