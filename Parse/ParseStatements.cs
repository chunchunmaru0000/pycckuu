namespace pycckuu;

partial class Parser
{
    private ICompilable Import()
    {
        Consume(TokenType.FROM);
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
        ICompilable compilable = new ImportStatement(lib, [.. imp], [.. asImp], [.. varArg], [.. type]);

        return compilable;
    }

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

            Match(TokenType.SEMICOLON);
            return new SetVarStatement(name, value);
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
}
