namespace pycckuu;

class ContinueStatement : ICompilable
{
    public Instruction Compile()
    {
        KeyValuePair<string, string> loopLabels = Compiler.GetLastLoopLabels();
        if (loopLabels.Key == null)
            return new(EvaluatedType.VOID, "; ПУСТОЕ СНАЧАЛА ЦИКЛА");
        return new(EvaluatedType.VOID, $"    jmp {loopLabels.Key}; СНАЧАЛА ЦИКЛ");
    }
}
