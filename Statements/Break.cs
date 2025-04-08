namespace pycckuu;

public class BreakStatement : ICompilable
{
    public Instruction Compile() 
    {
        KeyValuePair<string, string> loopLabels = Compiler.GetLastLoopLabels();
        if (loopLabels.Value == null)
            return new(EvaluatedType.VOID, "; ПУСТОЕ ПРЕРЫВАНИЕ ЦИКЛА");

        Instruction ret = new(EvaluatedType.VOID, $"    jmp {loopLabels.Value}; ПРЕРВАТЬ ЦИКЛ");
        Compiler.RemoveLastLoopLabels();
        return ret;
    }
}
