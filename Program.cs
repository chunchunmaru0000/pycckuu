namespace pycckuu;

class Program
{
	private static string AsmName = "some.asm";
	private static string AsmPath = $"V:\\langs\\as\\test\\{AsmName}";
	private static string IncPath = "V:\\fasm\\INCLUDE\\";
    private static string FileName = "V:\\langs\\kptyata#\\pycckuu\\Tests\\sqlite.эээ";

    static void Main(string[] args)
	{
        // Console.Write(">>> ");
        //string input = Console.ReadLine()!;
        string input = File.ReadAllText(FileName, System.Text.Encoding.UTF8);
		Token[] tokens = new Tokenizator(input).Tokenize();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(input);
        Console.WriteLine("#############################################");
        Console.WriteLine(string.Join("|", tokens.Select(t => t.Value ?? t.Type.Log())));
        Console.WriteLine("#############################################");
        Console.ForegroundColor = ConsoleColor.White;

		// "w" for windows
		string code = new Compiler("w", IncPath + "win64a.inc", tokens).Compile();
		Console.WriteLine(code);
		Console.ReadLine();
		File.WriteAllText(AsmPath, code);

		Console.ReadLine();
	}
}