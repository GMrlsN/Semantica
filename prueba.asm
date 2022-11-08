;Archivo: prueba.asm
;fecha: 11/7/2022 8:12:12 PM
#make_COM#
include "emu8086.inc"
ORG 100H


;Variables: 
	area DW ?
	radio DW ?
	pi DW ?
	resultado DW ?
	a DW ?
	d DW ?
	altura DW ?
	cinco DW ?
	x DW ?
	y DW ?
	i DW ?
	j DW ?
	k DW ?
	ln DW ?
PRINT "Introduce la altura de la piramide: "
CALL SCAN_NUM
MOV altura, CX
MOV BX, altura
PUSH BX
MOV AX, 2
PUSH AX
POP BX
POP AX
CMP AX,BX
JLE if1
MOV BX, altura
PUSH BX
POP AX
MOV i, AX
inicioFor0:
MOV BX, i
PUSH BX
MOV AX, 0
PUSH AX
POP BX
POP AX
CMP AX,BX
JLE finFor0
MOV AX, 0
PUSH AX
POP AX
MOV j, AX
inicioWhile1:
MOV BX, j
PUSH BX
MOV BX, altura
PUSH BX
MOV BX, i
PUSH BX
POP BX
POP AX
SUB AX, BX
PUSH AX
POP BX
POP AX
CMP AX,BX
JGE finWhile1
MOV BX, j
PUSH BX
MOV AX, 2
PUSH AX
POP BX
POP AX
DIV BX
PUSH DX
MOV AX, 0
PUSH AX
POP BX
POP AX
CMP AX,BX
JNE if3
PRINT "*"
JMP else4
if3:
PRINT "-"
else4:
INC j
JMP inicioWhile1
finWhile1:
PRINTN ""
PRINT ""
DEC i
JMP inicioFor0
finFor0:
MOV AX, 0
PUSH AX
POP AX
MOV k, AX
inicioDo12:
PRINT "-"
MOV AX, 2
PUSH AX
ADD k, 2
MOV BX, k
PUSH BX
MOV BX, altura
PUSH BX
MOV AX, 2
PUSH AX
POP BX
POP AX
MUL BX
PUSH AX
POP BX
POP AX
CMP AX,BX
JGE finDo12
JMP inicioDo12
finDo12:
PRINTN ""
PRINT ""
JMP else2
if1:
PRINTN ""
PRINTN ""
PRINT "Error: la altura debe de ser mayor que 2"
else2:
MOV AX, 1
PUSH AX
MOV AX, 1
PUSH AX
POP BX
POP AX
CMP AX,BX
JE if117
PRINT "Esto no se debe imprimir"
MOV AX, 2
PUSH AX
MOV AX, 2
PUSH AX
POP BX
POP AX
CMP AX,BX
JNE if119
PRINT "Esto tampoco"
if119:
if117:
MOV AX, 258
PUSH AX
POP AX
MOV a, AX
PRINT "Valor de variable int a antes del casteo: "
MOV BX, a
PUSH BX
POP AX
CALL PRINTf
MOV BX, a
PUSH BX
POP AX
MOV AX,2
PUSH AX
POP AX
MOV y, AX
PRINTN ""
PRINT "Valor de variable char y despues del casteo de a: "
MOV BX, y
PUSH BX
POP AX
CALL PRINTf
PRINTN ""
PRINTN ""
PRINT "A continuacion se intenta asignar un int a un char sin usar casteo: "
JMP fin
PRINTf PROC
mov cx,0
mov dx,0
label1:
cmp ax,0
je print1
mov bx,10
div bx
push dx
inc cx
xor dx,dx
jmp label1
print1:
cmp cx,0
je exit
pop dx
add dx,48
mov ah,02h
int 21h
dec cx
jmp print1
exit:
ret
PRINTf ENDP
fin:
DEFINE_SCAN_NUM
END
RET
