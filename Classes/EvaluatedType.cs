namespace pycckuu;

public enum EvaluatedType
{
    [Log("ИНТ")][Size(8)]INT,
    [Log("ХММ")][Size(8)]XMM,
    [Log("СТР")][Size(1)]STR, // 1 is for char if it will be ever used
    [Log("ПТР")][Size(8)]PTR,
    [Log("БУЛ")][Size(8)]BOOL,
    [Log("БАЙТ")][Size(1)]BYTE,
    [Log("2БАЙТ")][Size(2)]DBYTE,
    [Log("4БАЙТ")][Size(4)]CHBYTE,
    [Log("8БАЙТ")][Size(8)]VBYTE,

    [Log("ИНТ")][Size(0)]VOID, // not like in C here its literally void as nothing
    [Log("ИНТ")][Size(8)]CALL,

    [Log("НАЧ")][Size(0)]BEGIN_PROGRAM,
    [Log("КНЦ")][Size(0)]END_PROGRAM,
    [Log("ИМП")][Size(0)]IMPORT,
}
