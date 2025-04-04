namespace pycckuu;

public class LogAttribute(string value) : Attribute
{
    public string Value { get; } = value;
}

public class ViewAttribute(string value) : Attribute
{
    public string Value { get; } = value;
}

public static class EnumExtensions
{
    public static string View(this Enum value)
    {
        var type = value.GetType();
        var fieldInfo = type.GetField(value.ToString());
        var attribs = fieldInfo!.GetCustomAttributes(typeof(LogAttribute), false) as LogAttribute[];
        return attribs!.Length > 0 ? attribs[0].Value : value.Log();
    }

    public static string Log(this Enum value)
    {
        var type = value.GetType();
        var fieldInfo = type.GetField(value.ToString());
        var attribs = fieldInfo!.GetCustomAttributes(typeof(ViewAttribute), false) as ViewAttribute[];
        return attribs!.Length > 0 ? attribs[0].Value : value.ToString();
    }
}
