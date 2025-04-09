namespace pycckuu;

public class ReturnStatement(ICompilable value) : ICompilable
{
    public ICompilable Value { get; } = value;

    public Instruction Compile()
    {
        Instruction value = Value.Compile();
        return new(value.Type, Comp.Str([
            value.Code,
            $"    pop rax",
            $"    ret ; ВОЗВРАЩАЕТ ЗНАЧЕНИЕ ТИПА {value.Type}"
        ]));
    }
}
