//Gabriel Morales Nuñez
using System;
using System.IO;
using System.Collections.Generic;
//Requerimiento 1.- Actualizacion:
//                  a) Agregar el residuo de la division en porfactor                               //Ya jala
//                  b) Agregar en asignacion los incrementos de termino y de factor 
//                     a++; a--; a+=1; a-=1; a*=1; a/=1; a%=1;
//                     en donde el 1 puede ser una expresion
//                  c) Programar el destructor en la clase lexico
//                     #libreria especial? contenido?
//                     cerrar el archivo sin ejecutar el a.close
//                     Programar el destructor para ejecutar el metodo cerrarAchivo
//                     en program se tiene que implementar otra cosa
//Requerimiento 2.- Actualizacion la Venganza:
//                  a) Marcar errores semanticos cuando los incrementos de termino o incrementos
//                     superen el rango de la variable
//                  b) Considerar el inciso b) y c) para el for
//                  c) Hacer funcionar el while y do while
//Requerimiento 3.- Actualizacion
namespace Semantica
{
    public class Lenguaje : Sintaxis
    {
        List <Variable> variables = new List<Variable>();
        Stack<float> stack = new Stack<float>();

        Variable.TipoDato dominante;
        public Lenguaje()
        {

        }
        public Lenguaje(string nombre) : base(nombre)
        {

        }

        ~Lenguaje(){
            Console.WriteLine("Destuctor");
            cerrar();
        }
        private void addVariable(String nombre,Variable.TipoDato tipo)
        {
            variables.Add(new Variable(nombre, tipo));
        }

        private void displayVariables()
        {
            log.WriteLine("");
            log.WriteLine("Variables: " );
            foreach (Variable v in variables)
            {
                log.WriteLine(v.getNombre()+" "+v.getTipo()+" "+v.getValor());
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
            Libreria();
            Variables();
            Main();
            displayVariables();
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
            BloqueInstrucciones(true);
        }
        //Bloque de instrucciones -> {listaIntrucciones?}
        private void BloqueInstrucciones(bool evaluacion)
        {
            match("{");
            if (getContenido() != "}")
            {
                ListaInstrucciones(evaluacion);
            }    
            match("}"); 
        }

        //ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones(bool evaluacion)
        {
            Instruccion(evaluacion);
            if (getContenido() != "}")
            {
                ListaInstrucciones(evaluacion);
            }
        }

        //ListaInstruccionesCase -> Instruccion ListaInstruccionesCase?
        private void ListaInstruccionesCase(bool evaluacion)
        {
            Instruccion(evaluacion);
            if (getContenido() != "case" && getContenido() !=  "break" && getContenido() != "default" && getContenido() != "}")
            {
                ListaInstruccionesCase(evaluacion);
            }
        }

        //Instruccion -> Printf | Scanf | If | While | do while | For | Switch | Asignacion
        private void Instruccion(bool evaluacion) //True para que no marque error
        {
            if (getContenido() == "printf")
            {
                Printf(evaluacion);
            }
            else if (getContenido() == "scanf")
            {
                Scanf(evaluacion);
            }
            else if (getContenido() == "if")
            {
                If(evaluacion);
            }
            else if (getContenido() == "while")
            {
                While(evaluacion);
            }
            else if(getContenido() == "do")
            {
                Do(evaluacion);
            }
            else if(getContenido() == "for")
            {
                For(evaluacion);
            }
            else if(getContenido() == "switch")
            {
                Switch(evaluacion);
            }
            else
            {
                Asignacion(evaluacion);
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
        private void Asignacion(bool evaluacion)
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
                            Incremento(evaluacion, nombre);
                            break;
                        case "--":
                            Incremento(evaluacion, nombre);
                            break;
                        case "+=":
                            match(Tipos.IncrementoTermino);
                            Expresion();
                            float resultado = getValor(nombre)+stack.Pop();
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
                            break;
                        case "%=":
                            match(Tipos.IncrementoFactor);
                            Expresion();
                            modificaValor(nombre, getValor(nombre)%stack.Pop());
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
                }
            }
            else
            {
                throw new Error("Error de sintaxis, variable <" + getContenido()+"> no existe en linea: "+linea, log);
            }
        }

        //While -> while(Condicion) bloque de instrucciones | instruccion
        private void While(bool evaluacion)
        {
            bool validarFor;
            bool inc;
            //Hacer que funcione
            match("while");
            match("(");
            bool validarWhile = Condicion();
            if(!evaluacion)
            {
                validarWhile = false;
            }
            match(")");
            if (getContenido() == "{") 
            {
                BloqueInstrucciones(validarWhile);
            }
            else
            {
                Instruccion(!validarWhile);
            }
        }

