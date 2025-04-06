namespace pycckuu;

public enum EvaluatedType
{
    [Size(8)]INT,
    [Size(8)]XMM,
    [Size(1)]STR, // 1 is for char if it will be ever used
    [Size(8)]PTR,
    [Size(8)]BOOL,
    [Size(1)]BYTE,
    [Size(2)]DBYTE,

    [Size(0)]VOID, // not like in C here its literally void as nothing
    [Size(8)]CALL,

    [Size(0)]BEGIN_PROGRAM,
    [Size(0)]END_PROGRAM,
    [Size(0)]IMPORT,
}
