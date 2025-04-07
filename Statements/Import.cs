namespace pycckuu;

public sealed class ImportStatement(Token lib, Token[] imp, Token[] impAs, bool[] varArg, EvaluatedType[] type) : ICompilable
{
    private Token Lib { get; set; } = lib;
    private Token[] Imp { get; set; } = imp;
    private Token[] ImpAs { get; set; } = impAs;
    private bool[] VarArg { get; set; } = varArg;
    private EvaluatedType[] Type { get; set; } = type;

    private static string Sure(Token t) => t.Value!.ToString()!;

    public Instruction Compile()
    {
        for (int i = 0; i < Imp.Length; i++)
            Compiler.AddLibImports(new(
                Sure(Lib), Sure(Imp[i]), Sure(ImpAs[i]), VarArg[i], Type[i]));
        return new Instruction(EvaluatedType.IMPORT, "");
    }

    public override string ToString() =>
        $"{TokenType.FROM.Log()}{Lib.Value}{TokenType.IMPORT.Log()}" + 
        string.Join(", ", Enumerable.Range(0, Imp.Length).Select(i => $"{Imp[i].Value}{TokenType.AS.Log()}{ImpAs[i].Value}")) +
        ";";
}
