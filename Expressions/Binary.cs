namespace pycckuu;

public sealed class BinaryExpression(ICompilableExpression left, Token op, ICompilableExpression right) : ICompilableExpression
{
    private ICompilableExpression Left = left;
    private Token Op = op;
    private ICompilableExpression Right = right;

    public object Evaluate()
    {
        object left = Left.Evaluate();
        object right = Right.Evaluate();

        if (left is long && right is long) {
            long lleft = Convert.ToInt64(left);
            long lright = Convert.ToInt64(right);
            switch (Op.Type) {
                case TokenType.PLUS:
                    return lleft + lright;
                case TokenType.MINUS:
                    return lleft - lright;
                default:
                    break;
            }
        }
        throw new Exception("НЕ ВЫЧИСЛИМОЕ ЗНАЧЕНИЕ");
    }

    public Instruction Compile()
    {
        Instruction left = Left.Compile();
        Instruction right = Right.Compile();

        if (left.Type == right.Type) {
            return Op.Type switch {
                TokenType.PLUS => left.Type switch {
                    EvaluatedType.INT => new(EvaluatedType.INT, Comp.Str([
                        left.Code,
                        right.Code,
                        $"; {ToString()}",
                        "    pop r9",
                        "    pop r8",
                        "    add r8, r9 ; ПЛЮС",
                        "    push r8",
                    "",])),
                    _ => throw U.YetCantEx($"{left.Type.View()} {Op.Type.View()} {right.Type.View()}", "BinaryExpression")
                },
                TokenType.MINUS => left.Type switch {
                    EvaluatedType.INT => new(EvaluatedType.INT, Comp.Str([
                        left.Code,
                        right.Code,
                        $"; {ToString()}",
                        "    pop r9",
                        "    pop r8",
                        "    sub r8, r9 ; МИНУС",
                        "    push r8",
                    "",])),
                    _ => throw U.YetCantEx($"{left.Type.View()} {Op.Type.View()} {right.Type.View()}", "BinaryExpression")
                },
                TokenType.MULTIPLICATION => left.Type switch {
                    EvaluatedType.INT => new(EvaluatedType.INT, Comp.Str([
                        left.Code,
                        right.Code,
                        $"; {ToString()}",
                        "    pop r9",
                        "    pop r8",
                        "    imul r8, r9 ; УМНОЖЕНИЕ",
                        "    push r8",
                    "",])),
                    _ => throw U.YetCantEx($"{left.Type.View()} {Op.Type.View()} {right.Type.View()}", "BinaryExpression")
                },
                TokenType.DIVISION => left.Type switch {
                    EvaluatedType.INT => new(EvaluatedType.INT, Comp.Str([
                        left.Code,
                        right.Code,
                        $"; {ToString()}",
                        "    pop r8",
                        "    pop rax",
                        "    xor rdx, rdx",
                        "    cqo ; РАСШИРЯЕТ ЗНАК С RAX В RDX",
                        "    idiv r8 ; ДЕЛЕНИЕ ИСПОЛЬЗУЕТ ЧИСЛО 128БИТ RDX:RAX ЗНАКОВОЕ",
                        "    push rax",
                    "",])),
                    _ => throw U.YetCantEx($"{left.Type.View()} {Op.Type.View()} {right.Type.View()}", "BinaryExpression")
                },
                TokenType.DIV => left.Type switch {
                    EvaluatedType.INT => new(EvaluatedType.INT, Comp.Str([
                        left.Code,
                        right.Code,
                        $"; {ToString()}",
                        "    pop r8",
                        "    pop rax",
                        "    xor rdx, rdx",
                        "    cqo ; РАСШИРЯЕТ ЗНАК С RAX В RDX",
                        "    idiv r8 ; ЦЕЛОЕ ИСПОЛЬЗУЕТ ЧИСЛО 128БИТ RDX:RAX ЗНАКОВОЕ",
                        "    push rax",
                    "",])),
                    _ => throw U.YetCantEx($"{left.Type.View()} {Op.Type.View()} {right.Type.View()}", "BinaryExpression")
                },
                TokenType.MOD => left.Type switch {
                    EvaluatedType.INT => new(EvaluatedType.INT, Comp.Str([
                        left.Code,
                        right.Code,
                        $"; {ToString()}",
                        "    pop r8",
                        "    pop rax",
                        "    xor rdx, rdx",
                        "    cqo ; РАСШИРЯЕТ ЗНАК С RAX В RDX",
                        "    idiv r8 ; ДЕЛЕНИЕ ИСПОЛЬЗУЕТ ЧИСЛО 128БИТ RDX:RAX ЗНАКОВОЕ",
                        "    push rdx",
                    "",])),
                    _ => throw U.YetCantEx($"{left.Type.View()} {Op.Type.View()} {right.Type.View()}", "BinaryExpression")
                },
                _ => throw new Exception("НЕ КОМПИЛИРУЕМОЕ БИНАРНОЕ ДЕЙСТВИЕ")
            };
        }
        else
        {
            throw U.YetCantEx("разные типы", "BinaryExpression");
        }
    }

    public override string ToString() => $"{Left} {Op.Type.View()} {Right}";
}
