namespace pycckuu;

public class Compiler(string platform, string includePath, Token[] tokens)
{
	private string Platform { get; set; } = platform;
    private string IncludePath { get; set; } = includePath;
    private Token[] Tokens { get; set; } = tokens;

	private Instruction Begin() => new(EvaluatedType.BEGIN_PROGRAM,
		Platform == "w" ?
		Comp.Str([
			"format PE64 console",
			"",
			$"include '{IncludePath}'",
			"",
			"section '.code' executable",
			"entry _main",
			"",
			"_main:",
            "    sub rsp, 40 ; shadow space"
		]) :
		Comp.Str([
			"format ELF64 executable",
			"",
			"segment readable executable",
			"entry _main",
			"",
			"_main:"
		]));

	private Instruction End() => new (EvaluatedType.END_PROGRAM, 
		Platform == "w" ?
		Comp.Str([
			//"    pop r8",
            //"    invoke printf, float, r8",
			"    invoke ExitProcess, 0",
			"",
			"section '.data' data readable writeable",
			"    result db 'result>>> ', 0",
			"    number db '%lld', 0",
			"    float  db '%llf', 0",
            "	 MINUS_ONE dq -1.0",
            "",
            Comp.Str([.. Strings.Select(s => $"    {s.Value} db '{s.Key}', 0")]),
            "",
			"section '.idata' import data readable writeable",
            LibsImports.Count == 0 ? "" : Comp.Str([
                $"    library {string.Join(", ", LibsImports.Select(p => $"{p.Key}, '{p.Key}'"))}",
                "",
                Comp.Str([.. LibsImports.Select(li =>
                    $"    import {li.Key}, {string.Join(", ", li.Value.Select(i => $"{i.Value}, '{i.Key}'"))}"
                )])
            ]),
			""
		]) :
		Comp.Str([
			"    xor rdi, rdi",
			"    mov rax, 60",
			"    syscall",
			""
		]));

    #region LIBS
    private static Dictionary<string, Dictionary<string, string>> LibsImports { get; set; } = [];
    private static Dictionary<string, ImportedFunction> LibsFuncs { get; set; } = [];

    public static void AddLibImports(ImportedFunction f)
    {
        if (!LibsImports.ContainsKey(f.Lib))
            LibsImports[f.Lib] = new() { { f.Imp, f.ImpAs } };
        else
            LibsImports[f.Lib][f.Imp] = f.ImpAs;
        LibsFuncs[f.ImpAs] = f;
    }

    public static ImportedFunction GetFuncImportedAs(string impAs) =>
        LibsFuncs.ContainsKey(impAs)
        ? LibsFuncs[impAs]
        : throw new Exception($"НЕТ ФУНКЦИИ С ИМЕНЕМ {impAs}");

    #endregion LIBS

    #region STRINGS

    public static Dictionary<string, string> Strings { get; private set; } = [];

    public static string AddString(string str)
    {
        string aStr = $"aStr{Strings.Count}";
        if (!Strings.ContainsKey(str)) {
            Strings[str] = aStr;
            return aStr;
        }
        else
            return Strings[str];
    }

    #endregion STRINGS

    public string Compile()
	{
		List<string> instructions = [];
		instructions.Add(Begin().Code);

		instructions.Add(new Parser(Tokens).ParseInstructions());

        AddLibImports(new("kernel32", "ExitProcess", "ExitProcess", false, EvaluatedType.VOID));
        instructions.Add(End().Code);

		return string.Join(Platform == "w" ? "\r\n" : "\n", instructions);
	}
}