namespace pycckuu;

class Program
{
	private static string ExitEnter(string msg, ConsoleColor color = ConsoleColor.Red)
	{
		Console.ForegroundColor = color;
		Console.WriteLine(msg);
		Console.ResetColor();
		Console.WriteLine("[Enter] чтобы выйти");
		Console.ReadLine();
		Environment.Exit(0);
		return "";
	}
#if DEBUG
	private static string AsmName = "some.asm";
	private static string AsmPath = $"V:\\langs\\as\\test\\{AsmName}";
	private static string IncPath = "V:\\fasm\\INCLUDE\\";
    private static string FileName = "V:\\langs\\kptyata#\\pycckuu\\Tests\\wnd.эээ";

    static void Main(string[] args)
	{
        string input = File.ReadAllText(FileName, System.Text.Encoding.UTF8);
		Token[] tokens = new Tokenizator(input).Tokenize();

		// "w" for windows
		string code = new Compiler("w", IncPath + "win64a.inc", tokens).Compile();
		Console.WriteLine(code);
		Console.ReadLine();
		File.WriteAllText(AsmPath, code);

		Console.ReadLine();
	}
#else
	static void Main(string[] args)	
	{
		if (args.Length < 1)
			ExitEnter($"НЕДОСТАТОЧНО АГРУМЕНТОВ, БЫЛО ВСЕГО[{args.Length}]");
        string path = args[0];

		if (!File.Exists(path))
			ExitEnter($"ФАЙЛ НЕ СУЩЕСТВУЕТ [{path}]");

		string incPath = args.FirstOrDefault(a => a.ToLower().EndsWith(".inc"), 
			Path.Combine(Path.GetDirectoryName(path)!, "INCLUDE/WIN64A.INC"));
		if (!File.Exists(incPath))
			ExitEnter($"ФАЙЛ НЕ СУЩЕСТВУЕТ [{incPath}]");

        string asm = args.FirstOrDefault(a => a.ToLower().EndsWith(".asm"),
            Path.ChangeExtension(path, ".asm"));

        string input = File.ReadAllText(path, System.Text.Encoding.UTF8);
		Token[] tokens = new Tokenizator(input).Tokenize();
		string code = new Compiler("w", incPath, tokens).Compile();
		Console.WriteLine(code);

		Console.WriteLine($"{asm}\n");
		Console.ReadLine();
		File.WriteAllText(asm, code);
	}

#endif
}