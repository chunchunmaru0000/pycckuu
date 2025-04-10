namespace pycckuu;

public class PtrExpression(string name) : ICompilable
{
    private string Name { get; set; } = name;

    public Instruction Compile() => new(EvaluatedType.INT, Comp.Str([
        $"    lea r8, [rbp - {Compiler.GetVariable(Name).Offset}]",
        $"    push r8 ; {this}",
    ]));

    public override string ToString() => $"УКАЗАТЕЛЬ НА {Name}";
}
