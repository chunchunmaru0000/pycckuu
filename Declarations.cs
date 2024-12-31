namespace Pycckuu
{
	public class StringValueAttribute(string value) : Attribute
	{
		public string Value { get; } = value;
	}

	public static class EnumExtensions
	{
		public static string GetStringValue(this Enum value)
		{
			var type = value.GetType();
			var fieldInfo = type.GetField(value.ToString());
			var attribs = fieldInfo!.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
			return attribs!.Length > 0 ? attribs[0].Value : null!;
		}
	}

	public enum TokenType
	{
		[StringValue("КОНЕЦ ФАЙЛА")]
		EOF,
		[StringValue("СЛОВО")]
		WORD,
		[StringValue("СТРОКА")]
		STRING,
		[StringValue("ИСТИННОСТЬ")]
		BOOLEAN,
		[StringValue("ЦЕЛОЕ ЧИСЛО64")]
		INTEGER,
		[StringValue("НЕ ЦЕЛОЕ ЧИСЛО64")]
		DOUBLE,
		[StringValue("КЛАСС")]
		CLASS,
		[StringValue("ЭТОТ")]
		THIS,
		[StringValue("ЛЯМБДА")]
		LAMBDA,

		//operators
		[StringValue("ПЛЮС")]
		PLUS,
		[StringValue("МИНУС")]
		MINUS,
		[StringValue("УМНОЖЕНИЕ")]
		MULTIPLICATION,
		[StringValue("ДЕЛЕНИЕ")]
		DIVISION,
		[StringValue("СДЕЛАТЬ РАВНЫМ")]
		DO_EQUAL,
		[StringValue("СТРЕЛКА")]
		ARROW,
		[StringValue("БЕЗ ОСТАТКА")]
		DIV,
		[StringValue("ОСТАТОК")]
		MOD,
		[StringValue("СТЕПЕНЬ")]
		POWER,
		[StringValue("+=")]
		PLUSEQ,
		[StringValue("-=")]
		MINUSEQ,
		[StringValue("*=")]
		MULEQ,
		[StringValue("/=")]
		DIVEQ,
		[StringValue("НОВЫЙ")]
		NEW,

		//cmp
		[StringValue("РАВЕН")]
		EQUALITY,
		[StringValue("НЕ РАВЕН")]
		NOTEQUALITY,
		[StringValue(">")]
		MORE,
		[StringValue(">=")]
		MOREEQ,
		[StringValue("<")]
		LESS,
		[StringValue("<=")]
		LESSEQ,
		[StringValue("НЕ")]
		NOT,
		[StringValue("И")]
		AND,
		[StringValue("ИЛИ")]
		OR,

		//other
		[StringValue("ПЕРЕМЕННАЯ")]
		VARIABLE,
		[StringValue("ФУНКЦИЯ")]
		FUNCTION,
		[StringValue(";")]
		SEMICOLON,
		[StringValue(":")]
		COLON,
		[StringValue("++")]
		PLUSPLUS,
		[StringValue("--")]
		MINUSMINUS,
		[StringValue(",")]
		COMMA,

		[StringValue(")")]
		RIGHTSCOB,
		[StringValue("(")]
		LEFTSCOB,
		[StringValue("]")]
		RCUBSCOB,
		[StringValue("[")]
		LCUBSCOB,
		[StringValue("}")]
		RTRISCOB,
		[StringValue("{")]
		LTRISCOB,

		[StringValue("ПЕРЕНОС")]
		SLASH_N,
		[StringValue("ЦИТАТА")]
		COMMENTO,
		[StringValue("ПУСТОТА")]
		WHITESPACE,
		[StringValue("СОБАКА")]
		DOG,
		[StringValue("КАВЫЧКА")]
		QUOTE,
		[StringValue("ТОЧКА")]
		DOT,
		[StringValue("ТОЧКАТОЧКА")]
		DOTDOT,
		[StringValue("..=")]
		DOTDOTEQ,
		[StringValue("ЗНАК ВОПРОСА")]
		QUESTION,
		[StringValue("ПАЛКА | ")]
		STICK,

		//words types
		[StringValue("ЕСЛИ")]
		WORD_IF,
		[StringValue("ИНАЧЕ")]
		WORD_ELSE,
		[StringValue("ИНАЧЕЛИ")]
		WORD_ELIF,
		[StringValue("ПОКА")]
		WORD_WHILE,
		[StringValue("НАЧЕРТАТЬ")]
		WORD_PRINT,
		[StringValue("ДЛЯ")]
		WORD_FOR,
		[StringValue("ЦИКЛ")]
		LOOP,
		[StringValue("ИСТИНА")]
		WORD_TRUE,
		[StringValue("ЛОЖЬ")]
		WORD_FALSE,
		[StringValue("ПРОДОЛЖИТЬ")]
		CONTINUE,
		[StringValue("ВЫЙТИ")]
		BREAK,
		[StringValue("ВЕРНУТЬ")]
		RETURN,
		[StringValue("ВЫПОЛНИТЬ ПРОЦЕДУРУ")]
		PROCEDURE,
		[StringValue("СЕЙЧАС")]
		NOW,
		[StringValue("ЧИСТКА")]
		CLEAR,
		[StringValue("СОН")]
		SLEEP,
		[StringValue("РУСИТЬ")]
		VOVASCRIPT,
		[StringValue("ТОЧНО")]
		EXACTLY,
		[StringValue("ЗАПОЛНИТЬ")]
		FILL,
		[StringValue("КОТОРЫЙ/АЯ/ОЕ")]
		WHICH,
		[StringValue("ВКЛЮЧИТЬ")]
		IMPORT,
		[StringValue("НАСЛЕДУЕТ")]
		SON,
		[StringValue("БРОСИТЬ")]
		THROW,
		[StringValue("ПОПРОБОВАТЬ")]
		TRY,
		[StringValue("ПОЙМАЙТЬ")]
		CATCH,

		//SQL
		[StringValue("СОЗДАТЬ")]
		CREATE,
		[StringValue("БД")]
		DATABASE,
		[StringValue("ТАБЛИЦА")]
		TABLE,
		[StringValue("ДОБАВИТЬ")]
		INSERT,
		[StringValue("В")]
		IN,
		[StringValue("ЗНАЧЕНИЯ")]
		VALUES,
		[StringValue("КОЛОНКИ")]
		COLONS,
		[StringValue("ГДЕ")]
		WHERE,
		[StringValue("ВЫБРАТЬ")]
		SELECT,
		[StringValue("ИЗ")]
		FROM,

		[StringValue("ОТ")]
		AT,
		[StringValue("ДО")]
		TILL,
		[StringValue("ШАГ")]
		STEP,

		[StringValue("КАК")]
		AS,
		[StringValue("ЦЕЛОЕ ЧИСЛО")]
		INT,
		[StringValue("ВЕЩЕСТВЕННОЕ ЧИСЛО")]
		DOUBLEPRECISION,

		[StringValue("ЧИСЛО")]
		NUMBER,
		[StringValue("ЧИСЛО С ТОЧКОЙ")]
		FNUMBER,
		[StringValue("СТРОЧКА")]
		STROKE,
		[StringValue("ПРАВДИВОСТЬ")]
		BUL,

		[StringValue("ВСЁ")]
		ALL,
	}

	public class Location(int line, int letter)
	{
		public int Line { get; set; } = line;
		public int Letter { get; set; } = letter;

		public override string ToString() => $"<СТРОКА {Line}, СИМВОЛ {Letter}>";
	}

	public class Token
	{
		public string? View { get; set; }
		public object? Value { get; set; }
		public TokenType Type { get; set; }

		public required Location Location { get; set; }

		public Token Clone() => new() { Value = Value, View = View, Type = Type, Location = Location };

		public override string ToString() => $"<{View}> <{Convert.ToString(Value)}> <{Type.GetStringValue()}> <{Location}>";
	}

	public interface IExecutable
	{
		public void Execute();
	}

	public interface IExpression
	{
		public object Evaluate();
	}
		
	public interface ICompilable
	{
		public string Compile();
	}

	public interface ICompilableExpression: IExpression, ICompilable;

	public static class Comp
	{
		public static string Str(string[] sArr) => string.Join("\r\n", sArr);
	}
}