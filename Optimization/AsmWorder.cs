namespace pycckuu;

class AsmWorder: IWorder
{
    private static Token ChangeType(in Token token, TokenType tokenType)
    {
        token.Type = tokenType;
        return token;
    }

    public Token Word(Token word, string str) => str switch {

        _ => word
    };
}
