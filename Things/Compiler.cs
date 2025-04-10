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
            "    push rbp ; rbp actually not to shadow space but to align stack by 16"
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
			"    invoke ExitProcess, 0",
            "",
            Comp.Str([.. DeclaredFunctions]),
			"",
			"section '.data' data readable writeable",
            "    MINUS_ONE dq -1.0",
            "",
            Comp.Str([.. Vars.Select(v => $"    {v} dq 0")]),
            "",
            Comp.Str([.. Strings.Select(s => $"    {s.Value} db '{s.Key}', 0")]),
            "",
			"section '.idata' import data readable writeable",
            LibsImports.Count == 0 ? "" : Comp.Str([
           $"    library {string.Join(", ", LibsImports.Where(f => f.Key != "?").Select(p => $"{p.Key}, '{p.Key}'"))}",
            "",
                Comp.Str([.. LibsImports.Where(f => f.Key != "?").Select(li =>
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

    public static bool HaveImpFunc(string name) => 
        LibsFuncs.ContainsKey(name) && LibsFuncs[name].Lib != "?";

    public static ImportedFunction GetFuncImportedAs(string impAs) =>
        LibsFuncs.ContainsKey(impAs)
        ? LibsFuncs[impAs]
        : throw new Exception($"НЕТ ФУНКЦИИ С ИМЕНЕМ {impAs}");

    private static List<string> DeclaredFunctions { get; set; } = [];

    public static void DeclareFunction(string code) => DeclaredFunctions.Add(code);

    #endregion LIBS

    #region STRINGS

    public static Dictionary<string, string> Strings { get; private set; } = [];

    public static string AddString(string str)
    {
        string aStr = $"аСтрока{Strings.Count}";
        if (!Strings.ContainsKey(str)) {
            Strings[str] = aStr;
            return aStr;
        }
        else
            return Strings[str];
    }

    #endregion STRINGS

    #region VAR

    private static List<string> Vars { get; set; } = [];

    public static string SetVariable(Token var)
    {
        string name = var.Value!.ToString()!;
        if (!Vars.Contains(name))
            Vars.Add(name);
        return name;
    }

    #endregion VAR

    #region BLOCKS

    private static long Blocks { get; set; } = 0;

    public static KeyValuePair<string, string> AddBlock() =>
        new($"аБлокаНачало{Blocks}", $"аБлокаКонец{Blocks++}");

    private static long LoopLabels { get; set; } = 0;
    private static List<KeyValuePair<string, string>> LoopLabelsPool { get; set; } = [];

    public static KeyValuePair<string, string> AddLoopLabels()
    {
        KeyValuePair<string, string> labels =
            new($"аЦиклаНачало{LoopLabels}", $"аЦиклаКонец{LoopLabels++}");
        LoopLabelsPool.Add(labels);
        return labels;
    }

    public static void RemoveLastLoopLabels() =>
        LoopLabelsPool.RemoveAt(LoopLabelsPool.Count - 1);

    public static KeyValuePair<string, string> GetLastLoopLabels() => LoopLabelsPool.LastOrDefault();

    #endregion BLOCKS

    #region CONDITION_LABELS

    private static long ConditionLabels { get; set; } = 0;

    public static string AddLabel() => $"аМетка{ConditionLabels++}";

    #endregion CONDITION_LABELS

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