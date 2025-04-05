namespace pycckuu;

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
			"    invoke printf, number, r8", // пока просто надо сначала калькулятор сделать с результатом в r8
			"    invoke ExitProcess, 0",
			"",
			"section '.data' data readable writeable",
			"    result db 'result>>> ', 0",
			"    number db '%lld', 0",
			"    float  db '%llf', 0",
                "	 MINUS_ONE dq -1.0",
                "",
			"section '.idata' import data readable writeable",
			"    library kernel32, 'kernel32.dll', msvcrt, 'msvcrt.dll'",
			"",
			"    import kernel32, ExitProcess, 'ExitProcess'",
			"    import msvcrt, printf, 'printf', scanf, 'scanf'",
			""
		]) :
		Comp.Str([
			"	 	 print",
			"    xor rdi, rdi",
			"    mov rax, 60",
			"    syscall",
			""
		]));

	public string Compile()
	{
		List<Instruction> instructions = [];
		instructions.Add(Begin());

		// instructions.Add(new ) print instruction
		instructions.Add(new Parser(Tokens).ParseInstructions().Compile());

		instructions.Add(End());

		return string.Join(Platform == "w" ? "\r\n" : "\n", instructions.Select(i => i.Code));
	}
}