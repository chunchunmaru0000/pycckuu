namespace pycckuu;

class Tokenizator
{
	private readonly string code;
	private int position;
	private int line;
	private int location;
	private int startLine;
	public static Token Nothing = new() { Value = null, Type = TokenType.WHITESPACE, Location = new Location(-1, -1) };
	private readonly static Dictionary<char, char> RusCharInString = new() { { 'н', '\n' }, { 'т', '\t' }, { '\\', '\\' } };

	public Tokenizator(string code)
	{
		this.code = code.Replace("\r", "");
		position = 0;
		line = 1;
		location = position;
		startLine = 0;
	}

	private char Current { get => position < code.Length ? code[position] : '\0'; }

    private char Get(int off)
    {
        int offPos = position + off;
        return offPos < code.Length ? code[offPos] : '\0';
    }

	private void Next()
	{
		position++;
		if (Current == '\n') {
			Next();
            startLine = position;
			line++;
			location = 0;
		}
		location = position - startLine;
	}

	private Location Loc() => new(line, location);

    private static readonly char[] UnUsableChars = [
        '+', '-', '*', '/', '%', '(', ')', '{', '}', '[', ']', '|', '@', ';', '.', ',', '"', ':',
        '?', 'Ё', 'ё', '\n', '\t', '\0', '\r', ' ', '<', '>', '!', '='
    ];

    public static bool Usable(char c) => !UnUsableChars.Contains(c);

	private Token DoNextAndGiveToken(object? value, TokenType type)
	{
		Next();
		return new() { Value = value, Type = type, Location = Loc() };
	}

	private Token StringToken(bool isPrintfFormatString)
	{
        Next();
        string buffer = "";
        while (Current != '"') {
            while (true) {
                if (Current == '\\') {
                    Next();
					char cur = Current;
                    Next();
					if (RusCharInString.ContainsKey(cur))
						buffer += RusCharInString[cur];
                    continue;
                }
                break;
            }
            if (Current == '"')
                break;

            buffer += Current;
            Next();

            if (Current == '\0')
				throw new Exception($"НЕЗАКОНЧЕНА СТРОКА: <{Loc()}>, буфер<{buffer}>");
        }
        buffer = buffer
            .Replace("'", "',39,'")
            .Replace("\n", "',10,'")
            .Replace("\t", "',9,'")
            ;
        if (isPrintfFormatString)
            buffer = PrintfFormatString(buffer);
        return DoNextAndGiveToken(buffer, TokenType.STRING);
	}

    private static string PrintfFormatString(string str) => 
        str
        .Replace("%ллд", "%lld").Replace("%ллф", "%llf")
        .Replace("%х", "%x").Replace("%Х", "%X")
        .Replace("%с", "%s").Replace("%ч", "%c")
        .Replace("%п", "%p").Replace("%н", "%n")
        .Replace("%д", "%d").Replace("%ф", "%f")
        .Replace("%а", "%a").Replace("%А", "%A");

    private static readonly char[] Base2 =  ['b', 'б', 'B', 'Б'];
	private static readonly char[] Base8 =  ['o', 'о', 'O', 'О'];
	private static readonly char[] Base16 = ['x', 'х', 'X', 'Х'];
	private static readonly char[][] AllBases = [Base2, Base8, Base16];

	private static bool IsBaseChar(char c) => AllBases.Any(b => b.Contains(c));

	private static void IncNum(ref int num, Func<int, string> message)
	{
        num++;
		if (num > 1)
			throw new(message(num));
    }

