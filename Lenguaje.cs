//Gabriel Morales Nuñez
using System;
using System.IO;
using System.Collections.Generic;
//Requerimiento 1.- Actualizacion:
//                  a) Agregar el residuo de la division en porfactor                               //Ya jala
//                  b) Agregar en asignacion los incrementos de termino y de factor                 //Ya jala
//                     a++; a--; a+=1; a-=1; a*=1; a/=1; a%=1;
//                     en donde el 1 puede ser una expresion
//                  c) Programar el destructor en la clase lexico                                   //Ya jala
//                     #libreria especial? contenido?
//                     cerrar el archivo sin ejecutar el a.close
//                     Programar el destructor para ejecutar el metodo cerrarAchivo
//                     en program se tiene que implementar otra cosa
//Requerimiento 2.- Actualizacion la Venganza:
//                  a) Marcar errores semanticos cuando los incrementos de termino o incrementos    //Ya jala
//                     superen el rango de la variable
//                  b) Considerar el inciso b) y c) para el for                                     //Ya jala
//                  c) Hacer funcionar el while y do while                                          //Ya jala
//Requerimiento 3.-
//                  a) considerar las variables y los casteos de las expresiones matematicas       //Ya jala?
//                     en ensamblador
//                  b) considerar el residuo de la division en ensamblador (el residuo se queda en dx) //Ya jala
//                  c) programar el printf y el scanf en ASM                                       //Ya jala
//Requerimiento 4.-
//                  a) Programar el else en ASM
//                  b) programar el for en ASM 
//Requerimiento 5.- 
//                  a) Programar el while en ASM
//                  b) Programar el do while en ASM
//
namespace Semantica
{
    public class Lenguaje : Sintaxis
    {
        List <Variable> variables = new List<Variable>();
        Stack<float> stack = new Stack<float>();

        Variable.TipoDato dominante;
        int cIf;
        int cFor;
        public Lenguaje()
        {
            cIf  = cFor = 0;
        }
        public Lenguaje(string nombre) : base(nombre)
        {
            cIf = cFor = 0;
            
        }
        private void addVariable(String nombre,Variable.TipoDato tipo)
        {
            variables.Add(new Variable(nombre, tipo));
        }

        private void displayVariables()
        {
            log.WriteLine("\n\nVariables: " );
            foreach (Variable v in variables)
            {
                log.WriteLine(v.getNombre()+", "+v.getTipo()+", "+v.getValor());
            }
        }
        private void variablesASM(){
            asm.WriteLine("\n\n;Variables: " );
            foreach (Variable v in variables)
            {
                asm.WriteLine("\t"+v.getNombre()+" DW ?");
            }
        }
        private bool existeVariable(string nombre)
        {
            foreach (Variable v in variables)
            {
                if (v.getNombre().Equals(nombre))
                {
                    return true;
                }
            }
            return false;
        }
        private void modificaValor(string nombre, float nuevoValor){
            foreach (Variable v in variables)
            {
                if (v.getNombre().Equals(nombre))
                {
                    v.setValor(nuevoValor);
                }
            }
        }
        private float getValor(string nombreVariable){
            foreach (Variable v in variables)
                {
                    if (v.getNombre().Equals(nombreVariable))
                    {
                        return v.getValor();
                    }
                }    
            return 0;
        }
        private Variable.TipoDato getTipo(string nombreVariable){
            foreach (Variable v in variables)
                {
                    if (v.getNombre().Equals(nombreVariable))
                    {
                        return v.getTipo();
                    }
                }    
            return Variable.TipoDato.Char;
        }
        //Programa  -> Librerias? Variables? Main
        public void Programa()
        {
            
            asm.WriteLine("#make_COM#");
            asm.WriteLine("include emu8086.inc");
            asm.WriteLine("ORG 100H");
            Libreria();
            Variables();
            variablesASM();
            Main();
            displayVariables();
            asm.WriteLine("RET");
            asm.WriteLine("DEFINE_SCAN_NUM");
            //asm.WriteLine("END");
        }

        //Librerias -> #include<identificador(.h)?> Librerias?
        private void Libreria()
        {
            if (getContenido() == "#")
            {
                match("#");
                match("include");
                match("<");
                match(Tipos.Identificador);
                if (getContenido() == ".")
                {
                    match(".");
                    match("h");
                }
                match(">");
                Libreria();
            }
        }

