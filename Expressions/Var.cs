namespace pycckuu;

public class VarExpression(Token word): ICompilableExpression
{
    private string Name { get; set; } = word.Value!.ToString()!;

    public Instruction Compile() => new(EvaluatedType.INT, Comp.Str([
        $"    push qword [{Name}] ; ПЕРЕМЕННАЯ {Name}",
    ]));

    public object Evaluate()
    {
        throw new NotImplementedException();
    }

    public override string ToString() => Name;
}
