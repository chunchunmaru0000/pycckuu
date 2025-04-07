namespace pycckuu;

public class TernaryExpression(ICompilable condition, ICompilable tru, ICompilable fal): ICompilable
{
    private ICompilable Condition { get; set; } = condition;
    private ICompilable Tru { get; set; } = tru;
    private ICompilable Fal { get; set; } = fal;

    public Instruction Compile()
    {
        string notTru = Compiler.AddConditionLabel();
        string tru = Compiler.AddConditionLabel();

        return new(EvaluatedType.INT, Comp.Str([
            $"    ; ТЕРНАРНЫЙ НАЧАЛО",
            Condition.Compile().Code,
            $"    pop r8",
            $"    cmp r8, 0",
            $"    je {notTru}",
            Tru.Compile().Code,
            $"    jmp {tru}",
            $"{notTru}:",
            Fal.Compile().Code,
            $"{tru}:",
            $"    ; ТЕРНАРНЫЙ КОНЕЦ",
        ]));
    }
}
