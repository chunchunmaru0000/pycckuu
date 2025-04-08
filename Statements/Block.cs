namespace pycckuu;

public class BlockStatement(ICompilable[] statements, bool needLabels): ICompilable
{
    private ICompilable[] Statements { get; set; } = statements;
    public KeyValuePair<string, string> StartEnd { get; set; } =
        needLabels
        ? Compiler.AddBlock()
        : default;

    public Instruction Compile() => 
        !needLabels
        ? new(EvaluatedType.INT, Comp.Str([.. Statements.Select(s => s.Compile().Code)]))
        : new(EvaluatedType.INT, Comp.Str([
            $"{StartEnd.Key}:",
            Comp.Str([.. Statements.Select(s => s.Compile().Code)]),
            $"{StartEnd.Value}:",
        ]));
}
