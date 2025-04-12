namespace pycckuu;

public class WhileStatement(ICompilable cond, BlockStatement loop): ICompilable
{
    private ICompilable Cond { get; set; } = cond;
    private BlockStatement Loop { get; set; } = loop;

    public Instruction Compile()
    {
        KeyValuePair<string, string> loopLabels = Compiler.AddLoopLabels();
        int rspSize = EvaluatedType.INT.Size();
        Variable rsp = new(
            $"StackVarPleaseNotUseIt{Compiler.StackVarCounter++}", 
            Compiler.GetLastVarOffset() + rspSize, 
            EvaluatedType.INT);
        int stackOffset = rsp.Offset % 16;

        List<Variable> varsBeforeLoop = Compiler.CloneVars();
        Compiler.AddVariable(rsp);

        Instruction ret = new(EvaluatedType.VOID, Comp.Str([
            $"    ; НАЧАЛО ЦИКЛА ПОКА",
            $"{loopLabels.Key}:",
            $"    mov qword[rbp - {rsp.Offset}], rsp",
            stackOffset != 0
            ? $"    {(stackOffset == 0 ? "; " : "")}sub rsp, {stackOffset + rspSize}; ПРИ ОБЪЯВЛЕНИИ rsp ВЫЧЕСТЬ rsp ЧТОБЫ СДВИНУТЬ СТЭК ДО ПЕРЕМЕНОЙ И ВЫРОВНЯТЬ ЕГО НА 16"
            : $"    ; СДВИГАТЬ НЕЧЕГО, rsp УЖЕ ВЫРОВНЕН",
            Cond.Compile().Code,
            $"    pop r8",
            $"    cmp r8, 0",
            $"    je {loopLabels.Value}",
            Loop.Compile().Code,
            $"    mov rsp, qword[rbp - {rsp.Offset}]",
            $"    jmp {loopLabels.Key}",
            $"{loopLabels.Value}:",
            $"    ; КОНЕЦ  ЦИКЛА ПОКА",
        ]));

        Compiler.ReplaceVariables(varsBeforeLoop);
        if (loopLabels.EqualsTo(Compiler.GetLastLoopLabels()))
            Compiler.RemoveLastLoopLabels();
        return ret;
    }
}
