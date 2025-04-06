namespace pycckuu;

public sealed class UnaryExpression(Token op, ICompilable value) : ICompilable
{
    private Token Op = op;
    private ICompilable Value = value;

    public object Evaluate()
    {
        throw new Exception("НЕ ВЫЧИСЛИМОЕ ЗНАЧЕНИЕ");
    }

    public Instruction Compile()
    {
        Instruction instruction = Value.Compile();

        return Op.Type switch
        {
            TokenType.PLUS => instruction,
            TokenType.MINUS => instruction.Type switch {
                EvaluatedType.INT => new(EvaluatedType.INT, Comp.Str([
                    instruction.Code,
                    "    pop r8",
                    "    neg r8 ; ПОМЕНЯТЬ ЗНАК",
                    "    push r8",
                    ""])),
                EvaluatedType.XMM => new(EvaluatedType.XMM, Comp.Str([
                    instruction.Code,
                    "	pop r8",
                    "	movq xmm6, r8",
                    "	mulsd xmm6, [MINUS_ONE] ; УМНОЖИТЬ НА -1",
                    "	movq r8, xmm6",
                    "	push r8",
                    ""])),
                EvaluatedType.BOOL => throw U.YetCantEx("BOOL", "UnaryExpression"),
                EvaluatedType.STR => throw U.YetCantEx("STR", "UnaryExpression, реверсить строку будет наверно чебы нет"),
                _ => throw new Exception("НЕ ЧИСЛОВОЙ ТИП ПРИ ПОПЫТКЕ ПОМЕНЯТЬ ЗНАК")
            },
            _ => throw new Exception("НЕ КОМПИЛИРУЕМОЕ БИНАРНОЕ ДЕЙСТВИЕ")
        };
    }

    public override string ToString() => $"{Op.Type.View()}{Value}";
}
