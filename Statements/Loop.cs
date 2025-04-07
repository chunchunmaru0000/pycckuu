namespace pycckuu;

public class LoopStatement(BlockStatement loop): ICompilable
{
    private BlockStatement Loop { get; set; } = loop;

    public Instruction Compile() => new(EvaluatedType.VOID, Comp.Str([
        $"    ; НАЧАЛО ЦИКЛА",
        Loop.Compile().Code,
        $"    jmp {Loop.StartEnd.Key}",
        $"    ; КОНЕЦ  ЦИКЛА"
    ]));
}
