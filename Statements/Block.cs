namespace pycckuu;

public class BlockStatement(ICompilable[] statements): ICompilable
{
    private ICompilable[] Statements { get; set; } = statements;
    public KeyValuePair<string, string> StartEnd { get; set; } = Compiler.AddBlock();

    public Instruction Compile()
    {
        return new Instruction(EvaluatedType.INT, Comp.Str([
            $"{StartEnd.Key}:",
            Comp.Str([.. Statements.Select(s => s.Compile().Code)]),
            $"{StartEnd.Value}:",
        ]));
    }
}
