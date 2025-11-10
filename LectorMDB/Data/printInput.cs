using System;
using System.Collections.Generic;
using System.Printing;
using System.Text;
using System.Windows.Media;

namespace LectorMDB.Data
{
    public class printInput
    {
        public string sizePage { get; private set; }
        public string orientacion { get; private set; }
        public string sizeLetter { get; private set; }
        public string startNumber { get; private set; }
        public string endNumber { get; private set; }
        public PageMediaSize finalPageSize { get; private set; }
        public PageOrientation finalOrient { get; private set; }
        public int finalFontSize { get; private set; }
        public FontFamily finalFontFamily { get; private set; }
        public List<int> finalPrintRange { get; private set; }

        private List<string> sizesPage { get; set; }
        private List<string> orientaciones { get; set; }
        private List<string> sizesLetter { get; set; }
        
        public bool areFinalValuesOK()
        {
            var response = false;
            var notNull = false;
            if (finalPageSize != null && finalFontSize != null && finalOrient != null && finalPrintRange != null)
            {
                notNull = true;
            }
            if (notNull)
            {
                if(finalFontSize > 0 && finalPrintRange.Count > 0)
                {
                    response = true;
                }
            }
            return response;
        }

        public void setUpFinalValues(List<int> valuesPageNumbers, string fontFamily)
        {
            getPage();
            getOrient();
            getSizeFont();
            finalPrintRange = valuesPageNumbers;
            finalFontFamily = new FontFamily(fontFamily);
        }
        
        private void getPage()
        {
            PageMediaSize response = null;

            var indexSize = sizesPage.IndexOf(sizePage);
            if(indexSize == 0)//Carta
            {
                response = new PageMediaSize(PageMediaSizeName.NorthAmericaLetter);
            }
            else if(indexSize == 1)//A4
            {
                response = new PageMediaSize(PageMediaSizeName.ISOA4);
            }
            else if (indexSize == 2)//A5
            {
                response = new PageMediaSize(PageMediaSizeName.ISOA5);
            }
            else if (indexSize == 3)//Ejecutivo
            {
                response = new PageMediaSize(PageMediaSizeName.NorthAmericaExecutive);
            }
            else if (indexSize == 4)//Legal
            {
                response = new PageMediaSize(PageMediaSizeName.NorthAmericaLegal);
            }
            finalPageSize = response;
        }
        private void getOrient()
        {
            var response = PageOrientation.Landscape;
            var indexSize = orientaciones.IndexOf(orientacion);
            if (indexSize == 0)//Horizontal
            {
                response = PageOrientation.Landscape;
            }
            else if (indexSize == 1)//Vertical
            {
                response = PageOrientation.Portrait;
            }
            finalOrient = response;
        }
        private void getSizeFont()
        {
            var response = 0;
            var indexSize = sizesLetter.IndexOf(sizeLetter);
            if(indexSize != -1) 
            {
                response = indexSize + 6;
            }
            finalFontSize = response;
        }

        public void setVariables(List<string> pages, List<string> orients, List<string> letters)
        {
            sizesPage = pages;
            orientaciones = orients;
            sizesLetter = letters; 
        }
        public void setSizePage(string value)
        {
            sizePage = value;
        }
        public void setOrientacion(string value)
        {
            orientacion = value;
        }
        public void setSizeLetter(string value)
        {
            sizeLetter = value;
        }
        public void setStartNumber(string value)
        {
            startNumber = value;
        }
        public void setEndNumber(string value)
        {
            endNumber = value;
        }


    }
}
