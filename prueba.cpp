//Gabriel Morales Nuñez
#include <iostream>
#include <stdio.h>
#include <conio.h>
float area, radio, pi, resultado;
int a, d, altura;
float x;
char y;int i ;int j;
// Este programa calcula el volumen de un cilindro.
void main(){
    printf("Introduce la altura de la piramide: ");
    scanf("altura", &altura);
    for(i = 0; i < altura; i++)
    {
        printf(i);
        for(j = 0; j < i; j++)
        {
            printf(j);
            if(j!=1){
                printf("-");
            }
            else{
                printf("*");
            }
        }
        printf("\n");
    }
    a = 256;
    y = (char)(a);
    printf(y);
    printf("\nA continuacion se intenta asignar un int a un char sin usar casteo: \n");
    y = a;
}