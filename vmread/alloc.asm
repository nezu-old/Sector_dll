;alloc
mov rax, 0x1122334455667788
mov r10, 0x1122334455667788
mov r11, 0x1122334455667788
push rax
push rdi
push rcx
push rdx
push r8
push r9

push r10
push r11

sub rsp, 0x28

mov rax, 0xcccccccccccccccc
mov rcx, 0x0
mov rdx, 0x100000
mov r8d, 0x3000
mov r9d, 0x4
call rax

add rsp, 0x28

pop r11
pop r10

mov [r11], rax
mov rcx, 0
loop:
mov BYTE PTR [rax], 0x00
add rax, 0x1000
inc rcx
cmp rcx, 0xff
jle loop



pop r9
pop r8
pop rdx
pop rcx
pop rdi
pop rax

mov [r10], rax
jmp [r10]

;dealloc

sub rsp, 0x20
mov rax, 0xcccccccccccccccc
mov rcx, 0xaaaaaaaaaaaaaaaa
mov rdx, 0x0
mov r8d, 0x8000
call rax
add rsp, 0x20

;48 83 EC 28 48 B8 A0 9D E6 1E FF 7F 00 00 48 B9 0E 00 08 05 F7 01 00 00 BA 00 00 00 00 41 B8 00 80 00 00 FF D0 48 83 C4 28 B8 00 00 00 00 C3