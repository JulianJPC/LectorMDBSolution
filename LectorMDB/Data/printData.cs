using System;
using System.Collections.Generic;
using System.Text;

namespace LectorMDB.Data
{
    public class printData
    {
        public List<string> tiposHojas { get; private set; }
        public List<string> orientaciones { get; private set; }
        public List<string> fontSizes { get; private set; }
        public string textHojaTipo { get; private set; }
        public string textOrientacion { get; private set; }
        public string textFont { get; private set; }
        public string textInicialNumeroHoja { get; private set; }
        public string textFinalNumeroHoja { get; private set; }
        public string buttonPrint { get; private set; }
        public string buttonExit { get; private set; }
        public string title { get; private set; }
        public string errorInput { get; private set; }
        public string errorTitle { get; private set; }
        public string fontFamily { get; private set; }

        public void setFontFamily(string value)
        {
            fontFamily = value;
        }
        public void setErrorTitle(string value)
        {
            errorTitle = value;
        }
        public void setErrorInput(string value)
        {
            errorInput = value;
        }
        public void setTextHojaTipo(string value)
        {
            textHojaTipo = value;
        }
        public void setTextOrientacion(string value)
        {
            textOrientacion = value;
        }
        public void setTextFont(string value)
        {
            textFont = value;
        }
        public void setTextInicialNumeroHoja(string value)
        {
            textInicialNumeroHoja = value;
        }
        public void setTextFinalNumeroHoja(string value)
        {
            textFinalNumeroHoja = value;
        }
        public void setButtonPrint(string value)
        {
            buttonPrint = value;
        }
        public void setButtonExit(string value)
        {
            buttonExit = value;
        }
        public void setTitle(string value)
        {
            title = value;
        }
        public void setTiposHojas(List<string> value)
        {
            tiposHojas = value;
        }
        public void setOrientaciones(List<string> value)
        {
            orientaciones = value;
        }
        public void setFontSizes(List<string> value)
        {
            fontSizes = value;
        }
    }
}
