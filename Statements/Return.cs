namespace pycckuu;

public class ReturnStatement(ICompilable value) : ICompilable
{
    public ICompilable Value { get; } = value;

    public Instruction Compile()
    {
        Instruction value = Value.Compile();
        int argsOffset = Compiler.GetLastVarOffset();
        argsOffset += argsOffset % 16;
        return new(value.Type, Comp.Str([
            value.Code,
            $"    pop rax",
            $"    add rsp, {argsOffset} ; ОЧИЩЕНИЕ СТЕКА ОТ ПАРАМЕТРОВ",
            $"    pop rbp ; ВЫКРИВДЕНИЕ СТЭКА НАЗАД ЧТОБЫ ПОТОМ ОН БЫЛ ВОССТАНОВЛЕН САМ ЧЕРЕЗ pop rip ПРИ ret",
            $"    ret ; ВОЗВРАЩАЕТ ЗНАЧЕНИЕ ТИПА {value.Type.Log()}"
        ]));
    }
}
