namespace pycckuu;

partial class Parser
{
    private ICompilable Import(bool toTranscribeWords)
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
                _ => c
            }
        )
    );

    private ICompilable Call()
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
            Consume(TokenType.BE);
            ICompilable value = CompilableExpression();

            Match(TokenType.SEMICOLON);
            return new SetPtrStatement(ptr, value);
        }
    }

    private BlockStatement Block()
    {
        Consume(TokenType.LEFTSCOB);
        List<ICompilable> statements = [];

        while (!Match(TokenType.RIGHTSCOB))
            statements.Add(CompilableStatement());

        return new BlockStatement([.. statements]);
    }

    private ICompilable If()
    {
        Consume(TokenType.WORD_IF);
        List<KeyValuePair<ICompilable, BlockStatement>> ifs = [];
        ifs.Add(new(CompilableExpression(), Block()));

        while (Match(TokenType.WORD_ELIF))
            ifs.Add(new(CompilableExpression(), Block()));

        BlockStatement? elseBlock = null;
        if (Match(TokenType.WORD_ELSE))
            elseBlock = Block();

        return new IfStatement([.. ifs], elseBlock);
    }

    private ICompilable Loop()
    {
        Consume(TokenType.LOOP);
        BlockStatement loop = Block();
        return new LoopStatement(loop);
    }

    private ICompilable While()
    {
        Consume(TokenType.WORD_WHILE);
        ICompilable cond = CompilableExpression();
        BlockStatement loop = Block();

        return new WhileStatement(cond, loop);
    }
}