         //Variables -> tipo_dato Lista_identificadores; Variables?
        private void Variables()
        {
            if (getClasificacion() == Tipos.TipoDato)
            {
                Variable.TipoDato tipo = Variable.TipoDato.Char; 
                switch (getContenido())
                {
                    case "int": tipo = Variable.TipoDato.Int; break;
                    case "float": tipo = Variable.TipoDato.Float; break;
                }
                match(Tipos.TipoDato);
                Lista_identificadores(tipo);
                match(Tipos.FinSentencia);
                Variables();
            }
        }

         //Lista_identificadores -> identificador (,Lista_identificadores)?
        private void Lista_identificadores(Variable.TipoDato tipo)
        {
            if (getClasificacion() == Tipos.Identificador)
            {
                if (!existeVariable(getContenido()))
                {
                    addVariable(getContenido(), tipo);
                }
                else
                {
                    throw new Error("Error de sintaxis, variable duplicada <" +getContenido()+"> en linea: "+linea, log);
                }
            }
            
            match(Tipos.Identificador);
            if (getContenido() == ",")
            {
                match(",");
                Lista_identificadores(tipo);
            }
        }
//Main      -> void main() Bloque de instrucciones
        private void Main()
        {
            match("void");
            match("main");
            match("(");
            match(")");
            BloqueInstrucciones(true, true);
        }
        //Bloque de instrucciones -> {listaIntrucciones?}
        private void BloqueInstrucciones(bool evaluacion, bool evasm)
        {
            match("{");
            if (getContenido() != "}")
            {
                ListaInstrucciones(evaluacion, evasm);
            }    
            match("}"); 
        }

        //ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones(bool evaluacion, bool evasm)
        {
            Instruccion(evaluacion, evasm);
            if (getContenido() != "}")
            {
                ListaInstrucciones(evaluacion, evasm);
            }
        }

        //ListaInstruccionesCase -> Instruccion ListaInstruccionesCase?
        private void ListaInstruccionesCase(bool evaluacion, bool evasm)
        {
            Instruccion(evaluacion, evasm);
            if (getContenido() != "case" && getContenido() !=  "break" && getContenido() != "default" && getContenido() != "}")
            {
                ListaInstruccionesCase(evaluacion, evasm);
            }
        }

