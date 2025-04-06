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
        "numbera" => ChangeType(word, TokenType.NUMBERA),
        "числа" => ChangeType(word, TokenType.NUMBERA),
        "floata" => ChangeType(word, TokenType.FLOATA),
        "вещего" => ChangeType(word, TokenType.FLOATA),
        "stringa" => ChangeType(word, TokenType.STRINGA),
        "строки" => ChangeType(word, TokenType.STRINGA),
        "pointera" => ChangeType(word, TokenType.POINTERA),
        "указателя" => ChangeType(word, TokenType.POINTERA),
        "vararg" => ChangeType(word, TokenType.VARARG),
        "некого" => ChangeType(word, TokenType.VARARG),

        _ => word
    };
}