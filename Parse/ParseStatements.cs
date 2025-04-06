namespace pycckuu;

partial class Parser
{
    private ICompilable Import()
    {
        Consume(TokenType.FROM);
        Token lib = Consume(TokenType.WORD);
        Consume(TokenType.IMPORT);
        Token imp = Consume(TokenType.WORD);

        Token asImp = Match(TokenType.AS) ? Consume(TokenType.WORD) : imp;
        bool varArg = Match(TokenType.VARARG);

        // надо как то сделать типа 
        EvaluatedType type = Match(TokenType.TYPE)
            ? EvaluatedType.INT
            : EvaluatedType.VOID;

        ICompilable compilable = new ImportStatement(lib, imp, asImp, varArg, type);

        Consume(TokenType.SEMICOLON);
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

    private ICompilable If()
    {
        Consume(TokenType.WORD_IF);
        throw new("эээээ");
    }
}
