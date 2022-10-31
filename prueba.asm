;Archivo: prueba.asm
;fecha: 10/31/2022 3:54:56 PM
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
MOV AX, 10
PUSH AX
POP AX
POP BX
CMP AX,BX
JLE 
PRINTN "Hola\n"
MOV AX, 2
PUSH AX
POP AX
RET
DEFINE_SCAN_NUM
