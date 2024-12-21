namespace Pycckuu
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Write(">>> ");
			foreach (Token s in new Tokenizator(Console.ReadLine()).Tokenize())
				Console.WriteLine(s.ToString());
			Console.WriteLine(new Compiler("w").Compile());
			File.WriteAllText("E:\\fasm\\some.asm", new Compiler("w").Compile());
			Console.ReadLine();
		}
	}
}