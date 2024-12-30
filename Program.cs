namespace Pycckuu
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Write(">>> ");
			Token[] tokens = new Tokenizator(Console.ReadLine()!).Tokenize();
			//foreach (Token s in tokens)
			//	Console.WriteLine(s.ToString());

			string code = new Compiler("w", "E:/fasm/include/win64a.inc", tokens).Compile();
			Console.WriteLine(code);
			Console.ReadLine();
			// "w" for windows
			File.WriteAllText("C:\\Users\\user\\desktop\\some.asm", code);
			File.WriteAllText("some.asm", code);

			//Console.WriteLine(new Compiler("w").Compile());
			Console.ReadLine();
		}
	}
}