using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Printing;
using System.Text.RegularExpressions;

namespace LectorMDB
{
    /// <summary>
    /// Interaction logic for PrintWindow.xaml
    /// </summary>
    public partial class PrintWindow : Window
    {
        public Clases.BaseMDB libroAImprimir { get; set; }
        public PrintWindow(Clases.BaseMDB libroDado)
        {
            InitializeComponent();
            libroAImprimir = new Clases.BaseMDB(12);
            libroAImprimir.path = libroDado.path;
            libroAImprimir.hojaActual = libroDado.hojaActual;
            libroAImprimir.numeroHojaActual = libroDado.numeroHojaActual;
            libroAImprimir.numeroHojaMaxima = libroDado.numeroHojaMaxima;
            libroAImprimir.fontSize = libroDado.fontSize;
            libroAImprimir.largoHojaActual = libroDado.largoHojaActual;
            printNumeroHoja.Text = libroAImprimir.numeroHojaActual.ToString();
            printConfig.Items.Add("        Una Hoja         ");
            printConfig.Items.Add("Varias Hojas Consecutivas");
            printConfig.SelectedItem = "        Una Hoja         ";
            tipoHoja.Items.Add("Carta");
            tipoHoja.Items.Add("A4");
            tipoHoja.Items.Add("A5");
            tipoHoja.Items.Add("Legal");
            tipoHoja.Items.Add("Ejecutivo");
            tipoHoja.SelectedItem = "Legal";
            orientacionHoja.Items.Add("Horizontal");
            orientacionHoja.Items.Add("Vertical");
            orientacionHoja.SelectedItem = "Horizontal";
            fontPrint.Items.Add("6");
            fontPrint.Items.Add("7");
            fontPrint.Items.Add("8");
            fontPrint.Items.Add("9");
            fontPrint.Items.Add("10");
            fontPrint.Items.Add("11");
            fontPrint.Items.Add("12");
            fontPrint.SelectedItem = "8";
        }
        private void printEverything_Click(object sender, RoutedEventArgs e)
        {
            
            if (printConfig.SelectedItem == "        Una Hoja         " & printNumeroHoja.Text.Length > 0)
            {
                //GeneratePdf(newMDB.hojaActual, newMDB.path.Replace(".MDB", ".pdf").Replace(".mdb", ".pdf"));
                var dlg = new PrintDialog();
                //Prepara Configuracion Impresion
                dlg.PageRangeSelection = PageRangeSelection.AllPages;
                dlg.UserPageRangeEnabled = false;
                dlg.PrintTicket.PageMediaSize = getPageSize();
                dlg.PrintTicket.PageOrientation = getOrientationPage();
                //Prepara Hojas
                var doc = new FlowDocument();
                doc.ColumnWidth = dlg.PrintableAreaWidth;
                doc.PageHeight = dlg.PrintableAreaHeight;
                doc.PageWidth = dlg.PrintableAreaWidth;
                doc.FontSize = Int32.Parse(fontPrint.SelectedItem.ToString());
                doc.FontFamily = new FontFamily("Courier New");
                doc.Blocks.Add(new System.Windows.Documents.Paragraph(new Run(libroAImprimir.hojaActual)));
                DocumentPaginator paginator = ((IDocumentPaginatorSource)doc).DocumentPaginator;
                paginator.ComputePageCount();
                int pagesNumber = paginator.PageCount;
                bool aceptaImprimir = false;
                if (pagesNumber > 1)
                {
                    if (MessageBox.Show("Se va imprimir mas de una Hoja ¿Desea Continuar?", "Aviso", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        aceptaImprimir = true;
                    }
                }
                else if (pagesNumber == 1)
                {
                    aceptaImprimir = true;
                }
                if (aceptaImprimir)
                {
                    if (dlg.ShowDialog() == true)
                    {

                        dlg.PrintDocument(((IDocumentPaginatorSource)doc).DocumentPaginator, "Simple document");
                    }
                }
                
            }
            else if(printConfig.SelectedItem == "Varias Hojas Consecutivas" & printNumeroHojaUno.Text.Length > 0 & printNumeroHojaDos.Text.Length > 0)
            {
                int hojaInicial = Int32.Parse(printNumeroHojaUno.Text);
                int hojaFinal= Int32.Parse(printNumeroHojaDos.Text);
                int hojaCantidad = hojaFinal - hojaInicial + 1;

                if (hojaFinal >= hojaInicial & hojaFinal <= libroAImprimir.numeroHojaMaxima)
                {
                    var dlg = new PrintDialog();
                    dlg.PageRangeSelection = PageRangeSelection.AllPages;
                    dlg.UserPageRangeEnabled = false;

                    dlg.PrintTicket.PageMediaSize = getPageSize();
                    dlg.PrintTicket.PageOrientation = getOrientationPage();
                    var doc = new FlowDocument();
                    doc.ColumnWidth = dlg.PrintableAreaWidth;
                    doc.PageHeight = dlg.PrintableAreaHeight;
                    doc.PageWidth = dlg.PrintableAreaWidth;
                    doc.FontSize = Int32.Parse(fontPrint.SelectedItem.ToString());
                    doc.FontFamily = new FontFamily("Courier New");
                    for (int i = hojaInicial; i <= hojaFinal; i++)
                    {
                        libroAImprimir.darHoja(i);
                        doc.Blocks.Add(new System.Windows.Documents.Paragraph(new Run(libroAImprimir.hojaActual)) { BreakPageBefore = true }) ;
                    }
                    DocumentPaginator paginator = ((IDocumentPaginatorSource)doc).DocumentPaginator;
                    paginator.ComputePageCount();
                    int pagesNumber = paginator.PageCount;
                    bool aceptaImprimir = false;
                    if(pagesNumber > hojaCantidad)
                    {
                        if (MessageBox.Show("Se van imprimir " + pagesNumber.ToString() + " hojas. Deberian de ser " + hojaCantidad.ToString() + "¿Desea Continuar?", "Aviso", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            aceptaImprimir = true;
                        }
                    }
                    else if (pagesNumber == hojaCantidad)
                    {
                        aceptaImprimir = true;
                    }
                    if (aceptaImprimir)
                    {
                        if (dlg.ShowDialog() == true)
                        {

                            dlg.PrintDocument(((IDocumentPaginatorSource)doc).DocumentPaginator, "Simple document");
                        }
                    }
                }

            }
        }

        private PageOrientation getOrientationPage()
        {
            PageOrientation orientacion = PageOrientation.Landscape;
            if (orientacionHoja.SelectedItem.ToString() == "Horizontal")
            {
                orientacion = PageOrientation.Landscape;
            }
            else if (orientacionHoja.SelectedItem.ToString() == "Vertical")
            {
                orientacion = PageOrientation.Portrait;
            }
            return orientacion;
        }
        private PageMediaSize getPageSize()
        {
            PageMediaSize pageSize = null;
            if (tipoHoja.SelectedItem.ToString() == "Carta")
            {
                pageSize = new PageMediaSize(PageMediaSizeName.NorthAmericaLetter);
            }
            else if (tipoHoja.SelectedItem.ToString() == "A4")
            {
                pageSize = new PageMediaSize(PageMediaSizeName.ISOA4);
            }
            else if (tipoHoja.SelectedItem.ToString() == "A5")
            {
                pageSize = new PageMediaSize(PageMediaSizeName.ISOA5);
            }
            else if (tipoHoja.SelectedItem.ToString() == "Legal")
            {
                pageSize = new PageMediaSize(PageMediaSizeName.NorthAmericaLegal);
            }
            else if (tipoHoja.SelectedItem.ToString() == "Ejecutivo")
            {
                pageSize = new PageMediaSize(PageMediaSizeName.NorthAmericaExecutive);
            }
            return pageSize;
        }

        private void printNumeroHoja_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void printNumeroHojaUno_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void printNumeroHojaDos_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void printConfig_DropDownClosed(object sender, EventArgs e)
        {
            if(printConfig.SelectedItem == "        Una Hoja         ")
            {
                printNumeroHojaUno.Text = "";
                printNumeroHojaDos.Text = "";
                printNumeroHojaUno.IsReadOnly = true;
                printNumeroHojaDos.IsReadOnly = true;
                getMaxHoja.IsEnabled = false;
                printNumeroHoja.IsReadOnly = false;
                printNumeroHoja.Text = libroAImprimir.numeroHojaActual.ToString();
            }
            else if (printConfig.SelectedItem == "Varias Hojas Consecutivas")
            {
                printNumeroHojaUno.Text = libroAImprimir.numeroHojaActual.ToString();
                printNumeroHojaDos.Text = libroAImprimir.numeroHojaActual.ToString();
                printNumeroHojaUno.IsReadOnly = false;
                printNumeroHojaDos.IsReadOnly = false;
                printNumeroHoja.IsReadOnly = true;
                getMaxHoja.IsEnabled = true;
                printNumeroHoja.Text = "";
            }
        }

        private void getMaxHoja_Click(object sender, RoutedEventArgs e)
        {
            printNumeroHojaDos.Text = libroAImprimir.numeroHojaMaxima.ToString();
        }
    }
}
