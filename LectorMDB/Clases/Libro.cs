using System;
using System.Collections.Generic;
using System.Text;

namespace LectorMDB.Clases
{
    public class Libro
    {
        public string path { get; private set; }
        public int numeroHojaMaxima { get; private set; }

        public void setPath(string value)
        {
            path = value;
        }
        
    }
}
