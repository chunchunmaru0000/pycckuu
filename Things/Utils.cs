﻿namespace pycckuu;

public static class U
{
    public static string YetCant(string name, string action) =>
        $"{name} ПОКА НЕ УМЕЕТ {action}";

    public static NotImplementedException YetCantEx(string name, string action) =>
        new($"{name} ПОКА НЕ УМЕЕТ {action}");

    public static Dictionary<int, string> Sizes { get; set; } = new() {
        { 1, "byte" }, { 4, "dword" },
        { 2, "word" }, { 8, "qword" },
    };

    public static Dictionary<int, string> RRegs { get; set; } = new() {
        { 1, "b" }, { 2, "w" },
        { 4, "d" }, { 8, "" },
    };

    public static Dictionary<TokenType, EvaluatedType> T2T { get; set; } = new() {
        { TokenType.BYTE, EvaluatedType.BYTE },
        { TokenType.DBYTE, EvaluatedType.DBYTE },
        { TokenType.CHBYTE, EvaluatedType.CHBYTE },
        { TokenType.VBYTE, EvaluatedType.VBYTE },
    };

    public static Dictionary<long, EvaluatedType> SizeToType { get; set; } = new() {
        { 1, EvaluatedType.BYTE }, 
        { 2, EvaluatedType.DBYTE },
        { 4, EvaluatedType.CHBYTE }, 
        { 8, EvaluatedType.VBYTE },
    };

    public static string[] Registers { get; } = ["rcx", "rdx", "r8", "r9"];
}
