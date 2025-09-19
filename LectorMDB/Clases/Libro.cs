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
        public void setHojaMax(int value)
        {
            numeroHojaMaxima = value;
        }
        public bool isValidNumberHoja(int numberHoja)
        {
            var response = false;
            if (numberHoja >= 1 & numberHoja <= numeroHojaMaxima)
            {
                response = true;
            }
            return response;
        }
    }
}