        //Instruccion -> Printf | Scanf | If | While | do while | For | Switch | Asignacion
        private void Instruccion(bool evaluacion, bool evasm) //True para que no marque error
        {
            if (getContenido() == "printf")
            {
                Printf(evaluacion, evasm);
            }
            else if (getContenido() == "scanf")
            {
                Scanf(evaluacion, evasm);
            }
            else if (getContenido() == "if")
            {
                If(evaluacion, evasm);
            }
            else if (getContenido() == "while")
            {
                While(evaluacion, evasm);
            }
            else if(getContenido() == "do")
            {
                Do(evaluacion, evasm);
            }
            else if(getContenido() == "for")
            {
                For(evaluacion, evasm);
            }
            else if(getContenido() == "switch")
            {
                Switch(evaluacion, evasm);
            }
            else
            {
                Asignacion(evaluacion, evasm);
            }
        }
        private Variable.TipoDato evaluaNumero(float resultado)
        {
            if(resultado%1 != 0)
            {
                return Variable.TipoDato.Float;
            }
            if(resultado <= 255)
            {
                return Variable.TipoDato.Char;
            }
            else if(resultado <= 65535)
            {
                return Variable.TipoDato.Int;
            }
            return Variable.TipoDato.Float;
        }
        private bool evaluaSemantica(string variable, float resultado)
        {
            Variable.TipoDato tipoDato = getTipo(variable);

            return false;
        }
        private float convert(float valor, string tipoDato){
            switch(tipoDato){
                        case "char":
                            return valor%256;
                        case "int": 
                            return valor%65535; 
                        case "float": 
                            return valor;

                    }
            return valor;
        }
        //Asignacion -> identificador = cadena | Expresion;
        private void Asignacion(bool evaluacion, bool evasm)
        {
            if(existeVariable(getContenido())){
                log.WriteLine();
                log.Write(getContenido()+" = ");
                string nombre = getContenido();
                match(Tipos.Identificador);
                dominante = Variable.TipoDato.Char;
                if(getClasificacion() == Tipos.IncrementoTermino || getClasificacion() == Tipos.IncrementoFactor)
                {   
                    //Requerimiento 1.b
                    //Console.WriteLine("Asignacion: "+getContenido());
                    string operador = getContenido();
                    switch(operador)
                    {
                        case "++":
                            Incremento(evaluacion, evasm, nombre);
                            break;
                        case "--":
                            Incremento(evaluacion, evasm, nombre);
                            break;
                        case "+=":
                            match(Tipos.IncrementoTermino);
                            Expresion();
                            float resultado = getValor(nombre)+stack.Pop();
                            if(evasm)
                            asm.WriteLine("POP AX");
                            if(dominante < evaluaNumero(resultado))
                            {
                                dominante = evaluaNumero(resultado);
                            }
                            if(dominante <= getTipo(nombre))
                            {
                                if(evaluacion){
                                    modificaValor(nombre, resultado);
                                }
                            }
                            else
                            {
                                throw new Error("Error de semantica: no podemos asignar un: <" + dominante + "> a un: <" + getTipo(nombre) + "> en la linea " + linea,log);
                            }
                            modificaValor(nombre, resultado);
                            break;
                        case "-=":
                            match(Tipos.IncrementoTermino);
                            Expresion();
                            modificaValor(nombre, getValor(nombre)-stack.Pop());
                            if(evasm)
                            asm.WriteLine("POP AX");
                            break;
                        case "*=":
                            match(Tipos.IncrementoFactor);
                            Expresion();
                            resultado = getValor(nombre)*stack.Pop();
                            if(dominante < evaluaNumero(resultado))
                            {
                                dominante = evaluaNumero(resultado);
                            }
                            if(dominante <= getTipo(nombre))
                            {
                                if(evaluacion){
                                    modificaValor(nombre, resultado);
                                }
                            }
                            else
                            {
                                throw new Error("Error de semantica: no podemos asignar un: <" + dominante + "> a un: <" + getTipo(nombre) + "> en la linea " + linea,log);
                            }
                            modificaValor(nombre, resultado);
                            break;
                        case "/=":
                            match(Tipos.IncrementoFactor);
                            Expresion();
                            modificaValor(nombre, getValor(nombre)/stack.Pop());
                            if(evasm)
                            asm.WriteLine("POP AX");
                            break;
                        case "%=":
                            match(Tipos.IncrementoFactor);
                            Expresion();
                            modificaValor(nombre, getValor(nombre)%stack.Pop());
                            if(evasm)
                            asm.WriteLine("POP AX");
                            break;
                    }
                    match(";");
                    //Requerimiento 1.c
                }
                else
                {
                    match(Tipos.Asignacion);
                    Expresion();
                    match(";");
                    float resultado = stack.Pop();
                    if(evasm)
                    asm.WriteLine("POP AX");
                    log.Write("= "+ resultado);
                    log.WriteLine();
                    //Console.WriteLine(resultado + " = " + dominante + " ");
                    if(dominante < evaluaNumero(resultado))
                    {
                        dominante = evaluaNumero(resultado);
                    }
                    if(dominante <= getTipo(nombre))
                    {
                        if(evaluacion){
                            modificaValor(nombre, resultado);
                        }
                    }
                    else
                    {
                        throw new Error("Error de semantica: no podemos asignar un: <" + dominante + "> a un: <" + getTipo(nombre) + "> en la linea " + linea,log);
                    }
                    if(evasm)
                    asm.WriteLine("MOV " + nombre + ", AX");
                }
            }
            else
            {
                throw new Error("Error de sintaxis, variable <" + getContenido()+"> no existe en linea: "+linea, log);
            }
        }

