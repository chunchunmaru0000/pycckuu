namespace Pycckuu
{
	public class Compiler(string platform, string includePath, Token[] tokens)
	{
		private string includePath = includePath;

		private string Platform { get; set; } = platform;
		private string IncludePath { get => includePath; set => includePath = value; }
		private Token[] Tokens { get; set; } = tokens;

		private string Begin() => Platform == "w" ?
			Comp.Str([
				"format PE64 console",
				"",
				$"include '{IncludePath}'",
				"",
				"section '.code' executable",
				"entry _main",
				"",
				"_main:"
			]) :
			Comp.Str([
				"format ELF64 executable 3",
				"",
				"segment readable executable",
				"entry _main",
				"",
				"_main:"
			]);

		private string End() => Platform == "w" ?
			Comp.Str([
			/*
				"    mov rcx, result",
				"    call [printf]",
				"    mov rcx, number",
				"    pop rdx",
				"    pop rdx",
				"    pop rdx", // зачем почему и вообще как надо три раза его так делать
				"    call [printf]",*/
				"    pop r8",
				"    invoke printf, number, r8", // пока просто надо сначала калькулятор сделать с результатом в r8
				"    invoke ExitProcess, 0",
				"",
				"section '.data' data readable writeable",
				"    result db 'result>>> ', 0",
				"    number dd '%lld', 0, 10",
				"",
				"section '.idata' import data readable writeable",
				"    library kernel32, 'kernel32.dll', msvcrt, 'msvcrt.dll'",
				"",
				"    import kernel32, ExitProcess, 'ExitProcess'",
				"    import msvcrt, printf, 'printf', scanf, 'scanf'",
				""
			]) :
			Comp.Str([
				"    сделать вывод r8 в консоль как результата",
				"    xor rdi, rdi",
				"    mov rax, 60",
				"    syscall",
				""
			]);

		public string Compile()
		{
			List<string> instructions = [];
			instructions.Add(Begin());

		// здесь компилировать
			// instructions.Add("    mov r8, 0");
			// instructions.Add("    push r8");
			instructions.Add(new Parser(Tokens).ParseInstructions().Compile());

			instructions.Add(End());

			return string.Join(Platform == "w" ? "\r\n" : "\n", instructions);
		}
	}
}