format PE64 console

include 'E:/fasm/include/win64a.inc'

section '.code' executable
entry _main

_main:
    mov r8, 2 ; ЦЕЛОЕ ЧИСЛО 2
    push r8

    mov r8, 2 ; ЦЕЛОЕ ЧИСЛО 2
    push r8

    pop r8
    pop r9
    imul r8, r9 ; УМНОЖЕНИЕ
    push r8

    mov r8, 10 ; ЦЕЛОЕ ЧИСЛО 10
    push r8

    pop r8
    neg r8 ; ПОМЕНЯТЬ ЗНАК
    push r8

    mov r8, 2 ; ЦЕЛОЕ ЧИСЛО 2
    push r8

    pop r8
    pop rax
    xor rdx, rdx
    cqo ; РАСШИРЯЕТ ЗНАК С RAX В RDX
    idiv r8 ; ДЕЛЕНИЕ ИСПОЛЬЗУЕТ ЧИСЛО 128БИТ RDX:RAX ЗНАКОВОЕ
    push rax

    pop r8
    pop r9
    add r8, r9 ; ПЛЮС
    push r8

    pop r8
    invoke printf, number, r8
    invoke ExitProcess, 0

section '.data' data readable writeable
    result db 'result>>> ', 0
    number dd '%lld', 0, 10

section '.idata' import data readable writeable
    library kernel32, 'kernel32.dll', msvcrt, 'msvcrt.dll'

    import kernel32, ExitProcess, 'ExitProcess'
    import msvcrt, printf, 'printf', scanf, 'scanf'
