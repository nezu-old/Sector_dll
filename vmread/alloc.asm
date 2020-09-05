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
mov r9d, 0x40
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
cmp rcx, 0x100
jle loop



pop r9
pop r8
pop rdx
pop rcx
pop rdi
pop rax

mov [r10], rax
jmp [r10]