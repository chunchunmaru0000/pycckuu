format PE64 console

include 'E:/fasm/include/win64a.inc'

section '.code' executable
entry _main

_main:
    mov r8, 8 ; ЦЕЛОЕ ЧИСЛО 8
    push r8

    mov r8, 3 ; ЦЕЛОЕ ЧИСЛО 3
    push r8

    pop r8
    pop rax
    xor rdx, rdx
    cqo ; РАСШИРЯЕТ ЗНАК С RAX В RDX
    idiv r8 ; ДЕЛЕНИЕ ИСПОЛЬЗУЕТ ЧИСЛО 128БИТ RDX:RAX ЗНАКОВОЕ
    push rdx
    mov r8, 10 ; ЦЕЛОЕ ЧИСЛО 10
    push r8

; 8 остаток 3 + 10
    pop r8
    pop r9
    add r8, r9 ; ПЛЮС
    push r8

    pop r8
    invoke printf, number, r8
    invoke ExitProcess, 0

section '.data' data readable writeable
    result db 'result>>> ', 0
    number dd '%lld', 0

section '.idata' import data readable writeable
    library kernel32, 'kernel32.dll', msvcrt, 'msvcrt.dll'

    import kernel32, ExitProcess, 'ExitProcess'
    import msvcrt, printf, 'printf', scanf, 'scanf'