        //Do -> do bloque de instrucciones | intruccion while(Condicion)
        private void Do(bool evaluacion)
        {
            bool validarFor;
            bool inc;
            
            match("do");
            if (getContenido() == "{")
            {
                BloqueInstrucciones(evaluacion);
            }
            else
            {
                Instruccion(evaluacion);
            } 
            match("while");
            match("(");
            bool validarDo = Condicion();
            if(!evaluacion)
            {
                validarDo = false;
            }
            match(")");
            match(";");
        }
        //For -> for(Asignacion Condicion; Incremento) BloqueInstruccones | Intruccion 
        private void For(bool evaluacion)
        {
            match("for");
            match("(");
            bool validarFor;
            bool inc;
            String varInc;
            Asignacion(evaluacion);
            //a) necesito guardar la posicion del archivo de texto en una variable int
            long contador = getContador(); 
            //Console.WriteLine("Se guarda: " + contador);
            int linea = getLinea();  
            //Console.WriteLine("peek " + (char)archivo.Peek());
            int tam = getContenido().Length - 1; 
            //b) metemos un ciclo while
            do
            {
                validarFor = Condicion();
                match(";");
                if(!evaluacion)
                {
                    validarFor = false;
                }
                varInc = getContenido();
                inc = IncrementoFor(false);
                match(")");
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(validarFor);  
                }
                else
                {
                    Instruccion(validarFor);
                }
                if(inc)
                {
                    modificaValor(varInc, getValor(varInc) + 1);
                }
                else
                {
                    modificaValor(varInc, getValor(varInc) - 1);
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
        }

        //Incremento -> Identificador ++ | --
        private bool IncrementoFor(bool evaluacion)
        {
            string variable = getContenido();
            if(existeVariable(variable)){
                match(Tipos.Identificador);
                if( getContenido() == "++")
                {
                    match("++");
                    if(evaluacion)
                    {
                        modificaValor(variable, getValor(variable) + 1);
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
            else
            {
                throw new Error("Error de sintaxis, variable <" +getContenido()+"> no existe en linea: "+linea, log);
            }
            
        }
        //Incremento normal
        private bool Incremento(bool evaluacion, string variable)
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
        private void Switch(bool evaluacion)
        {
            match("switch");
            match("(");
            Expresion();
            stack.Pop();
            match(")");
            match("{");
            ListaDeCasos(evaluacion);
            if(getContenido() == "default")
            {
                match("default");
                match(":");
                if (getContenido() == "{")
                {
                    if(evaluacion)
                    BloqueInstrucciones(evaluacion);  
                }
                else
                {
                    If(evaluacion);
                    Instruccion(evaluacion);
                }
            }
            match("}");
        }

        //ListaDeCasos -> case Expresion: listaInstruccionesCase (break;)? (ListaDeCasos)?
        private void ListaDeCasos(bool evaluacion)
        {
            match("case");
            Expresion();
            stack.Pop();
            match(":");
            ListaInstruccionesCase(evaluacion);
            if(getContenido() == "break")
            {
                match("break");
                match(";");
            }
            if(getContenido() == "case")
            {
                ListaDeCasos(evaluacion);
            }
        }

        //Condicion -> Expresion operador relacional Expresion
        private bool Condicion()
        {
            Expresion();
            String operador = getContenido();
            match(Tipos.OperadorRelacional);
            Expresion();
            float e2 = stack.Pop();
            float e1 = stack.Pop();
            switch (operador){
                case "==":
                    return e1 == e2;
                case ">":
                    return e1 > e2;
                case "<":
                    return e1 < e2;
                case ">=":
                    return e1 >= e2;
                case "<=":
                    return e1 <= e2;
                default:
                    return e1 != e2;
            }
        }

        //If -> if(Condicion) bloque de instrucciones (else bloque de instrucciones)?
        private void If(bool evaluacion)
        {
            match("if");
            match("(");
            //Requerimiento 4
            bool validarIf = Condicion();
            if(!evaluacion)
            {
                validarIf = false;
            }
            match(")");
            if (getContenido() == "{")
            {
                BloqueInstrucciones(validarIf);  
            }
            else
            {
                Instruccion(validarIf);
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
                    BloqueInstrucciones(!validarIf);
                }
                else
                {
                    Instruccion(!validarIf);
                }
            }
        }

        //Printf -> printf(cadena o expresion); 
        private void Printf(bool evaluacion)
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
                match(Tipos.Cadena);
            }
            else{
                //stack.Pop();
                Expresion();
                if(evaluacion)
                {
                    Console.Write(stack.Pop());
                }
            }
            match(")");
            match(";");
        }

        //Scanf -> scanf(cadena, &identificador);
        private void Scanf(bool evaluacion)    
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
                //Requerimiento 5    
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
                float n2 = stack.Pop();
                switch (operador)
                {
                    case "+":
                        stack.Push(n2 + n1);
                        break;
                    case "-":
                        stack.Push(n2 - n1);
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
                float n2 = stack.Pop();
                //Requerimiento 1.a)
                switch (operador)
                {
                    case "*":
                        stack.Push(n2 * n1);
                        break;
                    case "/":
                        stack.Push(n2 / n1);
                        break;
                    case "%":
                    stack.Push(n2 % n1);
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
                    stack.Push(convert(valor,tipo));
                    dominante = casteo;

                }
            }
        }
    }
}