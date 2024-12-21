using System;
using System.Security.Principal;

namespace Pycckuu
{
	class Tokenizator
	{
		private readonly string code;
		private int position;
		private int line;
		private int location;
		private int startLine;
		private bool commented = false;
		public static Token Nothing = new() { View = "", Value = null, Type = TokenType.WHITESPACE, Location = new Location(-1, -1) };
		private readonly static Dictionary<char, char> RusCharInString = new() { { 'н', '\n' }, { 'т', '\t' }, { '\\', '\\' } };

		public Tokenizator(string code)
		{
			this.code = code;
			position = 0;
			line = 1;
			location = position;
			startLine = 0;
		}

		private char Current
		{
			get
			{
				if (position < code.Length)
					return code[position];
				return '\0';
			}
		}

		private void Next()
		{
			position++;
			if (Current == '\n')
			{
				Next();
				startLine = position;
				line++;
				location = 0;
			}
			location = position - startLine;
		}

		private Location Loc() => new Location(line, location);

		public static bool Usable(char c)
		{
			return c != '+' && c != '-' && c != '*' && c != '/' && c != '%' &&
				   c != '(' && c != ')' && c != '[' && c != ']' && c != '{' && c != '}' && c != '|' &&
				   c != '@' && c != ';' && c != '.' && c != ',' && c != '"' && c != ':' && c != '?' &&
				   c != 'Ё' && c != '\n' && c != ' ' &&
				   c != '<' && c != '>' && c != '!' && c != '=' &&
				   c != '\0';
		}

		private Token DoNextAndGiveToken(string view, object? value, TokenType type)
		{
			Next();
			return new() { View = view, Value = value, Type = type, Location = Loc() };
		}

		private Token DoNextAndGiveToken(Token token)
		{
			Next();
			return token;
		}

		private Token StringToken()
		{
			if (!commented)
			{
				Next();
				string buffer = "";
				while (Current != '"' || Current != '\'')
				{
					while (true)
					{
						if (Current == '\\')
						{
							Next();
							char cur = Current;
							Next();
							if (RusCharInString.ContainsKey(cur))
								buffer += RusCharInString[cur];
							continue;
						}
						break;
					}
					if (Current == '"' || Current == '\'')
						break;

					buffer += Current;
					Next();

					if (Current == '\0')
						throw new Exception($"НЕЗАКОНЧЕНА СТРОКА: <{Loc()}>, буфер<{buffer}>");
				}
				return DoNextAndGiveToken(buffer, buffer, TokenType.STRING);
			}
			else
				return DoNextAndGiveToken(Nothing);
		}

		private Token NumberToken()
		{
			int start = position;
			int dots = 0;
			while (char.IsDigit(Current) || Current == '.')
			{
				if (Current == '.')
					dots++;
				if (dots > 1)
				{
					dots--;
					break;
				}
				Next();
			}
			string word = code.Substring(start, position - start).Replace('.', ',');
			if (dots == 0)
			{
				if (long.TryParse(word, out long res))
					return new Token() { View = word, Value = res, Type = TokenType.INTEGER, Location = Loc() };
				throw new Exception($"ЧИСЛО <{word}> БЫЛО СЛИШКОМ ВЕЛИКО ИЛИ МАЛО ДЛЯ ПОДДЕРЖИВАЕМЫХ СЕЙЧАС ЧИСЕЛ");
			}
			if (dots == 1)
			{
				if (double.TryParse(word, out double res))
					return new Token() { View = word, Value = res, Type = TokenType.DOUBLE, Location = Loc() };
				throw new Exception($"ЧИСЛО <{word}> БЫЛО СЛИШКОМ ВЕЛИКО ИЛИ МАЛО ДЛЯ ПОДДЕРЖИВАЕМЫХ СЕЙЧАС ЧИСЕЛ");
			}
			throw new Exception("МНОГА ТОЧЕК ДЛЯ ЧИСЛА");
		}

