namespace pycckuu;

public enum TokenType
{
    [Log("КОНЕЦ ФАЙЛА")]
    EOF,
    [Log("СЛОВО")]
    WORD,
    [Log("СТРОКА")]
    STRING,
    [Log("ИСТИННОСТЬ")]
    BOOLEAN,
    [Log("ЦЕЛОЕ ЧИСЛО64")]
    INTEGER,
    [Log("ВЕЩЕСТВЕННОЕ ЧИСЛО64")]
    DOUBLE,
    [Log("КЛАСС")]
    CLASS,
    [Log("ЭТОТ")]
    THIS,
    [Log("ЛЯМБДА")]
    LAMBDA,
    [Log("УКАЗАТЕЛЬ")]
    PTR,

    [Log("ЗОВ")]
    CALL,
    [Log("НЕОПРЕДЕЛЕННО")]
    VARARG,
    [Log("ТИПА")]
    TYPE,
    [Log("ПУСТЬ")]
    LET,
    [Log("БУДЕТ")]
    BE,


    //operators
    [Log("ПЛЮС")]
    PLUS,
    [Log("МИНУС")]
    MINUS,
    [Log("УМНОЖЕНИЕ")]
    MULTIPLICATION,
    [Log("ДЕЛЕНИЕ")]
    DIVISION,
    [Log("СДЕЛАТЬ РАВНЫМ")]
    DO_EQUAL,
    [Log("СТРЕЛКА")]
    ARROW,
    [Log("БЕЗ ОСТАТКА")]
    DIV,
    [Log("ОСТАТОК")]
    MOD,
    [Log("СТЕПЕНЬ")]
    POWER,
    [Log("+=")]
    PLUSEQ,
    [Log("-=")]
    MINUSEQ,
    [Log("*=")]
    MULEQ,
    [Log("/=")]
    DIVEQ,
    [Log("НОВЫЙ")]
    NEW,

    //cmp
    [Log("РАВЕН")]
    EQUALITY,
    [Log("НЕ РАВЕН")]
    NOTEQUALITY,
    [Log(">")]
    MORE,
    [Log(">=")]
    MOREEQ,
    [Log("<")]
    LESS,
    [Log("<=")]
    LESSEQ,
    [Log("НЕ")]
    NOT,
    [Log("И")]
    AND,
    [Log("ИЛИ")]
    OR,

    //other
    [Log("ПЕРЕМЕННАЯ")]
    VARIABLE,
    [Log("ФУНКЦИЯ")]
    FUNCTION,
    [Log(";")]
    SEMICOLON,
    [Log(":")]
    COLON,
    [Log("++")]
    PLUSPLUS,
    [Log("--")]
    MINUSMINUS,
    [Log(",")]
    COMMA,

    [Log(")")]
    RIGHTSCOB,
    [Log("(")]
    LEFTSCOB,
    [Log("]")]
    RCUBSCOB,
    [Log("[")]
    LCUBSCOB,
    [Log("}")]
    RTRISCOB,
    [Log("{")]
    LTRISCOB,

    [Log("ПЕРЕНОС")]
    SLASH_N,
    [Log("ЦИТАТА")]
    COMMENTO,
    [Log("ПУСТОТА")]
    WHITESPACE,
    [Log("СОБАКА")]
    DOG,
    [Log("КАВЫЧКА")]
    QUOTE,
    [Log("ТОЧКА")]
    DOT,
    [Log("ТОЧКАТОЧКА")]
    DOTDOT,
    [Log("..=")]
    DOTDOTEQ,
    [Log("ЗНАК ВОПРОСА")]
    QUESTION,
    [Log("ПАЛКА | ")]
    STICK,

    //words types
    [Log("ЕСЛИ")]
    WORD_IF,
    [Log("ИНАЧЕ")]
    WORD_ELSE,
    [Log("ИНАЧЕЛИ")]
    WORD_ELIF,
    [Log("ПОКА")]
    WORD_WHILE,
    [Log("НАЧЕРТАТЬ")]
    WORD_PRINT,
    [Log("ДЛЯ")]
    WORD_FOR,
    [Log("ЦИКЛ")]
    LOOP,
    [Log("ИСТИНА")]
    WORD_TRUE,
    [Log("ЛОЖЬ")]
    WORD_FALSE,
    [Log("ПРОДОЛЖИТЬ")]
    CONTINUE,
    [Log("ВЫЙТИ")]
    BREAK,
    [Log("ВЕРНУТЬ")]
    RETURN,
    [Log("ВЫПОЛНИТЬ ПРОЦЕДУРУ")]
    PROCEDURE,
    [Log("СЕЙЧАС")]
    NOW,
    [Log("ЧИСТКА")]
    CLEAR,
    [Log("СОН")]
    SLEEP,
    [Log("РУСИТЬ")]
    VOVASCRIPT,
    [Log("ТОЧНО")]
    EXACTLY,
    [Log("ЗАПОЛНИТЬ")]
    FILL,
    [Log("КОТОРЫЙ/АЯ/ОЕ")]
    WHICH,
    [Log("ЧЕРПАТЬ")]
    IMPORT,
    [Log("НАСЛЕДУЕТ")]
    SON,
    [Log("БРОСИТЬ")]
    THROW,
    [Log("ПОПРОБОВАТЬ")]
    TRY,
    [Log("ПОЙМАЙТЬ")]
    CATCH,

    //SQL
    [Log("СОЗДАТЬ")]
    CREATE,
    [Log("БД")]
    DATABASE,
    [Log("ТАБЛИЦА")]
    TABLE,
    [Log("ДОБАВИТЬ")]
    INSERT,
    [Log("В")]
    IN,
    [Log("ЗНАЧЕНИЯ")]
    VALUES,
    [Log("КОЛОНКИ")]
    COLONS,
    [Log("ГДЕ")]
    WHERE,
    [Log("ВЫБРАТЬ")]
    SELECT,
    [Log("ИЗ")]
    FROM,

    [Log("ОТ")]
    AT,
    [Log("ДО")]
    TILL,
    [Log("ШАГ")]
    STEP,

    [Log("КАК")]
    AS,
    [Log("ЦЕЛОЕ ЧИСЛО")]
    INT,
    [Log("ВЕЩЕСТВЕННОЕ ЧИСЛО")]
    DOUBLEPRECISION,

    [Log("ЧИСЛО")]
    NUMBER,
    [Log("ЧИСЛО С ТОЧКОЙ")]
    FNUMBER,
    [Log("СТРОЧКА")]
    STROKE,
    [Log("ПРАВДИВОСТЬ")]
    BUL,

    [Log("ВСЁ")]
    ALL,
}
