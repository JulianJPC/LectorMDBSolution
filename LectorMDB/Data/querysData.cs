using System;
using System.Collections.Generic;
using System.Text;

namespace LectorMDB.Data
{
    public class querysData
    {
        public string getMaxHojaNumber { get; private set; }
        public string getOneHoja { get; private set; }
        public string fieldNameHojaMAX { get; private set; }

        public void setfieldNameHojaMAX(string value)
        {
            fieldNameHojaMAX = value;
        }
        public void setMaxHojaNumber(string value)
        {
            getMaxHojaNumber = value;
        }
        public void setOneHoja(string value)
        {
            getOneHoja = value;
        }
    }
}
