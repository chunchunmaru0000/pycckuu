﻿namespace pycckuu;

public sealed class StringExpression : ICompilableExpression
{
    public string Value { get; set; }

    public StringExpression(Token token)
    {
        Value = token.Value!.ToString()!;

        Compiler.AddString(Value);
    }

    public object Evaluate() => Value;

    public Instruction Compile() => new(EvaluatedType.INT, Comp.Str([ // should be PTR but its literally INT
        $"    push qword {Compiler.AddString(Value)} ; СТРОКА '{Value}'",
    ]));

    public override string ToString() => Value;
}
