namespace pycckuu;

partial class Parser
{
    private ICompilable Import()
    {
        Consume(TokenType.FROM);
        Token lib = Consume(TokenType.STRING);
        Consume(TokenType.IMPORT);
        Token imp = Consume(TokenType.STRING);

        ICompilable compilable =
            Match(TokenType.AS)
            ? new ImportStatement(lib, imp, Consume(TokenType.STRING))
            : new ImportStatement(lib, imp, imp);

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
}
