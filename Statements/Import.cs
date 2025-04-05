namespace pycckuu;

public sealed class ImportStatement(Token lib, Token imp, Token impAs) : ICompilable
{
    private Token Lib { get; set; } = lib;
    private Token Imp { get; set; } = imp;
    private Token ImpAs { get; set; } = impAs;

    public Instruction Compile()
    {
        Compiler.AddLibImports(Lib.Value!.ToString()!, Imp.Value!.ToString()!, ImpAs.Value!.ToString()!);
        return new Instruction(EvaluatedType.IMPORT, "");
    }

    public override string ToString() => 
        $"{TokenType.FROM.Log()}{Lib.Value}{TokenType.IMPORT.Log()}{Imp.Value}{TokenType.AS.Log()}{ImpAs.Value}";
}
