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
        private string errorOpening;
        private string queryMAXHoja;
        private string queryGetOneHoja;
        private string queryCampoHoja;
        private string queryGetAllHojasRange;
        private string readerMDBString;
        private string fieldHojaMAX;
        private List<string> tiposHojas;
        private List<string> orientaciones;
        private List<string> fontPrintSizes;
        private string titleNP;
        private string textNP;
        private string titleErrorNP;
        private string textErrorNP;
        private string titleErrorCambiarHoja;
        private string textErrorCambiarHoja;
        private string searchQuery;
        private string oneParam;
        private string buscarErrorInput;
        private string buscarErrorNoMatch;
        private string buscarErrorTitle;
        private string fontToPrint;
        private string fieldHojaNro;
        private string inputPrintTextHojaTipo;
        private string inputPrintTextOrientacion;
        private string inputPrintTextFont;
        private string inputPrintTextInicialNumeroHoja;
        private string inputPrintTextFinalNumeroHoja;
        private string inputPrintButtonPrint;
        private string inputPrintButtonExit;
        private string inputPrintTitle;

        private string printErrorInput;
        private string printErrorTitle;
        private string desbordamientoError;
        private string desbordamientoErrorTitulo;
        private string printTitleDocument;
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
            errorOpening = "No se encontro: ";
            queryMAXHoja = "SELECT MAX(HojaNro) FROM Libro";
            queryGetOneHoja = "SELECT Hoja FROM Libro WHERE HojaNro = ?";
            queryCampoHoja = "Hoja";
            readerMDBString = @"Provider = Microsoft.Jet.OLEDB.4.0;Data Source=";
            tiposHojas = new List<string> { "Carta", "A4", "A5", "Ejecutivo", "Legal" };
            orientaciones = new List<string> { "Horizontal", "Vertical" };
            fontPrintSizes = new List<string> { "6", "7", "8", "9", "10", "11", "12" };
            fieldHojaMAX = "Expr1000";
            titleNP = "Número de página";
            textNP = "Escribir número de página:";
            titleErrorNP = "Error";
            textErrorNP = "Número de página no valido.";
            titleErrorCambiarHoja = "Error";
            textErrorCambiarHoja = "Número de hoja no esta en el rango de hojas";
            searchQuery = "SELECT HojaNro FROM Libro WHERE Hoja LIKE ?";
            queryGetAllHojasRange = "SELECT Hoja FROM Libro WHERE HojaNro BETWEEN ? AND ?";
            oneParam = "?";
            buscarErrorInput = "No hay texto a buscar.";
            buscarErrorNoMatch = "No se encontro coincidencia.";
            buscarErrorTitle = "Error";
            fontToPrint = "Courier New";
            fieldHojaNro = "HojaNro";

            inputPrintTextHojaTipo = "Tamaño:";
            inputPrintTextOrientacion = "Orientación:";
            inputPrintTextFont = "Tamaño Letra:";
            inputPrintTextInicialNumeroHoja = "Número Hoja Inicial:";
            inputPrintTextFinalNumeroHoja = "Número Hoja Final:";
            inputPrintButtonPrint = "Imprimir";
            inputPrintButtonExit = "Cancelar";
            inputPrintTitle = "Imprimir";
            printErrorInput = "Error con los parametros introducidos.";
            printErrorTitle = "Error";
            desbordamientoError = "Desbordamiento. Se van imprimir {0} hojas. Deberian de ser {1} ¿Desea Continuar?";
            desbordamientoErrorTitulo = "Desbordamiento";
            printTitleDocument = "Páginas de Libro MDB";
        }
        public printData getPrint()
        {
            var response = new printData();
            response.setOrientaciones(orientaciones);
            response.setFontSizes(fontPrintSizes);
            response.setTiposHojas(tiposHojas);
            response.setTextHojaTipo(inputPrintTextHojaTipo);
            response.setTextOrientacion(inputPrintTextOrientacion);
            response.setTextFont(inputPrintTextFont);
            response.setTextInicialNumeroHoja(inputPrintTextInicialNumeroHoja);
            response.setTextFinalNumeroHoja(inputPrintTextFinalNumeroHoja);
            response.setButtonPrint(inputPrintButtonPrint);
            response.setButtonExit(inputPrintButtonExit);
            response.setTitle(inputPrintTitle);
            response.setErrorInput(printErrorInput);
            response.setErrorTitle(printErrorTitle);
            response.setFontFamily(fontToPrint);
            response.setDesbordamiento(desbordamientoError);
            response.setTitleDesbordamiento(desbordamientoErrorTitulo);
            response.setPrintTitle(printTitleDocument);
            return response;
        }
        public string getReaderString()
        {
            return readerMDBString;
        }
        public buscarData getBuscar()
        {
            var response = new buscarData();
            response.setErrorInput(buscarErrorInput);
            response.setErrorNoMatch(buscarErrorNoMatch);
            response.setErrorTitle(buscarErrorTitle);
            return response;
        }
        public querysData getQuerys()
        {
            var response = new querysData();
            response.setMaxHojaNumber(queryMAXHoja);
            response.setOneHoja(queryGetOneHoja);
            response.setFieldNameHojaMAX(fieldHojaMAX);
            response.setFieldHojaText(queryCampoHoja);
            response.setSearchTextHojas(searchQuery);
            response.setOneParam(oneParam);
            response.setFieldHojaNumero(fieldHojaNro);
            response.setGetAllHojasRange(queryGetAllHojasRange);
            return response;
        }
        public dialogueData getDialogue()
        {
            var response = new dialogueData();
            response.setDefaultExt(defaultExt);
            response.setFilterExt(filterExt);
            response.setErrorWindowTitle(titleError);
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
