namespace pycckuu;

public class PtrExpression(string name) : ICompilable
{
    private string Name { get; set; } = name;

    public Instruction Compile() => new(EvaluatedType.INT, Comp.Str([
        $"    push {Name} ; {this}",
    ]));

    public override string ToString() => $"УКАЗАТЕЛЬ НА {Name}";
}
