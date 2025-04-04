namespace pycckuu;

public class Token
{
    public object? Value { get; set; }
    public TokenType Type { get; set; }

    public required Location Location { get; set; }

    public Token Clone() => new() { Value = Value, Type = Type, Location = Location };

    public override string ToString() => $"<{Convert.ToString(Value)}> <{Type.Log()};{Type.View()}> <{Location}>";
}
