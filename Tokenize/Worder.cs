﻿namespace pycckuu;

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

		"div" => ChangeType(word, TokenType.DIV),
		"целое" => ChangeType(word, TokenType.DIV),
		"mod" => ChangeType(word, TokenType.MOD),
		"остаток" => ChangeType(word, TokenType.MOD),

        "from" => ChangeType(word, TokenType.FROM),
        "из" => ChangeType(word, TokenType.FROM),
        "outof" => ChangeType(word, TokenType.OUTOF),
        "изо" => ChangeType(word, TokenType.OUTOF),

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
        "pointer" => ChangeType(word, TokenType.PTR),
        "УК" => ChangeType(word, TokenType.PTR),
        "ук" => ChangeType(word, TokenType.PTR),
        "указатель" => ChangeType(word, TokenType.PTR),

        "if" => ChangeType(word, TokenType.WORD_IF),
        "если" => ChangeType(word, TokenType.WORD_IF),
        "elif" => ChangeType(word, TokenType.WORD_ELIF),
        "коли" => ChangeType(word, TokenType.WORD_ELIF),
        "else" => ChangeType(word, TokenType.WORD_ELSE),
            "then" => ChangeType(word, TokenType.WORD_THEN),
            "тогда" => ChangeType(word, TokenType.WORD_THEN),
        "инак" => ChangeType(word, TokenType.WORD_ELSE),
        "иначе" => ChangeType(word, TokenType.WORD_ELSE),

        "true" => ChangeType(word, TokenType.WORD_TRUE),
        "истина" => ChangeType(word, TokenType.WORD_TRUE),
        "аминь" => ChangeType(word, TokenType.WORD_TRUE),
        "ВладимирВладимировичПутин" => ChangeType(word, TokenType.WORD_TRUE),
        "false" => ChangeType(word, TokenType.WORD_FALSE),
        "ложь" => ChangeType(word, TokenType.WORD_FALSE),
        "ВладимирАлександровичЗеленский" => ChangeType(word, TokenType.WORD_FALSE),

        "not" => ChangeType(word, TokenType.NOT),
        "не" => ChangeType(word, TokenType.NOT),

        "more" => ChangeType(word, TokenType.MORE),
        "больше" => ChangeType(word, TokenType.MORE),
        "паче" => ChangeType(word, TokenType.MORE),
        "less" => ChangeType(word, TokenType.LESS),
        "меньше" => ChangeType(word, TokenType.LESS),

        "and" => ChangeType(word, TokenType.AND),
        "и" => ChangeType(word, TokenType.AND),
        "or" => ChangeType(word, TokenType.OR),
        "или" => ChangeType(word, TokenType.OR),
        "xor" => ChangeType(word, TokenType.XOR),
        "хор" => ChangeType(word, TokenType.XOR),

        "loop" => ChangeType(word, TokenType.LOOP),
        "цикл" => ChangeType(word, TokenType.LOOP),
        "вихрь" => ChangeType(word, TokenType.LOOP),
        "реквием" => ChangeType(word, TokenType.LOOP),
        "break" => ChangeType(word, TokenType.BREAK),
        "зиять" => ChangeType(word, TokenType.BREAK),
        "прервать" => ChangeType(word, TokenType.BREAK),
        "continue" => ChangeType(word, TokenType.CONTINUE),
        "воскрест" => ChangeType(word, TokenType.CONTINUE),
        "сначала" => ChangeType(word, TokenType.CONTINUE),

        _ => word
    };
}