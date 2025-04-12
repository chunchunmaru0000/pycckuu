namespace pycckuu;

class Program
{
	private static string AsmName = "some.asm";
	private static string AsmPath = $"V:\\langs\\as\\test\\{AsmName}";
	private static string IncPath = "V:\\fasm\\INCLUDE\\";
    private static string FileName = "V:\\langs\\kptyata#\\pycckuu\\Tests\\file.эээ";

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
}