    private Token NumberToken()
	{
		int start = position;
		int dots = 0, xs = 0, os = 0, bs = 0;
		while (char.IsDigit(Current) || Current == '.' || IsBaseChar(Current) || Current == '_') {
			if (Current == '.')
                IncNum(ref dots, (n) => $"МНОГА ТОЧЕК ДЛЯ ЧИСЛА БЫЛО {n}");
			else if (Base2.Contains(Current))
                IncNum(ref bs, (n) => $"МНОГА [б] ДЛЯ ЧИСЛА БЫЛО {n}");
            else if (Base8.Contains(Current))
                IncNum(ref os, (n) => $"МНОГА [о] ДЛЯ ЧИСЛА БЫЛО {n}");
            else if (Base16.Contains(Current))
                IncNum(ref xs, (n) => $"МНОГА [х] ДЛЯ ЧИСЛА БЫЛО {n}");
            Next();
		}
		string word = 
			code[start..position]
			.Replace('.', ',').Replace("_", "").ToLower()
			.Replace('х', 'x').Replace('о', 'o').Replace('б', 'b');
		if (xs == 1)
			return new Token() { Value = Convert.ToInt64(word, 16), Type = TokenType.INTEGER, Location = Loc() };
        if (os == 1)
            return new Token() { Value = Convert.ToInt64(word, 8), Type = TokenType.INTEGER, Location = Loc() };
        if (bs == 1)
            return new Token() { Value = Convert.ToInt64(word.TrimStart('0').TrimStart('b'), 2), Type = TokenType.INTEGER, Location = Loc() };
        if (dots == 0) {
			if (long.TryParse(word, out long res))
				return new Token() { Value = res, Type = TokenType.INTEGER, Location = Loc() };
			throw new Exception($"ЧИСЛО <{word}> БЫЛО СЛИШКОМ ВЕЛИКО ИЛИ МАЛО ДЛЯ ПОДДЕРЖИВАЕМЫХ СЕЙЧАС ЧИСЕЛ");
		}
        if (dots == 1)
		{
			if (double.TryParse(word, out double res))
				return new Token() { Value = res, Type = TokenType.DOUBLE, Location = Loc() };
			throw new Exception($"ЧИСЛО <{word}> БЫЛО СЛИШКОМ ВЕЛИКО ИЛИ МАЛО ДЛЯ ПОДДЕРЖИВАЕМЫХ СЕЙЧАС ЧИСЕЛ");
		}
		throw new Exception("МНОГА ТОЧЕК ДЛЯ ЧИСЛА");
	}

    private Token CharToken()
    {
        Next();
        char ch = Current;
        Next();
        if (Current != '\'')
            throw new Exception($"{Loc()}\nНЕ ЗАКРЫТ СИМВОЛ [']");
        Next();
        return ch > 255
            ? new Token() {
                Value = System.Text.Encoding.UTF8.GetBytes(ch.ToString()),
                Type = TokenType.DBYTE,
                Location = Loc()
            }
            : new Token() {
                Value = new byte[] { System.Text.Encoding.UTF8.GetBytes(ch.ToString())[0] },
                Type = TokenType.BYTE,
                Location = Loc()
            };
    }

    private Token WordToken()
    {
        int start = position;
        while (Usable(Current))
			if (Get(1) == '\n') {
				Next();
				break;
			}
			else
				Next();
        string word = code[start..position].Trim('\n');
        return Worder.Word(new Token() { Value = word, Type = TokenType.WORD, Location = Loc() }, word);
    }

