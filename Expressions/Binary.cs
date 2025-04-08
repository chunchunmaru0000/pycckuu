namespace pycckuu;

public sealed class BinaryExpression(ICompilable left, Token op, ICompilable right) : ICompilable
{
    private ICompilable Left = left;
    private Token Op = op;
    private ICompilable Right = right;

    public Instruction Compile()
    {
        Instruction left = Left.Compile();
        Instruction right = Right.Compile();
        NotImplementedException niet = U.YetCantEx($"разные типы {ToString()}", "BinaryExpression");
        NotImplementedException nie = U.YetCantEx($"{left.Type.View()} {Op.Type.View()} {right.Type.View()}", "BinaryExpression");

        // 14.23 % 3.0 - 10.0 * 100.0
        if (left.Type == right.Type) {
            return Op.Type switch {
                TokenType.PLUS => left.Type switch {
                    EvaluatedType.INT => new(EvaluatedType.INT, Comp.Str([
                        left.Code,
                        right.Code,
                       $"    pop r9; {ToString()}",
                        "    pop r8",
                        "    add r8, r9 ; ПЛЮС",
                        "    push r8",
                    ])),
                    EvaluatedType.XMM => new (EvaluatedType.XMM, Comp.Str([
                        left.Code,
                        right.Code,
                       $"    pop r8 ; {ToString()}",
                        "    movq xmm7, r8",
                        "    pop r8",
                        "    movq xmm6, r8",
                        "    addsd xmm6, xmm7 ; ПЛЮС",
                        "    movq r8, xmm6",
                        "    push r8",
                    ])),
                    _ => throw nie
                },
                TokenType.MINUS => left.Type switch {
                    EvaluatedType.INT => new(EvaluatedType.INT, Comp.Str([
                        left.Code,
                        right.Code,
                       $"    pop r9; {ToString()}",
                        "    pop r8",
                        "    sub r8, r9 ; МИНУС",
                        "    push r8",
                    ])),
                    EvaluatedType.XMM => new(EvaluatedType.XMM, Comp.Str([
                        left.Code,
                        right.Code,
                       $"    pop r8 ; {ToString()}",
                        "    movq xmm7, r8",
                        "    pop r8",
                        "    movq xmm6, r8",
                        "    subsd xmm6, xmm7 ; МИНУС",
                        "    movq r8, xmm6",
                        "    push r8",
                    ])),
                    _ => throw nie
                },
                TokenType.MULTIPLICATION => left.Type switch {
                    EvaluatedType.INT => new(EvaluatedType.INT, Comp.Str([
                        left.Code,
                        right.Code,
                       $"    pop r9 ; {ToString()}",
                        "    pop r8",
                        "    imul r8, r9 ; УМНОЖЕНИЕ",
                        "    push r8",
                    ])),
                    EvaluatedType.XMM => new(EvaluatedType.XMM, Comp.Str([
                        left.Code,
                        right.Code,
                       $"    pop r8 ; {ToString()}",
                        "    movq xmm7, r8",
                        "    pop r8",
                        "    movq xmm6, r8",
                        "    mulsd xmm6, xmm7 ; УМНОЖЕНИЕ",
                        "    movq r8, xmm6",
                        "    push r8",
                    ])),
                    _ => throw nie
                },
                TokenType.DIVISION => left.Type switch {
                    EvaluatedType.INT => new(EvaluatedType.INT, Comp.Str([
                        left.Code,
                        right.Code,
                       $"    pop r8 ; {ToString()}",
                        "    pop rax",
                        "    xor rdx, rdx",
                        "    cqo ; РАСШИРЯЕТ ЗНАК С RAX В RDX",
                        "    idiv r8 ; ДЕЛЕНИЕ ИСПОЛЬЗУЕТ ЧИСЛО 128БИТ RDX:RAX ЗНАКОВОЕ",
                        "    push rax",
                    ])),
                    EvaluatedType.XMM => new(EvaluatedType.XMM, Comp.Str([
                        left.Code,
                        right.Code,
                       $"    pop r8 ; {ToString()}",
                        "    movq xmm7, r8",
                        "    pop r8",
                        "    movq xmm6, r8",
                        "    divsd xmm6, xmm7 ; ДЕЛЕНИЕ",
                        "    movq r8, xmm6",
                        "    push r8",
                    ])),
                    _ => throw nie
                },
                TokenType.DIV => left.Type switch {
                    EvaluatedType.INT => new(EvaluatedType.INT, Comp.Str([
                        left.Code,
                        right.Code,
                       $"    pop r8 ; {ToString()}",
                        "    pop rax",
                        "    xor rdx, rdx",
                        "    cqo ; РАСШИРЯЕТ ЗНАК С RAX В RDX",
                        "    idiv r8 ; ЦЕЛОЕ ИСПОЛЬЗУЕТ ЧИСЛО 128БИТ RDX:RAX ЗНАКОВОЕ",
                        "    push rax",
                    ])),
                    EvaluatedType.XMM => new(EvaluatedType.XMM, Comp.Str([
                        left.Code,
                        right.Code,
                       $"    pop r8 ; {ToString()}",
                        "    movq xmm7, r8",
                        "    pop r8",
                        "    movq xmm6, r8",
                        "    divsd xmm6, xmm7 ; ДЕЛЕНИЕ",
                        "    roundsd xmm6, xmm6, 1 ; ОКРУГЛЕНИЕ РЕЗУЛЬТАТА ДЕЛЕНИЯ",
                        "    movq r8, xmm6",
                        "    push r8",
                    ])),
                    _ => throw nie
                },
                TokenType.MOD => left.Type switch {
                    EvaluatedType.INT => new(EvaluatedType.INT, Comp.Str([
                        left.Code,
                        right.Code,
                       $"    pop r8 ; {ToString()}",
                        "    pop rax",
                        "    xor rdx, rdx",
                        "    cqo ; РАСШИРЯЕТ ЗНАК С RAX В RDX",
                        "    idiv r8 ; ДЕЛЕНИЕ ИСПОЛЬЗУЕТ ЧИСЛО 128БИТ RDX:RAX ЗНАКОВОЕ",
                        "    push rdx",
                    ])),
                    EvaluatedType.XMM => new(EvaluatedType.XMM, Comp.Str([
                        left.Code,
                        right.Code,
                       $"    pop r9 ; {ToString()}",
                        "    movq xmm7, r9",
                        "    pop r8",
                        "    movq xmm6, r8",
                        "    divsd xmm6, xmm7 ; ДЕЛЕНИЕ",
                        "    roundsd xmm6, xmm6, 1 ; ОКРУГЛЕНИЕ РЕЗУЛЬТАТА ДЕЛЕНИЯ",
                        "    mulsd xmm7, xmm6 ; (a // b) * b",
                        "    movq xmm6, r8 ; a",
                        "    subsd xmm6, xmm7 ; a - (a // b) * b === mod",
                        "    movq r8, xmm6",
                        "    push r8",
                    ])),
                    _ => throw nie
                },
                _ => throw nie
            };
        } else {
            return Op.Type switch {
                TokenType.PLUS => left.Type switch {
                    EvaluatedType.INT => right.Type switch {
                        EvaluatedType.XMM => new(EvaluatedType.XMM, Comp.Str([
                            left.Code,
                            right.Code,
                           $"    pop r8 ; {ToString()}",
                            "    movq xmm7, r8",
                            "    pop r8",
                            "    cvtsi2sd xmm6, r8",
                            "    addsd xmm6, xmm7 ; ПЛЮС",
                            "    movq r8, xmm6",
                            "    push r8",
                        ])),
                        _ => throw niet
                    },
                    EvaluatedType.XMM => right.Type switch {
                        EvaluatedType.INT => new(EvaluatedType.XMM, Comp.Str([
                            left.Code,
                            right.Code,
                           $"    pop r8 ; {ToString()}",
                            "    cvtsi2sd xmm7, r8",
                            "    pop r8",
                            "    movq xmm6, r8",
                            "    addsd xmm6, xmm7 ; ПЛЮС",
                            "    movq r8, xmm6",
                            "    push r8",
                        ])),
                        _ => throw niet
                    },
                    _ => throw nie
                },
                TokenType.MINUS => left.Type switch {
                    EvaluatedType.INT => right.Type switch {
                        EvaluatedType.XMM => new(EvaluatedType.XMM, Comp.Str([
                            left.Code,
                            right.Code,
                           $"    pop r8 ; {ToString()}",
                            "    movq xmm7, r8",
                            "    pop r8",
                            "    cvtsi2sd xmm6, r8",
                            "    subsd xmm6, xmm7 ; МИНУС",
                            "    movq r8, xmm6",
                            "    push r8",
                        ])),
                        _ => throw niet
                    },
                    EvaluatedType.XMM => right.Type switch {
                        EvaluatedType.INT => new(EvaluatedType.XMM, Comp.Str([
                            left.Code,
                            right.Code,
                           $"    pop r8 ; {ToString()}",
                            "    cvtsi2sd xmm7, r8",
                            "    pop r8",
                            "    movq xmm6, r8",
                            "    subsd xmm6, xmm7 ; МИНУС",
                            "    movq r8, xmm6",
                            "    push r8",
                        ])),
                        _ => throw niet
                    },
                    _ => throw nie
                },
                TokenType.MULTIPLICATION => left.Type switch {
                    EvaluatedType.INT => right.Type switch {
                        EvaluatedType.XMM => new(EvaluatedType.XMM, Comp.Str([
                            left.Code,
                            right.Code,
                           $"    pop r8 ; {ToString()}",
                            "    movq xmm7, r8",
                            "    pop r8",
                            "    cvtsi2sd xmm6, r8",
                            "    mulsd xmm6, xmm7 ; УМНОЖЕНИЕ",
                            "    movq r8, xmm6",
                            "    push r8",
                        ])),
                        _ => throw niet
                    },
                    EvaluatedType.XMM => right.Type switch {
                        EvaluatedType.INT => new(EvaluatedType.XMM, Comp.Str([
                            left.Code,
                            right.Code,
                           $"    pop r8 ; {ToString()}",
                            "    cvtsi2sd xmm7, r8",
                            "    pop r8",
                            "    movq xmm6, r8",
                            "    mulsd xmm6, xmm7 ; УМНОЖЕНИЕ",
                            "    movq r8, xmm6",
                            "    push r8",
                        ])),
                        _ => throw niet
                    },
                    _ => throw nie
                },
                TokenType.DIVISION => left.Type switch {
                    EvaluatedType.INT => right.Type switch {
                        EvaluatedType.XMM => new(EvaluatedType.XMM, Comp.Str([
                            left.Code,
                            right.Code,
                           $"    pop r8 ; {ToString()}",
                            "    movq xmm7, r8",
                            "    pop r8",
                            "    cvtsi2sd xmm6, r8",
                            "    divsd xmm6, xmm7 ; ДЕЛЕНИЕ",
                            "    movq r8, xmm6",
                            "    push r8",
                        ])),
                        _ => throw niet
                    },
                    EvaluatedType.XMM => right.Type switch {
                        EvaluatedType.INT => new(EvaluatedType.XMM, Comp.Str([
                            left.Code,
                            right.Code,
                           $"    pop r8 ; {ToString()}",
                            "    cvtsi2sd xmm7, r8",
                            "    pop r8",
                            "    movq xmm6, r8",
                            "    divsd xmm6, xmm7 ; ДЕЛЕНИЕ",
                            "    movq r8, xmm6",
                            "    push r8",
                        ])),
                        _ => throw niet
                    },
                    _ => throw nie
                },
                TokenType.DIV => left.Type switch {
                    EvaluatedType.INT => right.Type switch {
                        EvaluatedType.XMM => new(EvaluatedType.XMM, Comp.Str([
                            left.Code,
                            right.Code,
                           $"    pop r8 ; {ToString()}",
                            "    movq xmm7, r8",
                            "    pop r8",
                            "    cvtsi2sd xmm6, r8",
                            "    divsd xmm6, xmm7 ; ДЕЛЕНИЕ",
                            "    roundsd xmm6, xmm6, 1 ; ОКРУГЛЕНИЕ РЕЗУЛЬТАТА ДЕЛЕНИЯ",
                            "    movq r8, xmm6",
                            "    push r8",
                        ])),
                        _ => throw niet
                    },
                    EvaluatedType.XMM => right.Type switch {
                        EvaluatedType.INT => new(EvaluatedType.XMM, Comp.Str([
                            left.Code,
                            right.Code,
                           $"    pop r8 ; {ToString()}",
                            "    cvtsi2sd xmm7, r8",
                            "    pop r8",
                            "    movq xmm6, r8",
                            "    divsd xmm6, xmm7 ; ДЕЛЕНИЕ",
                            "    roundsd xmm6, xmm6, 1 ; ОКРУГЛЕНИЕ РЕЗУЛЬТАТА ДЕЛЕНИЯ",
                            "    movq r8, xmm6",
                            "    push r8",
                        ])),
                        _ => throw niet
                    },
                    _ => throw nie
                },
                TokenType.MOD => left.Type switch {
                    EvaluatedType.INT => right.Type switch {
                        EvaluatedType.XMM => new(EvaluatedType.XMM, Comp.Str([
                            left.Code,
                            right.Code,
                           $"    pop r8 ; {ToString()}",
                            "    movq xmm7, r8",
                            "    pop r8",
                            "    cvtsi2sd xmm6, r8",
                            "    movq r8, xmm6",
                            "    divsd xmm6, xmm7 ; ДЕЛЕНИЕ",
                            "    roundsd xmm6, xmm6, 1 ; ОКРУГЛЕНИЕ РЕЗУЛЬТАТА ДЕЛЕНИЯ",
                            "    mulsd xmm7, xmm6 ; (a // b) * b",
                            "    movq xmm6, r8 ; a",
                            "    subsd xmm6, xmm7 ; a - (a // b) * b === mod",
                            "    movq r8, xmm6",
                            "    push r8",
                        ])),
                        _ => throw niet
                    },
                    EvaluatedType.XMM => right.Type switch {
                        EvaluatedType.INT => new(EvaluatedType.XMM, Comp.Str([
                            left.Code,
                            right.Code,
                           $"    pop r8 ; {ToString()}",
                            "    cvtsi2sd xmm7, r8",
                            "    pop r8",
                            "    movq xmm6, r8",
                            "    divsd xmm6, xmm7 ; ДЕЛЕНИЕ",
                            "    roundsd xmm6, xmm6, 1 ; ОКРУГЛЕНИЕ РЕЗУЛЬТАТА ДЕЛЕНИЯ",
                            "    mulsd xmm7, xmm6 ; (a // b) * b",
                            "    movq xmm6, r8 ; a",
                            "    subsd xmm6, xmm7 ; a - (a // b) * b === mod",
                            "    movq r8, xmm6",
                            "    push r8",
                        ])),
                        _ => throw niet
                    },
                    _ => throw nie
                },
                _ => throw new Exception("НЕ КОМПИЛИРУЕМОЕ БИНАРНОЕ ДЕЙСТВИЕ")
            };
            throw nie;
        }
    }

    public override string ToString() => $"{Left} {Op.Type.View()} {Right}";
}
