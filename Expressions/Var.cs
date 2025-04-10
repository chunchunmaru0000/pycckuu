namespace pycckuu;

public class VarExpression(Token word): ICompilable
{
    private string Name { get; set; } = word.Value!.ToString()!;

    public Instruction Compile()
    {
        Variable var = Compiler.GetVariable(Name);
        return new(var.Type, Comp.Str([
            $"    push qword[rbp - {var.Offset}] ; ПЕРЕМЕННАЯ {Name}", // { U.Sizes[var.Type.Size()] }
        ]));
    }

    public override string ToString() => Name;
}
