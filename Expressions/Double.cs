namespace pycckuu;

public sealed class DoubleExpression(Token token) : ICompilableExpression
{
    public double Value { get; } = Convert.ToDouble(token.Value);

    public object Evaluate() => Value;

    public Instruction Compile() => new(EvaluatedType.XMM,
        Comp.Str([
           $"    mov r8, {Value} ; ВЕЩЕСТВЕННОЕ ЧИСЛО {Value}", //  для вставления в стэк чисел с плавающей точкой
				"    push r8",
            "",
        ]));

    public override string ToString() => token.Type.View();
}
