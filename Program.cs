namespace pycckuu;

class Program
{
	private static string AsmName = "some.asm";
	private static string AsmPath = $"V:\\langs\\as\\test\\{AsmName}";
	private static string IncPath = "V:\\fasm\\INCLUDE\\";


        static void Main(string[] args)
	{
		Console.Write(">>> ");
		Token[] tokens = new Tokenizator(Console.ReadLine()!).Tokenize();

		// "w" for windows
		string code = new Compiler("w", IncPath + "win64a.inc", tokens).Compile();
		Console.WriteLine(code);
		Console.ReadLine();
		File.WriteAllText(AsmPath, code);

		Console.ReadLine();
	}
}