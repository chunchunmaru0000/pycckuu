using System.Globalization;

namespace pycckuu;

public sealed class DoubleExpression(Token token) : ICompilableExpression
{
    public double Value { get; } = Convert.ToDouble(token.Value);

    public object Evaluate() => Value;

    public Instruction Compile()
    {
        string value = Value.ToString("0.0###############", CultureInfo.InvariantCulture);

        return new(EvaluatedType.XMM,
            Comp.Str([
               $"    mov r8, {value} ; ВЕЩЕСТВЕННОЕ ЧИСЛО {value}", //  для вставления в стэк чисел с плавающей точкой
			    "    push r8",
                "",
            ]));
    } 

    public override string ToString() => token.Type.View();
}
