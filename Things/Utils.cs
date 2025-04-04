namespace pycckuu;

public static class U
{
    public static string YetCant(string name, string action) =>
        $"{name} ПОКА НЕ УМЕЕТ {action}";

    public static Exception YetCantEx(string name, string action) =>
        new NotImplementedException($"{name} ПОКА НЕ УМЕЕТ {action}");
}
