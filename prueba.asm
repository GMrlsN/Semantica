;Archivo: prueba.asm
;fecha: 10/30/2022 3:50:44 PM
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
inicioFor0:
MOV AX, 0
PUSH AX
POP AX
MOV i, AX
MOV AX, 10
PUSH AX
POP AX
POP BX
CMP AX,BX
JGE 
MOV AX, 2
PUSH AX
POP AX
PRINTN "Hola\n"
MOV AX, 10
PUSH AX
POP AX
POP BX
CMP AX,BX
JGE 
MOV AX, 2
PUSH AX
POP AX
PRINTN "Hola\n"
MOV AX, 10
PUSH AX
POP AX
POP BX
CMP AX,BX
JGE 
MOV AX, 2
PUSH AX
POP AX
PRINTN "Hola\n"
MOV AX, 10
PUSH AX
POP AX
POP BX
CMP AX,BX
JGE 
MOV AX, 2
PUSH AX
POP AX
PRINTN "Hola\n"
MOV AX, 10
PUSH AX
POP AX
POP BX
CMP AX,BX
JGE 
MOV AX, 2
PUSH AX
POP AX
PRINTN "Hola\n"
MOV AX, 10
PUSH AX
POP AX
POP BX
CMP AX,BX
JGE 
MOV AX, 2
PUSH AX
PRINTN "Hola\n"
finFor0:
RET
DEFINE_SCAN_NUM
