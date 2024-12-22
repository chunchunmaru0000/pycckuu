format PE64 console
include 'win64a.inc'
section '.code' executable
entry main
main:
    mov r8, 0
    push r8
    mov rcx, result
    call [printf]
    mov rcx, number
    pop rdx
    pop rdx
    pop rdx
    call [printf]
    invoke ExitProcess, 0
section '.data' data readable writeable
    result db 'result>>> ', 0
    number dd '%lld', 0, 10
section '.idata' import data readable writeable
    library kernel32, 'kernel32.dll', msvcrt, 'msvcrt.dll'
    import kernel32, ExitProcess, 'ExitProcess'
    import msvcrt, printf, 'printf', scanf, 'scanf'
