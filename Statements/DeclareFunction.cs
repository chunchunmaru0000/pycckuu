namespace pycckuu;

public class DeclareFunctionStatement(string name, bool typed, List<Token> args, BlockStatement body) : ICompilable
{
    public string Name { get; } = name;
    public bool Typed { get; } = typed;
    public List<Token> Args { get; } = args;
    public BlockStatement Body { get; } = body;

    public Instruction Compile()
    {
        Compiler.AddLibImports(new("?", Name, Name, false, Typed ? EvaluatedType.INT : EvaluatedType.VOID));

        string[] args = [.. Args.Select(Compiler.SetVariable)];
        int extraArgs = args.Length - 4;

        string code = Comp.Str([
            $"{Name}: ; ФУНКЦИЯ {Name}",
            Comp.Str([.. Enumerable.Range(0, args.Length).Reverse().Select(i =>
                i >= U.Registers.Length
                ? Comp.Str([
                    $"    mov r10, [rsp + {8 * extraArgs--}]",
                    $"    mov qword[{args[i]}], r10",
                ])
                :   $"    mov qword[{args[i]}], {U.Registers[i]}"
            )]),
            Body.Compile().Code,
            $"    ret ; КОНЕЦ ФУНКЦИИ {Name} КОТОРЫЙ НЕ ДОЛЖЕН БЫТЬ ДОСЯГАЕМ ЕСЛИ ФУНКЦИЯ ЧТОТО ВОЗВРАЩАЕТ",
        ]);

        Compiler.DeclareFunction(code);
        return new(EvaluatedType.VOID, "");
    }
}
