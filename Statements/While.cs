namespace pycckuu;

public class WhileStatement(ICompilable cond, BlockStatement loop): ICompilable
{
    private ICompilable Cond { get; set; } = cond;
    private BlockStatement Loop { get; set; } = loop;

    public Instruction Compile()
    {
        KeyValuePair<string, string> loopLabels = Compiler.AddLoopLabels();

        Instruction ret = new(EvaluatedType.VOID, Comp.Str([
            $"    ; НАЧАЛО ЦИКЛА ПОКА",
            $"{loopLabels.Key}:",
            Cond.Compile().Code,
            $"    pop r8",
            $"    cmp r8, 0",
            $"    je {loopLabels.Value}",
            Loop.Compile().Code,
            $"    jmp {loopLabels.Key}",
            $"{loopLabels.Value}:",
            $"    ; КОНЕЦ  ЦИКЛА ПОКА",
        ]));

        if (loopLabels.EqualsTo(Compiler.GetLastLoopLabels()))
            Compiler.RemoveLastLoopLabels();
        return ret;
    }
}
