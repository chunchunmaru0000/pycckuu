namespace pycckuu;

public class SetVarStatement(Token name, ICompilable value): ICompilable
{
    private Token Name { get; set; } = name;
    private ICompilable Value { get; set; } = value;

    private static Dictionary<int, string> Sizes { get; set; } = new() {
        { 1, "byte" },
        { 2, "word" },
        { 4, "dword" },
        { 8, "qword" },
    };

    public Instruction Compile()
    {
        string name = Compiler.SetVariable(Name);
        return new(EvaluatedType.VOID, Comp.Str([
            Value.Compile().Code,
            $"    pop r8",
            $"    mov qword [{name}], r8",
        ""]));
    }
}

public class SetPtrStatement(ICompilable ptr, ICompilable value) : ICompilable
{
    private ICompilable Ptr { get; set; } = ptr;
    private ICompilable Value { get; set; } = value;

    public Instruction Compile()
    {
        return new(EvaluatedType.VOID, Comp.Str([
            Ptr.Compile().Code,
            Value.Compile().Code,
            "    pop r9",
            "    pop r8",
            "    mov [r8], r9",
        ""]));
    }
}

