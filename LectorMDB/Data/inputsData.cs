using System;
using System.Collections.Generic;
using System.Text;

namespace LectorMDB.Data
{
    public class inputsData
    {
        public string errorCambiarHoja { get; private set; }
        public string titleErrorCambiarHoja { get; private set; }
        public string titleInputNP { get; private set; }
        public string textInputNP { get; private set; }
        public string errorTitleInputNP { get; private set; }
        public string errorTextInputNP { get; private set; }
        public string errorNoLibro { get; private set; }

        public void setErrorNoLibro(string value)
        {
            errorNoLibro = value;
        }
        public void setTitleInputNP(string value)
        {
            titleInputNP = value;
        }
        public void setTextInputNP(string value)
        {
            textInputNP = value;
        }
        public void setErrorTitleInputNP(string value)
        {
            textInputNP = value;
        }
        public void setErrorTextInputNP(string value)
        {
            errorTextInputNP = value;
        }
        public void setErrorCambiarHoja(string value)
        {
            errorCambiarHoja = value;
        }
        public void setTitleErrorCambiarHoja(string value)
        {
            titleErrorCambiarHoja = value;
        }
    }
}
