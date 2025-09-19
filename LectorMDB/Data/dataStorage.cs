using System;
using System.Collections.Generic;
using System.Text;

namespace LectorMDB.Data
{
    public class dataStorage
    {
        private int defaultFont;
        private int defaultMaxHoja;
        private List<string> fontsNumbers;
        private int fontChangePlusAmount;
        private int fontChangeMinusAmount;
        private string defaultExt;
        private string filterExt;
        private string contentFile;
        private string titleError;
        private string errorFileFormat;
        private string errorOpening;
        private string queryMAXHoja;
        private string queryGetOneHoja;
        private string queryCampoHoja;
        private string readerMDBString;
        private string fieldHojaMAX;
        private List<string> tiposHojas;
        private List<string> orientaciones;
        private List<string> printConfig;
        private List<string> fontPrintSizes;
        private string titleNP;
        private string textNP;
        private string titleErrorNP;
        private string textErrorNP;
        private string titleErrorCambiarHoja;
        private string textErrorCambiarHoja;
        public dataStorage()
        {
            defaultFont = 12;
            defaultMaxHoja = -1;
            fontsNumbers = new List<string> { "8", "12", "24", "36", "72" };
            fontChangePlusAmount = 1;
            fontChangeMinusAmount = -1;
            defaultExt = ".mdb";
            filterExt = "Base Access| *.mdb";
            contentFile = "H.MDB";
            titleError = "Error";
            errorFileFormat = "Error Archivo no valido";
            errorOpening = "No se encontro: ";
            queryMAXHoja = "SELECT MAX(HojaNro) FROM Libro";
            queryGetOneHoja = "SELECT Hoja FROM Libro WHERE HojaNro = ?";
            queryCampoHoja = "Hoja";
            readerMDBString = @"Provider = Microsoft.Jet.OLEDB.4.0;Data Source=";
            tiposHojas = new List<string> { "Carta", "A4", "A5", "Ejecutivo", "Legal" };
            orientaciones = new List<string> { "Horizontal", "Vertical" };
            printConfig = new List<string> { "        Una Hoja         ", "Varias Hojas Consecutivas" };
            fontPrintSizes = new List<string> { "6", "7", "8", "9", "10", "11", "12" };
            fieldHojaMAX = "Expr1000";
            titleNP = "Número de página";
            textNP = "Escribir número de página:";
            titleErrorNP = "Error";
            textErrorNP = "Número de página no valido.";
            titleErrorCambiarHoja = "Error";
            textErrorCambiarHoja = "Número de hoja no esta en el rango de hojas";
        }
        public List<string> getFPS()
        {
            return fontPrintSizes;
        }
        public List<string> getPC()
        {
            return printConfig;
        }
        public List<string> getO()
        {
            return orientaciones;
        }
        public List<string> getTH()
        {
            return tiposHojas;
        }

        public string getReaderString()
        {
            return readerMDBString;
        }
        public querysData getQuerys()
        {
            var response = new querysData();
            response.setMaxHojaNumber(queryMAXHoja);
            response.setOneHoja(queryGetOneHoja);
            response.setFieldNameHojaMAX(fieldHojaMAX);
            response.setFieldHojaText(queryCampoHoja);
            return response;
        }
        public dialogueData getDialogue()
        {
            var response = new dialogueData();
            response.setDefaultExt(defaultExt);
            response.setFilterExt(filterExt);
            response.setErrorWindowTitle(titleError);
            response.setErrorWindowWrongFile(errorFileFormat);
            response.setErrorWindowNotContentFile(errorOpening);
            response.setFileOfHojas(contentFile);
            return response;
        }
        public fontData getFont()
        {
            var response = new fontData();
            response.setPlus(fontChangePlusAmount);
            response.setMinus(fontChangeMinusAmount);
            response.setCombo(fontsNumbers);
            return response;
        }
        public inputsData getInputs()
        {
            var response = new inputsData();
            response.setErrorTextInputNP(textErrorNP);
            response.setErrorTitleInputNP(titleErrorNP);
            response.setTextInputNP(textNP);
            response.setTitleInputNP(titleNP);
            response.setErrorCambiarHoja(textErrorCambiarHoja);
            response.setTitleErrorCambiarHoja(titleErrorCambiarHoja);

            return response;
        }
        public List<int> getBaseMBDInts()
        {
            var response = new List<int>();
            response.Add(defaultFont);
            response.Add(defaultMaxHoja);
            return response;
        }
        
    }
}
