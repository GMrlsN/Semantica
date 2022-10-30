//Gabriel Morales Nuñez
using System;

namespace Semantica
{
    public class Token : IDisposable
    {
        private string Contenido = "";
        private Tipos Clasificacion;
        private bool disposedValue;

        public enum Tipos
        {
            Identificador,Numero,Caracter,Asignacion,Inicializacion,
            OperadorLogico,OperadorRelacional,OperadorTernario,
            OperadorTermino,OperadorFactor,IncrementoTermino,IncrementoFactor,
            FinSentencia,Cadena,TipoDato,Zona,Condicion,Ciclo
        }

        public void setContenido(string contenido)
        {
            this.Contenido = contenido;
        }

        public void setClasificacion(Tipos clasificacion)
        {
            this.Clasificacion = clasificacion;
        }

        public string getContenido()
        {
            return this.Contenido;
        }

        public Tipos getClasificacion()
        {
            return this.Clasificacion;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
         ~Token()
         {
            Console.WriteLine("Finalizador");
             // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
             Dispose(disposing: false);
         }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Console.WriteLine("Se ha liberado la memoria");
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}