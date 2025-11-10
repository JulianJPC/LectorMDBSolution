using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace LectorMDB.Data
{
    public class readyToPrintData
    {
        public PrintDialog dlg { get; private set; }
        public FlowDocument doc { get; private set; }
        public DocumentPaginator paginator { get; private set; }
        public bool isOverflowing { get; private set; }
        public int qPages { get; private set; }
        public int qPrintingPages { get; private set; }

        public void setQPrintingPages(int value)
        {
            qPrintingPages = value;
        }
        public void setPagesQ(Data.printInput input)
        {
            qPages = input.finalPrintRange[1] - input.finalPrintRange[0] + 1;
            qPrintingPages = paginator.PageCount;
            setIsOverflowing();
        }
        private void setIsOverflowing()
        {
            if(qPrintingPages != qPages)
            {
                isOverflowing = true;
            }
            else
            {
                isOverflowing = false;
            }
        }
        public void addPagesToDoc(List<string> allHojas)
        {
            foreach (var unaHoja in allHojas)
            {
                doc.Blocks.Add(new System.Windows.Documents.Paragraph(new Run(unaHoja)) { BreakPageBefore = true });
            }
        }
        public void setUpPaginator()
        {
            paginator = ((IDocumentPaginatorSource)doc).DocumentPaginator;
            paginator.ComputePageCount();
        }
        public void setDlg(PrintDialog value, Data.printInput pi)
        {
            dlg = value;
            dlg.PageRangeSelection = PageRangeSelection.AllPages;
            dlg.UserPageRangeEnabled = false;
            dlg.PrintTicket.PageMediaSize = pi.finalPageSize;
            dlg.PrintTicket.PageOrientation = pi.finalOrient;
        }
        public void setDoc(FlowDocument value, Data.printInput pi)
        {
            doc = value;
            doc.ColumnWidth = dlg.PrintableAreaWidth;
            doc.PageHeight = dlg.PrintableAreaHeight;
            doc.PageWidth = dlg.PrintableAreaWidth;
            doc.FontSize = pi.finalFontSize;
            doc.FontFamily = pi.finalFontFamily;
        }
        public void setPaginator(DocumentPaginator value)
        {
            paginator = value;
        }

    }
}
