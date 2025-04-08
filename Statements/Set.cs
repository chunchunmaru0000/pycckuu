namespace pycckuu;

public class SetVarStatement(Token name, ICompilable value, int exclamations) : ICompilable
{
    private Token Name { get; set; } = name;
    private ICompilable Value { get; set; } = value;
    private int Exclamations { get; set; } = exclamations;

    public Instruction Compile()
    {
        string name = Compiler.SetVariable(Name);
        Instruction value = Value.Compile();
        int s = value.Type.Size();
        throw new NotFiniteNumberException();

        return new(EvaluatedType.VOID, Comp.Str([
            value.Code,
            $"    pop r8",
            $"    mov {U.Sizes[s]} [{name}], r8{U.RRegs[s]}",
        ""]));
    }
}

public class SetPtrStatement(ICompilable ptr, ICompilable value) : ICompilable
{
    private ICompilable Ptr { get; set; } = ptr;
    private ICompilable Value { get; set; } = value;

    public Instruction Compile()
    {
        Instruction value = Value.Compile();
        int s = value.Type.Size();

        return new(EvaluatedType.VOID, Comp.Str([
            Ptr.Compile().Code,
            Value.Compile().Code,
            "    pop r9",
            "    pop r8",
           $"    mov {U.Sizes[s]} [r8], r9{U.RRegs[s]}",
        ""]));
    }
}

