namespace Pycckuu
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Write(">>> ");
			Token[] tokens = new Tokenizator(Console.ReadLine()!).Tokenize();
			foreach (Token s in tokens)
				Console.WriteLine(s.ToString());

			// "w" for windows
			File.WriteAllText("E:\\fasm\\some.asm", new Compiler("w", tokens).Compile());
			File.WriteAllText("some.asm", new Compiler("w", tokens).Compile());

			//Console.WriteLine(new Compiler("w").Compile());
			Console.ReadLine();
		}
	}
}