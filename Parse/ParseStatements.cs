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
}