		private Token NextToken()
		{
			if (Current == '\0')
				return new Token() { View = null, Value = null, Type = TokenType.EOF, Location = Loc() };

			if (char.IsWhiteSpace(Current))
				while (char.IsWhiteSpace(Current))
					Next();

			if (Current == '"' || Current == '\'')
				return StringToken();

			if (char.IsDigit(Current))
				return NumberToken();

			if (Usable(Current))
			{
				int start = position;
				while (Usable(Current))
					Next();
				string word = code.Substring(start, position - start);
				return Worder.Word(new Token() { View = word, Value = null, Type = TokenType.WORD, Location = Loc() });
			}

			switch (Current)
			{
				case '=':
					Next();
					if (Current == '=')
					{
						Next();
						if (Current == '=')
							return DoNextAndGiveToken("===", null, TokenType.ARROW);
						return new Token() { View = "==", Value = null, Type = TokenType.EQUALITY, Location = Loc() };
					}
					if (Current == '>')
						return DoNextAndGiveToken("=>", null, TokenType.ARROW);
					return new Token() { View = "=", Value = null, Type = TokenType.DO_EQUAL, Location = Loc() };
				case '/':
					Next();
					if (Current == '/')
						return DoNextAndGiveToken("//", null, TokenType.DIV);
					if (Current == '=')
						return DoNextAndGiveToken("/=", null, TokenType.DIVEQ);
					return new Token() { View = "/", Value = null, Type = TokenType.DIVISION, Location = Loc() };
				case '!':
					Next();
					if (Current == '=')
						return DoNextAndGiveToken("!=", null, TokenType.NOTEQUALITY);
					return new Token() { View = "!", Value = null, Type = TokenType.NOT, Location = Loc() };
				case '*':
					Next();
					if (Current == '*')
						return DoNextAndGiveToken("**", null, TokenType.POWER);
					if (Current == '=')
						return DoNextAndGiveToken("*=", null, TokenType.MULEQ);
					return new Token() { View = "*", Value = null, Type = TokenType.MULTIPLICATION, Location = Loc() };
				case '+':
					Next();
					if (Current == '+')
						return DoNextAndGiveToken("++", null, TokenType.PLUSPLUS);
					if (Current == '=')
						return DoNextAndGiveToken("+=", null, TokenType.PLUSEQ);
					return new Token() { View = "+", Value = null, Type = TokenType.PLUS, Location = Loc() };
				case '-':
					Next();
					if (Current == '-')
						return DoNextAndGiveToken("--", null, TokenType.MINUSMINUS);
					if (Current == '=')
						return DoNextAndGiveToken("-=", null, TokenType.MINUSEQ);
					return new Token() { View = "-", Value = null, Type = TokenType.MINUS, Location = Loc() };
				case '<':
					Next();
					if (Current == '=')
						return DoNextAndGiveToken("<=", null, TokenType.LESSEQ);
					return new Token() { View = "<", Value = null, Type = TokenType.LESS, Location = Loc() };
				case '>':
					Next();
					if (Current == '=')
						return DoNextAndGiveToken(">=", null, TokenType.MOREEQ);
					return new Token() { View = ">", Value = null, Type = TokenType.MORE, Location = Loc() };
				case '@':
					return DoNextAndGiveToken("@", null, TokenType.DOG);
				case ';':
					return DoNextAndGiveToken(";", null, TokenType.SEMICOLON);
				case '(':
					return DoNextAndGiveToken("(", null, TokenType.LEFTSCOB);
				case ')':
					return DoNextAndGiveToken(")", null, TokenType.RIGHTSCOB);
				case '[':
					return DoNextAndGiveToken("[", null, TokenType.LCUBSCOB);
				case ']':
					return DoNextAndGiveToken("]", null, TokenType.RCUBSCOB);
				case '{':
					return DoNextAndGiveToken("{", null, TokenType.LTRISCOB);
				case '}':
					return DoNextAndGiveToken("}", null, TokenType.RTRISCOB);
				case '%':
					return DoNextAndGiveToken("%", null, TokenType.MOD);
				case '.':
					Next();
					if (Current == '.')
					{
						Next();
						if (Current == '=')
							return DoNextAndGiveToken("..=", null, TokenType.DOTDOTEQ);
						return new Token() { View = "..", Value = null, Type = TokenType.DOTDOT, Location = Loc() };
					}
					return new Token() { View = ".", Value = null, Type = TokenType.DOT, Location = Loc() };
				case ',':
					return DoNextAndGiveToken(",", null, TokenType.COMMA);
				case 'Ё':
					commented = !commented;
					return DoNextAndGiveToken("Ё", null, TokenType.COMMENTO);
				case '\n':
					return DoNextAndGiveToken("\n", null, TokenType.SLASH_N);
				case ':':
					return DoNextAndGiveToken(":", null, TokenType.COLON);
				case '?':
					return DoNextAndGiveToken("?", null, TokenType.QUESTION);
				case '|':
					return DoNextAndGiveToken("|", null, TokenType.STICK);
				case '\0':
					return new Token() { View = null, Value = null, Type = TokenType.EOF, Location = Loc() };
				default:
					throw new Exception($"{Loc()}\nНЕ СУЩЕСТВУЮЩИЙ СИМВОЛ В ДАННОМ ЯЗЫКЕ <{Current}> <{(int)Current}>");
			}
		}

		public Token[] Tokenize()
		{
			List<Token> tokens = [];
			while (true)
			{
				Token token = NextToken();
				if (token.Type == TokenType.COMMENTO)
					while (true)
					{
						token = NextToken();
						if (token.Type == TokenType.EOF || token.Type == TokenType.COMMENTO)
						{
							token = NextToken();
							break;
						}
					}
				if (token.Type != TokenType.WHITESPACE)
					tokens.Add(token);
				if (token.Type == TokenType.EOF)
					break;
			}
			return [.. tokens];
		}
	}
}