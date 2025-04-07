namespace pycckuu;

public class IfStatement(KeyValuePair<ICompilable, BlockStatement>[] ifs, BlockStatement? elseBlock) : ICompilable
{
    private KeyValuePair<ICompilable, BlockStatement>[] Ifs { get; set; } = ifs;
    private BlockStatement? ElseBlock { get; set; } = elseBlock;

    private Instruction ConditionBlock(KeyValuePair<ICompilable, BlockStatement> cb) => new(EvaluatedType.VOID, Comp.Str([
        cb.Key.Compile().Code,
        $"    pop r8",
        $"    cmp r8, 0",
        $"    jne {cb.Value.StartEnd.Key}",
        $"    jmp {cb.Value.StartEnd.Value}",
        cb.Value.Compile().Code,
    ]));

    public Instruction Compile() => ElseBlock == null
        ? new(EvaluatedType.VOID, Comp.Str([
        Ifs[0].Key.Compile().Code,
            $"    pop r8",
            $"    cmp r8, 0",
            $"    je {Ifs[0].Value.StartEnd.Value}",
            Ifs[0].Value.Compile().Code,
        ]))
        : new (EvaluatedType.VOID, Comp.Str([
            Ifs[0].Key.Compile().Code,
            $"    pop r8",
            $"    cmp r8, 0",
            $"    je {ElseBlock.StartEnd.Key}",
            Ifs[0].Value.Compile().Code,
            $"    jmp {ElseBlock.StartEnd.Value}",
            ElseBlock.Compile().Code,
        ]));
}
