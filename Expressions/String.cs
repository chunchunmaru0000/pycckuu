namespace pycckuu;

public sealed class StringExpression : ICompilableExpression
{
    public string Value { get; set; }

    public StringExpression(Token token)
    {
        Value = token.Value!.ToString()!;

        Compiler.AddString(Value);
    }

    public object Evaluate() => Value;

    public Instruction Compile() => new(EvaluatedType.PTR, Comp.Str([
        $"    mov r8, {Compiler.AddString(Value)} ; СТРОКА '{Value}'",
         "    push r8",
    ""]));

    public override string ToString() => Value;
}
