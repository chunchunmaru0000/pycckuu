﻿namespace Pycckuu
{
	public class Compiler(string platform)
	{
		private string Platform { get; set; } = platform;

		private string Begin() => Platform == "w" ?
			string.Join("\r\n", [
				"format PE64 console",
				"include 'win64a.inc'",
				"section '.code' executable",
				"entry main",
				"main:"
				]) :
			string.Join("\n", [
				"format ELF64 executable 3",
				"segment readable executable",
				"entry main",
				"main:",
				""
				]);

		private string End() => Platform == "w" ?
			string.Join("\r\n", [
				"    invoke printf, result",
				"    invoke printf, number, r8", // пока просто надо сначала калькулятор сделать с результатом в r8
				"    invoke ExitProcess, 0",
				"section '.data' data readable writeable",
				"    result db 'result>>> ', 0",
				"    number dd '%lld', 0, 10",
				"section '.idata' import data readable writeable",
				"    library kernel32, 'kernel32.dll', msvcrt, 'msvcrt.dll'",
				"    import kernel32, ExitProcess, 'ExitProcess'",
				"    import msvcrt, printf, 'printf', scanf, 'scanf'"]) :
			string.Join("\n", [
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

			instructions.Add(End());

			return string.Join(Platform == "w" ? "\r\n" : "\n", instructions);
		}
	}
}