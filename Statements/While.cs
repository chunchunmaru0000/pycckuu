namespace pycckuu;

public class WhileStatement(ICompilable cond, BlockStatement loop): ICompilable
{
    private ICompilable Cond { get; set; } = cond;
    private BlockStatement Loop { get; set; } = loop;

    public Instruction Compile()
    {
        string beforeCond = Compiler.AddLabel();
        string afterLoop = Compiler.AddLabel();

        return new Instruction(EvaluatedType.VOID, Comp.Str([
            $"    ; НАЧАЛО ЦИКЛА ПОКА",
            $"{beforeCond}:",
            Cond.Compile().Code,
            $"    pop r8",
            $"    cmp r8, 0",
            $"    je {afterLoop}",
            Loop.Compile().Code,
            $"    jmp {beforeCond}",
            $"{afterLoop}:",
            $"    ; КОНЕЦ  ЦИКЛА ПОКА",
        ]));
    }
}
