namespace pycckuu
{
	public interface IExecutable
	{
		public void Execute();
	}

	public interface IExpression
	{
		public object Evaluate();
	}
		
	public interface ICompilable
	{
		public Instruction Compile();
	}

	public interface ICompilableExpression: IExpression, ICompilable;

	public static class Comp
	{
		public static string Str(string[] sArr) => string.Join("\r\n", sArr);
	}
}