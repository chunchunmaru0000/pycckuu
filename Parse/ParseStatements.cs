﻿namespace pycckuu;

partial class Parser
{
    private ImportStatement Import(bool toTranscribeWords)
    {
        Consume(TokenType.FROM, TokenType.OUTOF);
        Token lib = Consume(TokenType.WORD);
        Consume(TokenType.IMPORT);

        List<Token> imp = [];
        List<Token> asImp = [];
        List<bool> varArg = [];
        List<EvaluatedType> type = [];
        while (!Match(TokenType.SEMICOLON)) {
            imp.Add(Consume(TokenType.WORD));
            asImp.Add(Match(TokenType.AS) ? Consume(TokenType.WORD) : imp.Last());
            varArg.Add(Match(TokenType.VARARG));
            type.Add(Match(TokenType.TYPE)
                ? EvaluatedType.INT
                : EvaluatedType.VOID
            );

            Match(TokenType.COMMA);
        }

        ImportStatement compilable = new(lib, [.. imp], [.. asImp], [.. varArg], [.. type]);
        if (toTranscribeWords) 
            TranscribedImportStatement(compilable);

        return compilable;
    }

    private static void TranscribedImportStatement(ImportStatement statement)
    {
        statement.Lib.Value = TranscribeWord(statement.Lib.Value!.ToString()!);
        for (int i = 0; i < statement.Imp.Length; i++)
            statement.Imp[i].Value = TranscribeWord(statement.Imp[i].Value!.ToString()!);
    }

    private static string TranscribeWord(string str) => string.Join("", 
        str
        .ToCharArray()
        .Select(c => 
            c switch {
                'а' => 'a','б' => 'b','ц' => 'c','д' => 'd',
                'е' => 'e','ф' => 'f','г' => 'g','ш' => 'h',
                'и' => 'i','ж' => 'j','к' => 'k','л' => 'l',
                'м' => 'm','н' => 'n','о' => 'o','п' => 'p',
                'ъ' => 'q','р' => 'r','с' => 's','т' => 't',
                'у' => 'u','в' => 'v','ь' => 'w','х' => 'x',
                'й' => 'y','з' => 'z',
                'А' => 'A','Б' => 'B','Ц' => 'C','Д' => 'D',
                'Е' => 'E','Ф' => 'F','Г' => 'G','Ш' => 'H',
                'И' => 'I','Ж' => 'J','К' => 'K','Л' => 'L',
                'М' => 'M','Н' => 'N','О' => 'O','П' => 'P',
                'Ъ' => 'Q','Р' => 'R','С' => 'S','Т' => 'T',
                'У' => 'U','В' => 'V','Ь' => 'W','Х' => 'X',
                'Й' => 'Y','З' => 'Z',
                _ => c
            }
        )
    );

    private CallStatement Call()
    {
        Consume(TokenType.CALL);
        Token func = Consume(TokenType.WORD);
        List<ICompilable> parameters = [];
        while (!Match(TokenType.SEMICOLON))
            parameters.Add(CompilableExpression());
        return new CallStatement(func, [.. parameters]);
    }

    private ICompilable Let()
    {
        Consume(TokenType.LET);
        if (Get(1).Type == TokenType.BE) { // this is just like: let a be 10 
            Token name = Consume(TokenType.WORD);
            Consume(TokenType.BE);
            ICompilable value = CompilableExpression();

            int excl = 0;
            if (Match(TokenType.EXCL1))
                excl = 1;
            else if (Match(TokenType.EXCL2))
                excl = 2;

            return new SetVarStatement(name, value, excl);
        } else { // this is propably array like: let a + 3 be 32
            ICompilable ptr = CompilableExpression();
            Consume(TokenType.WILL);
            ICompilable value = CompilableExpression();

            return new SetPtrStatement(ptr, value);
        }
    }

    private BlockStatement Block(bool needLabels)
    {
        Consume(TokenType.LEFTSCOB);
        List<ICompilable> statements = [];

        while (!Match(TokenType.RIGHTSCOB))
            statements.Add(CompilableStatement());

        return new BlockStatement([.. statements], needLabels);
    }

    private IfStatement If()
    {
        Consume(TokenType.WORD_IF);
        List<KeyValuePair<ICompilable, BlockStatement>> ifs = [];
        ifs.Add(new(CompilableExpression(), Block(true)));

        while (Match(TokenType.WORD_ELIF))
            ifs.Add(new(CompilableExpression(), Block(true)));

        BlockStatement? elseBlock = null;
        if (Match(TokenType.WORD_ELSE))
            elseBlock = Block(true);

        return new IfStatement([.. ifs], elseBlock);
    }

    private LoopStatement Loop()
    {
        Consume(TokenType.LOOP);
        BlockStatement loop = Block(false);
        return new LoopStatement(loop);
    }

    private WhileStatement While()
    {
        Consume(TokenType.WORD_WHILE);
        ICompilable cond = CompilableExpression();
        BlockStatement loop = Block(false);

        return new WhileStatement(cond, loop);
    }

    private BreakStatement Break()
    {
        Consume(TokenType.BREAK);
        return new BreakStatement();
    }

    private ContinueStatement Continue()
    {
        Consume(TokenType.CONTINUE);
        return new ContinueStatement();
    }

    private DeclareFunctionStatement DeclareFunction()
    {
        Consume(TokenType.GLORY);
        Token God = Current;
        if (Match(TokenType.god)) {
            string god = Convert.ToString(God.Value)!;
            throw new Exception($"НЕУВАЖИТЕЛЬНОЕ ОБРАЩЕНИЕ К [{god[0..1].ToUpper()}{god[1..]}]");
        } else if (!Match(TokenType.GOD))
            throw new Exception($"ВЫ ЗАБЫЛИ ВОСХВАЛИТЬ");

        string name = Consume(TokenType.WORD).Value!.ToString()!;
        bool typed = Match(TokenType.TYPE);
        List<Token> args = [];
        while (Current.Type != TokenType.LEFTSCOB)
            args.Add(Consume(TokenType.WORD));

        BlockStatement body = Block(false);
        return new(name, typed, args, body);
    }

    private ReturnStatement Return()
    {
        Consume(TokenType.RETURN);
        return new(CompilableExpression());
    }
}
