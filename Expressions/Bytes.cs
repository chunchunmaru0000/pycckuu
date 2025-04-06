namespace pycckuu;

public class BytesExpression(Token value): ICompilable
{
    private byte[] Bytes { get; set; } = [.. ((byte[])value.Value!).Reverse()];
    private EvaluatedType Type { get; set; } = U.T2T[value.Type];

    public Instruction Compile() => new(Type, Comp.Str([
        $"    push qword 0x{string.Join("", Bytes.Select(b => b.ToString("X2")))} ; {this}",
    ""]));

    public override string ToString() => $"БАЙТЫ {string.Join(",", Bytes.Select(b => b.ToString()))}";
}