    private Token NextToken()
	{
		if (Current == '\0')
			return new Token() { Value = null, Type = TokenType.EOF, Location = Loc() };

		if (char.IsWhiteSpace(Current))
			while (char.IsWhiteSpace(Current))
				Next();

        if ((Current == 'ф' || Current == 'f') && Get(1) == '"') {
            Next();
            return StringToken(true);
        }

		if (Current == '"')
			return StringToken(false);

        if (Current == '\'')
            return CharToken();

		if (char.IsDigit(Current))
			return NumberToken();

        if (Usable(Current))
            return WordToken();

		switch (Current) {
			case '=':
				Next();
				if (Current == '=') {
					Next();
					if (Current == '=')
						return DoNextAndGiveToken(null, TokenType.ARROW);
					return new Token() { Value = null, Type = TokenType.EQUALITY, Location = Loc() };
				}
				if (Current == '>')
					return DoNextAndGiveToken(null, TokenType.ARROW);
				return new Token() { Value = null, Type = TokenType.DO_EQUAL, Location = Loc() };
			case '/':
				Next();
				if (Current == '/')
					return DoNextAndGiveToken(null, TokenType.DIV);
				if (Current == '=')
					return DoNextAndGiveToken(null, TokenType.DIVEQ);
				return new Token() { Value = null, Type = TokenType.DIVISION, Location = Loc() };
			case '!':
				Next();
                if (Current == '!')
                    return DoNextAndGiveToken(null, TokenType.EXCL2);
                if (Current == '=')
					return DoNextAndGiveToken(null, TokenType.NOTEQUALITY);
				return new Token() { Value = null, Type = TokenType.EXCL1, Location = Loc() };
			case '*':
				Next();
				if (Current == '*')
					return DoNextAndGiveToken(null, TokenType.POWER);
				if (Current == '=')
					return DoNextAndGiveToken(null, TokenType.MULEQ);
				return new Token() { Value = null, Type = TokenType.MULTIPLICATION, Location = Loc() };
			case '+':
				Next();
				if (Current == '+')
					return DoNextAndGiveToken(null, TokenType.PLUSPLUS);
				if (Current == '=')
					return DoNextAndGiveToken(null, TokenType.PLUSEQ);
				return new Token() { Value = null, Type = TokenType.PLUS, Location = Loc() };
			case '-':
				Next();
				if (Current == '-')
					return DoNextAndGiveToken(null, TokenType.MINUSMINUS);
				if (Current == '=')
					return DoNextAndGiveToken(null, TokenType.MINUSEQ);
				return new Token() { Value = null, Type = TokenType.MINUS, Location = Loc() };
			case '<':
				Next();
				if (Current == '=')
					return DoNextAndGiveToken(null, TokenType.LESSEQ);
				return new Token() { Value = null, Type = TokenType.LESS, Location = Loc() };
			case '>':
				Next();
				if (Current == '=')
					return DoNextAndGiveToken(null, TokenType.MOREEQ);
				return new Token() { Value = null, Type = TokenType.MORE, Location = Loc() };
			case '@':
				return DoNextAndGiveToken(null, TokenType.DOG);
			case ';':
				return DoNextAndGiveToken(null, TokenType.SEMICOLON);
			case '(':
				return DoNextAndGiveToken(null, TokenType.LEFTSCOB);
			case ')':
				return DoNextAndGiveToken(null, TokenType.RIGHTSCOB);
			case '[':
				return DoNextAndGiveToken(null, TokenType.LCUBSCOB);
			case ']':
				return DoNextAndGiveToken(null, TokenType.RCUBSCOB);
			case '{':
				return DoNextAndGiveToken(null, TokenType.LTRISCOB);
			case '}':
				return DoNextAndGiveToken(null, TokenType.RTRISCOB);
			case '%':
				return DoNextAndGiveToken(null, TokenType.MOD);
            case 'ё':
                return DoNextAndGiveToken(null, TokenType.YO);
            case '.':
				Next();
				if (Current == '.') {
					Next();
					if (Current == '=')
						return DoNextAndGiveToken(null, TokenType.DOTDOTEQ);
					return new Token() { Value = null, Type = TokenType.DOTDOT, Location = Loc() };
				}
				return new Token() { Value = null, Type = TokenType.DOT, Location = Loc() };
			case ',':
				return DoNextAndGiveToken(null, TokenType.COMMA);
			case 'Ё':
				return DoNextAndGiveToken(null, TokenType.COMMENTO);
			case '\n':
				return DoNextAndGiveToken(null, TokenType.SLASH_N);
			case ':':
				return DoNextAndGiveToken(null, TokenType.COLON);
			case '?':
				return DoNextAndGiveToken(null, TokenType.QUESTION);
			case '|':
				return DoNextAndGiveToken(null, TokenType.STICK);
			case '\0':
				return new Token() { Value = null, Type = TokenType.EOF, Location = Loc() };
			default:
				throw new Exception($"{Loc()}\nНЕ СУЩЕСТВУЮЩИЙ СИМВОЛ В ДАННОМ ЯЗЫКЕ <{Current}> <{(int)Current}>");
		}
	}

	public Token[] Tokenize()
	{
		List<Token> tokens = [];
		while (true) {
			Token token = NextToken();

			if (token.Type == TokenType.COMMENTO) {
				while (Current != 'Ё' && Current != 0)
					Next();
                Next();
                continue;
			}
            if (token.Type != TokenType.WHITESPACE)
                tokens.Add(token);
            if (token.Type == TokenType.EOF)
                return [.. tokens];
        }
	}
}