;Archivo: prueba.asm
;fecha: 11/6/2022 12:30:57 AM
#make_COM#
include emu8086.inc
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
PRINTN "Introduce la altura de la piramide: "
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
PRINTN "*"
JMP else4
if3:
PRINTN "-"
else4:
MOV AX, 1
PUSH AX
ADD j, 1
JMP inicioWhile1
finWhile1:
PRINTN "\n"
SUB i, 1
JMP inicioFor0
finFor0:
MOV AX, 0
PUSH AX
POP AX
MOV k, AX
inicioDo7:
PRINTN "-"
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
JGE finDo7
JMP inicioDo7
finDo7:
PRINTN "\n"
JMP else2
if1:
PRINTN "\nError: la altura debe de ser mayor que 2\n"
else2:
MOV AX, 1
PUSH AX
MOV AX, 1
PUSH AX
POP BX
POP AX
CMP AX,BX
JE if37
PRINTN "Esto no se debe imprimir"
MOV AX, 2
PUSH AX
MOV AX, 2
PUSH AX
POP BX
POP AX
CMP AX,BX
JNE if39
PRINTN "Esto tampoco"
if39:
if37:
MOV AX, 258
PUSH AX
POP AX
MOV a, AX
PRINTN "Valor de variable int 'a' antes del casteo: "
MOV BX, a
PUSH BX
POP AX
MOV BX, a
PUSH BX
POP AX
MOV AX,2
PUSH AX
POP AX
MOV y, AX
PRINTN "\nValor de variable char 'y' despues del casteo de a: "
MOV BX, y
PUSH BX
POP AX
PRINTN "\nA continuacion se intenta asignar un int a un char sin usar casteo: \n"
END
RET
DEFINE_SCAN_NUM
