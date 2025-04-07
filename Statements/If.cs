namespace pycckuu;

public class IfStatement(KeyValuePair<ICompilable, BlockStatement>[] ifs, BlockStatement? elseBlock) : ICompilable
{
    private KeyValuePair<ICompilable, BlockStatement>[] Ifs { get; set; } = ifs;
    private BlockStatement? ElseBlock { get; set; } = elseBlock;

    public Instruction Compile()
    {
        bool eNull = ElseBlock == null;

        string lastLabel = eNull ? Ifs.Last().Value.StartEnd.Value : ElseBlock!.StartEnd.Value;
        string[] beforeConditionLabels =
            [.. Enumerable.Range(0, Ifs.Length + 1)
            .Select(i => Compiler.AddLabel())];

        return new(EvaluatedType.VOID, Comp.Str([
            Comp.Str([.. Enumerable.Range(0, Ifs.Length).Select(i => Comp.Str([
                $"    ; НАЧАЛО {i} УСЛОВИЯ",
                $"{beforeConditionLabels[i]}:",
                Ifs[i].Key.Compile().Code,
                $"    ; КОНЕЦ  {i} УСЛОВИЯ",
                $"    pop r8",
                $"    cmp r8, 0",
                $"    je {beforeConditionLabels[i + 1]}",
                $"    ; НАЧАЛО {i} БЛОКА",
                Ifs[i].Value.Compile().Code,
                $"    ; КОНЕЦ  {i} БЛОКА",
                eNull
                ? $"    jmp {beforeConditionLabels.Last()}"
                : $"    jmp {lastLabel}",
            ]))]),
            $"{beforeConditionLabels.Last()}:",
            eNull ? "" : ElseBlock!.Compile().Code
        ]));
    }
        /*
        => ElseBlock == null
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
         */
}
