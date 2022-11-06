;Archivo: prueba.asm
;fecha: 11/4/2022 3:55:48 PM
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
PRINTN "Introduce la altura de la piramide: "
CALL SCAN_NUM
MOV altura, CX
MOV AX, 0
PUSH AX
POP AX
MOV i, AX
inicioFor0:
MOV BX, i
PUSH BX
MOV AX, 6
PUSH AX
POP BX
POP AX
CMP AX,BX
JGE finFor0
MOV BX, i
PUSH BX
POP AX
PRINTN " hola \n"
INC i
JMP inicioFor0
