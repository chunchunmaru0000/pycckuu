namespace pycckuu
{
    public class Location(int line, int letter)
    {
        public int Line { get; set; } = line;
        public int Letter { get; set; } = letter;

        public override string ToString() => $"<СТРОКА {Line}, СИМВОЛ {Letter}>";
    }

}