        //While -> while(Condicion) bloque de instrucciones | instruccion
        private void While(bool evaluacion, bool evasm)
        {
            string etiquetaInicioFor = "inicioWhile" + cFor;
            string etiquetaFinalFor = "finWhile" + cFor++;
            bool validarWhile;
            //bool inc;
            //Hacer que funcione
            match("while");
            match("(");
            long contador = getContador();
            int linea = getLinea();
            do{
            validarWhile = Condicion("", evasm);
            if(!evaluacion)
            {
                validarWhile = false;
            }
            match(")");
            if (getContenido() == "{") 
            {
                BloqueInstrucciones(validarWhile, evasm);
            }
            else
            {
                Instruccion(!validarWhile, evasm);
            }
            if(validarWhile)
                {
                archivo.DiscardBufferedData();
                archivo.BaseStream.Seek(contador, SeekOrigin.Begin);
                NextToken();
                setContador(contador);
                setLinea(linea);
                //d) sacar otro token
                }
            }while (validarWhile);
        }

        //Do -> do bloque de instrucciones | intruccion while(Condicion)
        private void Do(bool evaluacion, bool evasm)
        {
            
            bool validarDo;
            match("do");
            long contador = getContador(); 
            int linea = getLinea();  
            do{
            if (getContenido() == "{")
            {
                BloqueInstrucciones(evaluacion, evasm);
            }
            else
            {
                Instruccion(evaluacion, evasm);
            } 
            match("while");
            match("(");
            validarDo = Condicion("", evasm);
            if(!evaluacion)
            {
                validarDo = false;
            }
            match(")");
            match(";");
            if(validarDo)
                {
                archivo.DiscardBufferedData();
                archivo.BaseStream.Seek(contador, SeekOrigin.Begin);
                NextToken();
                setContador(contador);
                setLinea(linea);
            }
            }while(validarDo);
        }
        //For -> for(Asignacion Condicion; Incremento) BloqueInstruccones | Intruccion 
        private void For(bool evaluacion, bool evasm)
        {
            string etiquetaInicioFor = "inicioFor" + cFor;
            string etiquetaFinalFor = "finFor" + cFor++;
            if(evasm)
            asm.WriteLine(etiquetaInicioFor +":");
            match("for");
            match("(");
            bool validarFor;
            string inc;
            String varInc;
            Asignacion(evaluacion, evasm);
            long contador = getContador(); 
            int linea = getLinea();  
            int tam = getContenido().Length - 1; 
            //b) metemos un ciclo while
            do
            {
                validarFor = Condicion("",evasm);
                match(";");
                if(!evaluacion)
                {
                    validarFor = false;
                }
                varInc = getContenido();
                inc = IncrementoFor(false, false);
                match(")");
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(validarFor, evasm);  
                }
                else
                {
                    Instruccion(validarFor, evasm);
                }
                if(inc == "++")
                {
                    modificaValor(varInc, getValor(varInc) + 1);
                }
                else if (inc == "--")
                {
                    modificaValor(varInc, getValor(varInc) - 1);
                }
                else if (inc == "-=")
                {
                    modificaValor(varInc, getValor(varInc) - stack.Pop());
                    //if(evasm)
                    //asm.WriteLine("POP AX");
                }
                else if (inc == "+=")
                {
                    modificaValor(varInc, getValor(varInc) + stack.Pop());
                    //asm.WriteLine("POP AX");
                }
                else if (inc == "*=")
                {
                    modificaValor(varInc, getValor(varInc) * stack.Pop());
                    //asm.WriteLine("POP AX");
                }
                else if (inc == "/=")
                {
                    modificaValor(varInc, getValor(varInc) / stack.Pop());
                    //asm.WriteLine("POP AX");
                }
                else if (inc == "%=")
                {
                    modificaValor(varInc, getValor(varInc) % stack.Pop());
                    //asm.WriteLine("POP AX");
                }
                else
                {
                    //throw new Error("Error de sintaxis, no se reconoce el incremento <" + inc + "> en linea: " + linea, log);
                }
                //c) regresar a la posicion de lectura del archivo
                if(validarFor)
                {
                archivo.DiscardBufferedData();
                archivo.BaseStream.Seek(contador-tam, SeekOrigin.Begin);
                NextToken();
                setContador(contador);
                setLinea(linea);
                //d) sacar otro token
                }
            }while(validarFor);
            if(evasm)
            asm.WriteLine(etiquetaFinalFor +":");
        }

