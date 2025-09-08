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
            queryGetOneHoja = "SELECT Hoja FROM Libro WHERE HojaNro = ";
            queryCampoHoja = "Hoja";
            readerMDBString = @"Provider = Microsoft.Jet.OLEDB.4.0;Data Source=";
        }
        public string getErrorOpening()
        {
            return errorOpening;
        }
        public string getErrorFileFormat()
        {
            return errorFileFormat;
        }
        public string getTitleError()
        {
            return titleError;
        }
        public string getContentFile()
        {
            return contentFile;
        }
        public string getFilterExt()
        {
            return filterExt;
        }
        public string getDefExt()
        {
            return defaultExt;
        }
        public int getChangePlus()
        {
            return fontChangePlusAmount;
        }
        public int getChangeMinus()
        {
            return fontChangeMinusAmount;
        }
        public List<string> getFonts()
        {
            return fontsNumbers;
        }
        public List<int> getBaseMBDInts()
        {
            var response = new List<int>();
            response.Add(defaultFont);
            response.Add(defaultMaxHoja);
            return response;
        }
        public List<string> getBaseMBDStrings()
        {
            var response = new List<string>();
            response.Add(queryMAXHoja);
            response.Add(queryGetOneHoja);
            response.Add(queryCampoHoja);
            response.Add(readerMDBString);
            return response;
        }
    }
}
