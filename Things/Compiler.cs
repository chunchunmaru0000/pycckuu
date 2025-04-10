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
            "    push rbp ; rbp actually not to shadow space but to align stack by 16",
            "    mov rbp, rsp"
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
            "    mov rsp, rbp",
			"    invoke ExitProcess, 0",
            "",
            Comp.Str([.. DeclaredFunctions]),
			"",
			"section '.data' data readable writeable",
            "    MINUS_ONE dq -1.0",
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

    private static List<Variable> Vars { get; set; } = [];

    public static KeyValuePair<bool, Variable> AddVariable(Variable var)
    {
        bool has = Vars.Any(v => v.Name == var.Name);
        if (!has) {
            Vars.Add(var);
            return new(false, var);
        }
        return new(true, GetVariable(var.Name));
    }

    public static Variable GetVariable(string name) =>
        Vars.Any(v => v.Name == name)
        ? Vars.First(v => v.Name == name)
        : throw new($"НЕТ ПЕРЕМЕННОЙ С ИМЕНЕМ {name}");

    public static List<Variable> ReplaceVariables(List<Variable> vars)
    {
        List<Variable> varsNow = Vars;
        Vars = vars;
        return varsNow;
    }

    public static int GetLastVarOffset() => Vars.Count == 0 ? 0 : Vars.Last().Offset;

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
        AddLibImports(new("kernel32", "ExitProcess", "ExitProcess", false, EvaluatedType.VOID));

		return Comp.Str([
            Begin().Code,
            new Parser(Tokens).ParseInstructions(),
            End().Code
        ]);
		//return string.Join(Platform == "w" ? "\r\n" : "\n", instructions);
	}
}