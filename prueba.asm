;Archivo: prueba.asm
;fecha: 11/3/2022 3:47:40 PM
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
	x DW ?
	y DW ?
	i DW ?
	j DW ?
	k DW ?
	l DW ?
	m DW ?
MOV AX, 256
PUSH AX
POP AX
MOV AX,0
PUSH AX
POP AX
MOV m, AX
MOV BX, m
PUSH BX
POP AX
inicioDo0:
PRINTN ""
PRINT "Ete sech\n"
INC i
MOV BX, i
PUSH BX
MOV AX, 10
PUSH AX
POP BX
POP AX
CMP AX,BX
JLE finDo0
JMP inicioDo0
finDo0:
END
RET
DEFINE_SCAN_NUM
