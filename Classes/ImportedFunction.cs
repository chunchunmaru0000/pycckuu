namespace pycckuu;

public class ImportedFunction(string lib, string imp, string impAs, bool va, EvaluatedType t)
{
    public string Lib { get; set; } = lib;
    public string Imp { get; set; } = imp;
    public string ImpAs { get; set; } = impAs;
    public bool VariableArguments { get; set; } = va;
    public EvaluatedType ReturnType { get; set; } = t;
}
