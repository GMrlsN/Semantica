//Gabriel Morales Nuñez
using System;
using System.Collections.Generic;
//Requerimiento 1.- Actualizar dominante para variables en la expresion
//                  Ejemplo: float x; char y; y = x; eso deberia ser un error             --Ya jala
//Requerimiento 2.- Actualizar el dominante para el casteo y el valor de la subexpresion  --Ya jala
//Requerimiento 3.- Programar un metodo de conversion de un valor a un tipo de dato   --Ya jala
//                  private float convert(float valor, string tipoDato)
//                  deberan usar el residuo de la division %255, por 65535
//Requerimiento 4.- Evaluar nuevamente la condicion del if, while, for, do while con respecto
//                  al parametro que recibe, arreglar los else
//Requerimiento 5.- Levantar una excepcion cuando la captura no sea un numerico       --Ya  jala
//Requerimiento 6.- Ejecutar el for 
//                  
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
            else
            {
                throw new Error("Error de sintaxis, variable <" + getContenido()+"> no existe en linea: "+linea, log);
            }
        }

        //While -> while(Condicion) bloque de instrucciones | instruccion
        private void While(bool evaluacion)
        {
            match("while");
            match("(");
            //Requerimiento 4
            bool validarWhile = Condicion();
            match(")");
            if (getContenido() == "{") 
            {
                BloqueInstrucciones(evaluacion);
            }
            else
            {
                Instruccion(evaluacion);
            }
        }

        //Do -> do bloque de instrucciones | intruccion while(Condicion)
        private void Do(bool evaluacion)
        {
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
            //Requerimiento 4
            bool validarDo = Condicion();
            match(")");
            match(";");
        }
        //For -> for(Asignacion Condicion; Incremento) BloqueInstruccones | Intruccion 
        private void For(bool evaluacion)
        {
            match("for");
            match("(");
            Asignacion(evaluacion);
            //Requerimiento 4
            //Requerimiento 6:
            //a) necesito guardar la posicion del archivo de texto en una variable int
            bool validarFor = Condicion();
            //b) metemos un ciclo while
            //while()
            //  {
                match(";");
                Incremento(evaluacion);
                match(")");
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(evaluacion);  
                }
                else
                {
                    Instruccion(evaluacion);
                }
                //c) regresar a la posicion de lectura del archivo
                //d) sacar otro token
            //  }
        }

        //Incremento -> Identificador ++ | --
        private void Incremento(bool evaluacion)
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
                }
                else
                {
                    match("--");
                   if(evaluacion)
                    {
                        modificaValor(variable, getValor(variable) - 1);
                    }
                }
            }
            else
            {
                throw new Error("Error de sintaxis, variable <" +getContenido()+"> no existe en linea: "+linea, log);
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
                switch (operador)
                {
                    case "*":
                        stack.Push(n2 * n1);
                        break;
                    case "/":
                        stack.Push(n2 / n1);
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
                    //Console.WriteLine("Convertir a un " + tipo);
                    float valor = stack.Pop();
                    //Console.WriteLine("Valor: " + valor);
                    //Console.WriteLine("convert " + convert(valor,tipo));
                    stack.Push(convert(valor,tipo));
                    dominante = casteo;

                }
            }
        }
    }
}