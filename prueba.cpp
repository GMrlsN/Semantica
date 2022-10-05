//Gabriel Morales Nuñez
#include <iostream>
#include <stdio.h>
#include <conio.h>
float area, radio, pi, resultado;
int a, d;
float x;
char y;int ide ;int kj;
// Este programa calcula el volumen de un cilindro.
void main(){
    printf("If anidado\n");
    scanf("area", &area);
    for(ide = 0; ide < 10; ide++)
    {
        printf(ide);
        printf(" hola\n");
        for(kj = 0; kj < 10; kj++)
        {
            printf("\t");
            printf(kj);
            printf(" for anidado\n"); 
        }
    }
    a = 256;
    y = (char)(a);
    printf(y);
    printf("\nA continuacion se intenta asignar un int a un char sin usar casteo: \n");
    y = a;
}