        //Incremento -> Identificador ++ | --
        private string IncrementoFor(bool evaluacion, bool evasm)
        {
            string variable = getContenido();
            if(existeVariable(variable)){
                match(Tipos.Identificador);
                string OPfOR = getContenido();
                if( getContenido() == "++")
                {
                    match("++");
                    if(evaluacion)
                    {
                        modificaValor(variable, getValor(variable) + 1);
                    }
                    return OPfOR;
                }
                else if( getContenido() == "--")
                {
                    match("--");
                    if(evaluacion)
                    {
                        modificaValor(variable, getValor(variable) - 1);
                    }
                    return OPfOR;
                }
                else if(getContenido() == "+=")
                {
                    match("+=");
                    Expresion();
                    if(evaluacion)
                    {
                        modificaValor(variable, getValor(variable) + stack.Pop());
                        if(evasm)
                        asm.WriteLine("POP AX");
                    }
                    return OPfOR;
                }
                else if(getContenido() == "-=")
                {
                    match("-=");
                    Expresion();
                    if(evaluacion)
                    {
                        modificaValor(variable, getValor(variable) - stack.Pop());
                        if(evasm)
                        asm.WriteLine("POP AX");
                    }
                    return OPfOR;
                }
                else{
 return OPfOR;
                }
            }
            else
            {
                throw new Error("Error de sintaxis, variable <" +getContenido()+"> no existe en linea: "+linea, log);
            }
            
        }
        //Incremento normal
        private bool Incremento(bool evaluacion, bool evasm, string variable)
        {
                
                //match(Tipos.IncrementoTermino);
                //Consol
                if( getContenido() == "++")
                {
                    match("++");
                    if(evaluacion)
                    {
                        float resultado = getValor(variable) + 1;
                            if(dominante < evaluaNumero(resultado))
                            {
                                dominante = evaluaNumero(resultado);
                            }
                            if(dominante <= getTipo(variable))
                            {
                                if(evaluacion){
                                    modificaValor(variable, resultado);
                                }
                            }
                            else
                            {
                                throw new Error("Error de semantica: no podemos asignar un: <" + dominante + "> a un: <" + getTipo(variable) + "> en la linea " + linea,log);
                            }
                    }
                    return true;
                }
                else
                {
                    match("--");
                    if(evaluacion)
                    {
                        modificaValor(variable, getValor(variable) - 1);
                    }
                    return false;
                }
        }
        //Switch -> switch (Expresion) {Lista de casos} | (default: )
        private void Switch(bool evaluacion, bool evasm)
        {
            match("switch");
            match("(");
            Expresion();
            stack.Pop();
            if(evasm)
            asm.WriteLine("POP AX");
            match(")");
            match("{");
            ListaDeCasos(evaluacion, evasm);
            if(getContenido() == "default")
            {
                match("default");
                match(":");
                if (getContenido() == "{")
                {
                    if(evaluacion)
                    BloqueInstrucciones(evaluacion, evasm);  
                }
                else
                {
                    If(evaluacion, evasm);
                    Instruccion(evaluacion, evasm);
                }
            }
            match("}");
        }

        //ListaDeCasos -> case Expresion: listaInstruccionesCase (break;)? (ListaDeCasos)?
        private void ListaDeCasos(bool evaluacion, bool evasm)
        {
            match("case");
            Expresion();
            stack.Pop();
            if(evasm)
            asm.WriteLine("POP AX");
            match(":");
            ListaInstruccionesCase(evaluacion, evasm);
            if(getContenido() == "break")
            {
                match("break");
                match(";");
            }
            if(getContenido() == "case")
            {
                ListaDeCasos(evaluacion, evasm);
            }
        }

        //Condicion -> Expresion operador relacional Expresion
        private bool Condicion(string etiqueta, bool evasm)
        {
            Expresion();
            String operador = getContenido();
            match(Tipos.OperadorRelacional);
            Expresion();
            float e2 = stack.Pop();
            if(evasm)
            asm.WriteLine("POP AX");
            float e1 = stack.Pop();
            if(evasm){
            asm.WriteLine("POP BX");
            asm.WriteLine("CMP AX,BX");
            }
            switch (operador){
                case "==":
                    asm.WriteLine("JNE " + etiqueta);
                    return e1 == e2;
                case ">":
                    asm.WriteLine("JLE " + etiqueta);
                    return e1 > e2;
                case "<":
                    asm.WriteLine("JGE " + etiqueta);
                    return e1 < e2;
                case ">=":
                    asm.WriteLine("JL " + etiqueta);
                    return e1 >= e2;
                case "<=":
                    asm.WriteLine("JG " + etiqueta);
                    return e1 <= e2;
                default:
                    asm.WriteLine("JE " + etiqueta);
                    return e1 != e2;
            }
        }

