namespace Pycckuu
{
	public class Worder
	{
		public static Token ChangeType(in Token token, TokenType tokenType)
		{
			token.Type = tokenType;
			return token;
		}

		public static Token Word(Token word) => word.View switch 
		{ 
			"вещ" => ChangeType(word, TokenType.DOUBLEPRECISION),
			"цел" => ChangeType(word, TokenType.INT),

			"целое" => ChangeType(word, TokenType.DIV),
			"остаток" => ChangeType(word, TokenType.MOD),
			_ => Tokenizator.Nothing
		};
	}
}