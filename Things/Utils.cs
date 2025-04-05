namespace pycckuu;

public static class U
{
    public static string YetCant(string name, string action) =>
        $"{name} ПОКА НЕ УМЕЕТ {action}";

    public static NotImplementedException YetCantEx(string name, string action) =>
        new($"{name} ПОКА НЕ УМЕЕТ {action}");
}
