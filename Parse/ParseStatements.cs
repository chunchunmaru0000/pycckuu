namespace pycckuu;

partial class Parser
{
    private ICompilable Import()
    {
        Consume(TokenType.FROM);
        Token lib = Consume(TokenType.STRING);
        Consume(TokenType.IMPORT);
        Token imp = Consume(TokenType.STRING);

        Token asImp = Match(TokenType.AS) ? Consume(TokenType.STRING) : imp;
        bool varArg = Match(TokenType.VARARG);

        // надо как то сделать типа 
        EvaluatedType type = Match(TokenType.TYPE)
            ? ( Match(TokenType.STRINGA) ? EvaluatedType.INT : // ptr is int
                Match(TokenType.NUMBERA) ? EvaluatedType.INT :
                Match(TokenType.POINTERA) ? EvaluatedType.INT :
                Match(TokenType.FLOATA) ? EvaluatedType.XMM :
                throw U.YetCantEx(Current.Type.Log(), "ICompilable Import()")
            )
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
}
