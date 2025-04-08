namespace pycckuu;

public class LoopStatement(BlockStatement loop): ICompilable
{
    private BlockStatement Loop { get; set; } = loop;

    public Instruction Compile() 
    {
        KeyValuePair<string, string> loopLabels = Compiler.AddLoopLabels();

        Instruction ret = new(EvaluatedType.VOID, Comp.Str([
            $"    ; НАЧАЛО ЦИКЛА",
            $"{loopLabels.Key}:",
            Loop.Compile().Code,
            $"    jmp {Loop.StartEnd.Key}",
            $"{loopLabels.Value}:",
            $"    ; КОНЕЦ  ЦИКЛА"
        ]));

        if (loopLabels.EqualsTo(Compiler.GetLastLoopLabels()))
            Compiler.RemoveLastLoopLabels();
        return ret;
    } 
}