        //If -> if(Condicion) bloque de instrucciones (else bloque de instrucciones)?
        private void If(bool evaluacion, bool evasm)
        {
            string etiquetaIf = "if" + ++cIf;
            match("if");
            match("(");
            //Requerimiento 4
            bool validarIf = Condicion(etiquetaIf, evasm); 
            if(!evaluacion)
            {
                validarIf = false;
            }
            match(")");
            if (getContenido() == "{")
            {
                BloqueInstrucciones(validarIf, evasm);  
            }
            else
            {
                Instruccion(validarIf, evasm);
            }
            if (getContenido() == "else")
            {
                match("else");
                if(!evaluacion)
                {
                    validarIf = true;
                }
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(!validarIf, evasm);
                }
                else
                {
                    Instruccion(!validarIf, evasm);
                }
            }
            cIf++;
            asm.WriteLine(etiquetaIf + ":");
        }

        //Printf -> printf(cadena o expresion); 
        private void Printf(bool evaluacion, bool evasm)
        {
            match("printf");
            match("(");
            if(getClasificacion() == Tipos.Cadena)
            {
                if(evaluacion)
                {
                string cadena = getContenido().Substring(1,getContenido().Length - 2);
                char [] cad = new char[cadena.Length];
                for (int i = 0; i < cadena.Length; i++){
                    if (cadena[i] == '\\')
                    {
                        switch (cadena[i+1]){
                            case 'a': cad[i] = '\a'; i++; break;
                            case 'b': cad[i] = '\b'; i++; break;
                            case 'f': cad[i] = '\f'; i++; break;
                            case 'n': cad[i] = '\n'; i++; break;
                            case 'r': cad[i] = '\r'; i++; break;
                            case 't': cad[i] = '\t'; i++; break;
                            case 'v': cad[i] = '\v'; i++; break;
                            case '\\': cad[i] = '\\'; i++; break;
                            case '\'': cad[i] = '\''; i++; break;
                            case '\"': cad[i] = '\"'; i++; break;
                        }
                    }
                    else{
                        cad[i] = cadena[i];
                    }
                }
                cadena = new string(cad);
                Console.Write(cadena);
                }
                asm.WriteLine("PRINTN " + getContenido());
                match(Tipos.Cadena);
            }
            else{
                //stack.Pop();
                Expresion();
                if(evaluacion)
                {
                    Console.Write(stack.Pop());
                    asm.WriteLine("POP AX");
                    //Requerimiento printF codigo ensamblador para imprimir una variable
                }
            }
            match(")");
            match(";");
        }

        //Scanf -> scanf(cadena, &identificador);
        private void Scanf(bool evaluacion, bool evasm)    
        {
            match("scanf");
            match("(");
            match(Tipos.Cadena);
            match(",");
            match("&");
            if(existeVariable(getContenido())){
                string nombreVariable = getContenido();
                match(Tipos.Identificador);
                if(evaluacion){
                string val = "" + Console.ReadLine();
                for(int i = 0; i < val.Length; i++){
                    if(!Char.IsDigit(val[i])){
                      throw new Error("Error de sintaxis, se introdujo un caracter no numerico al scanf en linea: "+linea, log);  
                    }
                }
                float num;
                bool esNum = float.TryParse(val, out num);
                float valorFloat = float.Parse(val);
                modificaValor(nombreVariable,valorFloat);
                }
                match(")");
                match(";");
                asm.WriteLine("CALL SCAN_NUM");
                asm.WriteLine("MOV " + nombreVariable + ", CX");
            }
            else
            {
                throw new Error("Error de sintaxis, variable <" +getContenido()+"> no existe en linea: "+linea, log);
            }
        }

        

        //Expresion -> Termino MasTermino
        private void Expresion()
        {
            Termino();
            MasTermino();
        }
        //MasTermino -> (OperadorTermino Termino)?
        private void MasTermino()
        {
            if (getClasificacion() == Tipos.OperadorTermino)
            {
                string operador = getContenido();
                match(Tipos.OperadorTermino);
                Termino();
                log.Write(operador + " ");
                float n1 = stack.Pop();
                asm.WriteLine("POP BX");
                float n2 = stack.Pop();
                asm.WriteLine("POP AX");
                switch (operador)
                {
                    case "+":
                        stack.Push(n2 + n1);
                        asm.WriteLine("ADD AX, BX");
                        asm.WriteLine("PUSH AX");
                        break;
                    case "-":
                        stack.Push(n2 - n1);
                        asm.WriteLine("SUB AX, BX");
                        asm.WriteLine("PUSH AX");
                        break;
                }
            }
        }
        //Termino -> Factor PorFactor
        private void Termino()
        {
            Factor();
            PorFactor();
        }
        //PorFactor -> (OperadorFactor Factor)? 
        private void PorFactor()
        {
            if (getClasificacion() == Tipos.OperadorFactor)
            {
                string operador = getContenido();
                match(Tipos.OperadorFactor);
                Factor();
                log.Write(operador + " ");
                float n1 = stack.Pop();
                asm.WriteLine("POP BX");
                float n2 = stack.Pop();
                asm.WriteLine("POP AX");
                //Requerimiento 1.a)
                switch (operador)
                {
                    case "*":
                        stack.Push(n2 * n1);
                        asm.WriteLine("MUL BX");
                        asm.WriteLine("PUSH AX");
                        break;
                    case "/":
                        stack.Push(n2 / n1);
                        asm.WriteLine("DIV BX");
                        asm.WriteLine("PUSH AX");
                        break;
                    case "%":
                        stack.Push(n2 % n1);
                        asm.WriteLine("DIV BX");
                        asm.WriteLine("PUSH DX");
                        break;
                }
            }
        }
        //Factor -> numero | identificador | (Expresion)
        private void Factor()
        {
            if (getClasificacion() == Tipos.Numero)
            {
                log.Write(getContenido() + " " );
                if(dominante < evaluaNumero(float.Parse(getContenido())))
                {
                    dominante = evaluaNumero(float.Parse(getContenido()));
                }
                stack.Push(float.Parse(getContenido()));
                asm.WriteLine("MOV AX, " + float.Parse(getContenido()));
                asm.WriteLine("PUSH AX");
                match(Tipos.Numero);
            }
            else if (getClasificacion() == Tipos.Identificador)
            {
                if(existeVariable(getContenido()))
                {
                    log.Write(getContenido() + " " );
                    //Requerimiento 1.- 
                    dominante = getTipo(getContenido());
                    stack.Push(getValor(getContenido()));
                    match(Tipos.Identificador);
                }
                else
                {
                    throw new Error("Error de sintaxis, variable <" +getContenido()+"> no existe en linea: "+linea, log);
                }
            }
            else
            {
                string tipo = "";
                bool huboCasteo = false;
                Variable.TipoDato casteo = Variable.TipoDato.Char;
                match("(");
                if(getClasificacion() == Tipos.TipoDato)
                {
                    huboCasteo = true;
                    tipo = getContenido();
                    switch(tipo){
                        case "char":
                            casteo = Variable.TipoDato.Char;
                            break;
                        case "int": 
                            casteo = Variable.TipoDato.Int; 
                            break;
                        case "float": 
                            casteo = Variable.TipoDato.Float; 
                            break;

                    }
                    //casteo = getTipo(getContenido());
                    match(Tipos.TipoDato);
                    match(")");
                    match("(");
                }
                Expresion();
                match(")");
                if(huboCasteo)
                {
                    //Requerimiento 2.- Actualizar el dominante al de casteo y 
                    //Saco un elemento del stack
                    //convierto ese valor al equivalente en casteo
                    //Requerimiento 3.- 
                    //Ejemplo: si el casteo es (char) y el pop regresa un 256
                    //         el valor equivalente en casteo es 0
                    float valor = stack.Pop();
                    asm.WriteLine("POP AX");
                    stack.Push(convert(valor,tipo));
                    dominante = casteo;

                }
            }
        }
        ~Lenguaje(){
            Console.WriteLine("Destuctor");
            cerrar();
        }
    }
    
}