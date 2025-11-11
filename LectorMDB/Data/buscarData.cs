using System;
using System.Collections.Generic;
using System.Text;

namespace LectorMDB.Data
{
    public class buscarData
    {
        public string errorLibro { get; private set; }
        public string errorInput { get; private set; }
        public string errorNoMatch { get; private set; }
        public string errorTitle { get; private set; }
        public string buscarLoading { get; private set; }

        public void setBuscarLoading(string value)
        {
            buscarLoading = value;
        }
        public void setErrorLibro(string value)
        {
            errorLibro = value;
        }
        public void setErrorInput(string value)
        {
            errorInput = value;
        }
        public void setErrorNoMatch(string value)
        {
            errorNoMatch = value;
        }
        public void setErrorTitle(string value)
        {
            errorTitle = value;
        }
    }
}
