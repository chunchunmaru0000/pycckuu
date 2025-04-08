namespace pycckuu;

public class YoExpression(ICompilable pointer): ICompilable
{
    private ICompilable Pointer { get; set; } = pointer;

    public Instruction Compile()
    {
        Instruction ptr = Pointer.Compile();
        if (ptr.Type.Size() != 8)
            throw new Exception(
                $"ПРИ ё ВЫРАЖЕНИИ ОЖИДАЛСЯ УКАЗАТЕЛЬ РАЗМЕРОМ 8 БАЙТ\n" +
                $"РАЗМЕР ВЫРАЖЕНИЯ БЫЛ{ptr.Type.Size()} БАЙТ");
        return new Instruction(EvaluatedType.INT, Comp.Str([ // for now int maybe some time rheer will be size
            ptr.Code,
            "    pop r8",
            "    push qword [r8]" // size also will affect size here
        ]));
    }
}
