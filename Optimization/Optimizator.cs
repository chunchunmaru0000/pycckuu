namespace pycckuu;
/*
public class Optimizator
{
    private string Asm { get; }
    private string Begin { get; }
    private string End { get; }
    private AsmInstruction[] Instructions { get; }
    private IWorder Worder { get; }

    public Optimizator(string asm, IWorder worder)
    {
        Asm = asm;
        Worder = worder;

        Begin = Asm.Split("_main:")[0] + "_main:";
        End = "section '.data'" + Asm.Split("section '.data'").Last();
        Asm = Asm[Begin.Length..(Asm.Length - End.Length)];
        Instructions = Parse(Asm.Replace("\r", "").Split('\n'));
        /*
        Console.WriteLine(Begin);
        Console.WriteLine(Asm);
        Console.WriteLine(End);
         //
    }

    private AsmInstruction[] Parse(string[] lines) => [..
        lines.Where(l => !string.IsNullOrWhiteSpace(l))
        .Select(ParseLine)];

    private AsmInstruction ParseLine(string line)
    {
        line = line.Split(';')[0];
        string quote = string.Join(";", line.Split(';')[1..]);
        Token[] tokens = new Tokenizator(line, Worder).Tokenize();
    }

    private string[] OptimizeLines(AsmInstruction[] lines)
    {

    }

    public string Optimize() => Comp.Str([
    Begin,
        Comp.Str(OptimizeLines(Instructions)),
        End,
    ]);
}

 */