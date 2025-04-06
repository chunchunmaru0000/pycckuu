namespace pycckuu;

public class ImportedFunction
{
    public string Lib { get; set; }
    public string Imp { get; set; }
    public string ImpAs { get; set; }
    public bool VariableArguments { get; set; }
    public EvaluatedType ReturnType { get; set; } = EvaluatedType.VOID;
}
