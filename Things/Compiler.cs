﻿namespace pycckuu;

public class Compiler(string platform, string includePath, Token[] tokens)
{
	private string includePath = includePath;

	private string Platform { get; set; } = platform;
	private string IncludePath { get => includePath; set => includePath = value; }
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
			"    pop r8",
            "    invoke printf, float, r8", // пока просто надо сначала калькулятор сделать с результатом в r8
			"    invoke ExitProcess, 0",
			"",
			"section '.data' data readable writeable",
			"    result db 'result>>> ', 0",
			"    number db '%lld', 0",
			"    float  db '%llf', 0",
            "	 MINUS_ONE dq -1.0",
            "",
			"section '.idata' import data readable writeable",
            LibsImports.Count == 0 ? "" : Comp.Str([
                $"    library {string.Join(", ", LibsImports.Select(p => $"{p.Key}, '{p.Key}'"))}",
                "",
                Comp.Str([.. LibsImports.Select(li =>
                    $"    import {li.Key}, {string.Join(", ", li.Value.Select(i => $"{i.Key}, '{i.Value}'"))}"
                )])
            ]),
			""
		]) :
		Comp.Str([
			"	 	 print",
			"    xor rdi, rdi",
			"    mov rax, 60",
			"    syscall",
			""
		]));

    private static Dictionary<string, Dictionary<string, string>> LibsImports { get; set; } = [];
    public static void AddLibImports(string lib, string import, string importAs)
    {
        if (!LibsImports.ContainsKey(lib))
            LibsImports[lib] = new() { { import, importAs } };
        else
            LibsImports[lib][import] = importAs;
    }


    public string Compile()
	{
		List<Instruction> instructions = [];
		instructions.Add(Begin());

		instructions.Add(new Parser(Tokens).ParseInstructions().Compile());

        AddLibImports("kernel32", "ExitProcess", "ExitProcess");
        AddLibImports("msvcrt", "printf", "printf");

        instructions.Add(End());

		return string.Join(Platform == "w" ? "\r\n" : "\n", instructions.Select(i => i.Code));
	}
}