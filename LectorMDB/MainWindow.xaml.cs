using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Windows.Automation.Provider;
using System.Windows.Automation.Peers;


using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Printing;

namespace LectorMDB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// Main Clases
        Data.dataStorage theData = new Data.dataStorage();
        MDBConexion.MDBConexion theMDBConexion = new MDBConexion.MDBConexion();
        Data.fontData dFont;
        Data.dialogueData dDialogue;
        Data.querysData dQuerys;
        Data.inputsData dInputs;
        Data.buscarData dBuscar;
        Data.printData dPrint;
        Clases.Libro libroActual;

        public MainWindow()
        {
            InitializeComponent();
            /// Get Datas
            dFont = theData.getFont();
            dDialogue = theData.getDialogue();
            dQuerys = theData.getQuerys();
            dBuscar = theData.getBuscar();
            dPrint = theData.getPrint();
            theMDBConexion.setConextionString(theData.getReaderString());
            /// Set up fonts combo
            foreach (string oneFont in dFont.combo)
            {
                fontCombo.Items.Add(oneFont);
            }
            fontCombo.SelectedItem = dFont.combo[1];

            Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }
        /// <summary>
        /// When the font plus button is press it changes the font size by +1.
        /// </summary>
        private void fontPlusBoton_Click(object sender, RoutedEventArgs e)
        {
            var newSize = Int32.Parse(fontTamaño.Text) + dFont.plus;
            changeFontSize(newSize);
        }
        /// <summary>
        /// When the font minus button is press it changes the font size by -1.
        /// </summary>
        private void fontLessBoton_Click(object sender, RoutedEventArgs e)
        {
            var newSize = Int32.Parse(fontTamaño.Text) + dFont.minus;
            changeFontSize(newSize);
        }
        /// <summary>
        /// Changes the font to the number of the combo that is selected.
        /// </summary>
        private void fontCombo_DropDownClosed(object sender, EventArgs e)
        {
            if(fontCombo.SelectedItem is string)
            {
                var newSize = Int32.Parse(fontCombo.SelectedItem.ToString());
                changeFontSize(newSize);
            }  
        }
        /// <summary>
        /// Changes the font to the number of the combo that is selected.
        /// </summary>
        private void fontCombo_KeyDown(object sender, KeyEventArgs e)
        {
            if (fontCombo.SelectedItem is string)
            {
                var newSize = Int32.Parse(fontCombo.SelectedItem.ToString());
                changeFontSize(newSize);
            }
        }
        /// <summary>
        /// Changes the font to the number of the combo that is selected.
        /// </summary>
        private void fontCombo_KeyUp(object sender, KeyEventArgs e)
        {
            if (fontCombo.SelectedItem is string)
            {
                var newSize = Int32.Parse(fontCombo.SelectedItem.ToString());
                changeFontSize(newSize);
            }
        }
        /// <summary>
        /// Shows the dialog to open the MDB file. If correct its set up the file in the newMDB and set up variables in the window
        /// and change the page number to one.
        /// </summary>
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();// Set filter for file extension and default file extension
            dlg.DefaultExt = dDialogue.defaultExtension;
            dlg.Filter = dDialogue.filterExtension;
            var result = dlg.ShowDialog();// Display OpenFileDialog by calling ShowDialog method 
            var existMDBContent = false;
            var existMaxHoja = false;
            var fileNameLimpio = "";
            var pathMDB = "";
            var maxHoja = 0;
            var errorResult = "";

            if(result == true)
            {
                fileNameLimpio = System.IO.Path.GetFileNameWithoutExtension(dlg.FileName);
                pathMDB = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(dlg.FileName), fileNameLimpio, dDialogue.fileOfHojas);
                existMDBContent = File.Exists(pathMDB);
            }
            else
            {
                errorResult = dDialogue.errorWindowWrongFile;
            }

            if (existMDBContent)
            {
                var resultQuery = theMDBConexion.getSimple(dQuerys.getMaxHojaNumber, pathMDB, dQuerys.fieldNameHojaMAX);
                existMaxHoja = Int32.TryParse(resultQuery[0], out maxHoja);
            }
            else
            {
                errorResult = $"{dDialogue.errorWindowNotContentFile}{pathMDB}";
            }

            if(existMDBContent & existMaxHoja)
            {
                setUpNewLibro(pathMDB, maxHoja, fileNameLimpio);
            }
            else
            {
                MessageBox.Show(errorResult, dDialogue.errorWindowTitle);
            }
        }
        /// <summary>
        /// Takes the text in numeroH if is a number and is a valid page number it change to that page.
        /// If not it change numeroH to newMDB.numeroHojaActual.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void irAHoja_Click(object sender, RoutedEventArgs e)
        {
            int nH = 0;
            var inputWin = new windows.inputNumberPage(dInputs.textInputNP, dInputs.titleInputNP);
            inputWin.Owner = this; // So it centers on the main window
            if (inputWin.ShowDialog() == true)
            {
                var numberPage = inputWin.txtInput;
                var isNumber = Int32.TryParse(numeroH.Text, out nH);
                if (isNumber)
                {
                    CambiarHoja(nH);
                }
                else
                {
                    MessageBox.Show(dInputs.errorTextInputNP, dInputs.errorTitleInputNP);
                }
            }
        }
        private void setUpNewLibro(string path, int maxHoja, string shortName)
        {
            libroActual = new Clases.Libro();
            libroActual.setPath(path);
            libroActual.setHojaMax(maxHoja);
            nombreMDB.Text = shortName;
            textoHojaFinal.Text = libroActual.numeroHojaMaxima.ToString();
            CambiarHoja(1);
        }
        
        /// <summary>
        /// Given new font size it changes and reloads the container
        /// Doesnt allow fonts bellow or equal to 1.
        /// </summary>
        private void changeFontSize(int fontNew)
        {
            if (fontNew > 1)
            {
                fontTamaño.Text = fontNew.ToString();
                ContenidoMDB.Document.PageWidth = getNewSizePage();
                ContenidoMDB.FontSize = fontNew;
            }
        }
        /// <summary>
        /// Given the new number of hoja it searchs for it in the book. If its valid
        /// it will reload the ContenidoMDB with that hoja.
        /// </summary>
        /// <param name="numeroDeHoja"></param>
        private void CambiarHoja(int numeroDeHoja)
        {
            var isValidNumber = libroActual.isValidNumberHoja(numeroDeHoja);
            if (isValidNumber)
            {
                var paramsQuery = new List<string> { dQuerys.oneParam };
                var valuesParamsQuery = new List<string> { numeroDeHoja.ToString() };
                var rawHoja = theMDBConexion.getSimple(dQuerys.getOneHoja, libroActual.path, paramsQuery, valuesParamsQuery, dQuerys.fieldHojaText)[0];
                numeroH.Text = numeroDeHoja.ToString();
                ContenidoMDB.SelectAll();
                ContenidoMDB.Selection.Text = "";
                ContenidoMDB.AppendText(rawHoja);
                ContenidoMDB.Document.PageWidth = getNewSizePage(rawHoja);
            }
            else
            {
                MessageBox.Show(dInputs.errorCambiarHoja, dInputs.titleErrorCambiarHoja);
            }
        }
        private int getNewSizePage(string rawHoja)
        {
            var largo = 0;
            foreach (string linea in rawHoja.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                if (linea.Length > largo)
                {
                    largo = linea.Length;
                }
            }
            float cantidad = Convert.ToSingle(4.9) + (Convert.ToSingle(0.6) * (Convert.ToSingle(fontTamaño.Text) - 8));
            return Convert.ToInt32(largo * cantidad);
        }
        private int getNewSizePage()
        {
            var largo = 0;
            string text = new TextRange(ContenidoMDB.Document.ContentStart, ContenidoMDB.Document.ContentEnd).Text;
            foreach (string linea in text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                if (linea.Length > largo)
                {
                    largo = linea.Length;
                }
            }
            float cantidad = Convert.ToSingle(4.9) + (Convert.ToSingle(0.6) * (Convert.ToSingle(fontTamaño.Text) - 8));
            return Convert.ToInt32(largo * cantidad);
        }
        public void GeneratePdf(string htmlPdf, string locationPDF)
        {
            var pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            var htmlparser = new HTMLWorker(pdfDoc);
            using (var memoryStream = new MemoryStream())
            {
                var writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                htmlparser.Parse(new StringReader(htmlPdf));
                pdfDoc.Close();

                byte[] bytes = memoryStream.ToArray();
                File.WriteAllBytes(locationPDF, bytes);

                memoryStream.Close();
            }
        }

        private void printOpen_Click(object sender, RoutedEventArgs e)
        {

            windows.inputPrint aPrintWindow = new windows.inputPrint(dPrint);
            aPrintWindow.Show();

            if (aPrintWindow.ShowDialog() == true)
            {
                var result = aPrintWindow.theInput;
                var pageSize = result.getPage();
                var orient = result.getOrient();
                var fontSize = result.getSizeFont();
                var printRange = getPrintRage(result.startNumber, result.endNumber);
                
                var okInput = false;
                if(pageSize != null || fontSize != 0 || printRange.Count > 0)
                {
                    okInput = true;
                }

                if (okInput)
                {
                    printPages(printRange[0], printRange[1], pageSize, orient, fontSize);
                }

            }

        }
        private void printPages(int hojaInicial, int hojaFinal, PageMediaSize ps, PageOrientation po, int fs)
        {
            int hojaCantidad = hojaFinal - hojaInicial + 1;

            var dlg = new PrintDialog();
            dlg.PageRangeSelection = PageRangeSelection.AllPages;
            dlg.UserPageRangeEnabled = false;

            dlg.PrintTicket.PageMediaSize = ps;
            dlg.PrintTicket.PageOrientation = po;
            var doc = new FlowDocument();
            doc.ColumnWidth = dlg.PrintableAreaWidth;
            doc.PageHeight = dlg.PrintableAreaHeight;
            doc.PageWidth = dlg.PrintableAreaWidth;
            doc.FontSize = fs;
            doc.FontFamily = new FontFamily("Courier New");


            var paramsQuery = new List<string> { dQuerys.oneParam };
            
            for (int i = hojaInicial; i <= hojaFinal; i++)
            {
                var valuesParamsQuery = new List<string> { i.ToString() };
                var rawHoja = theMDBConexion.getSimple(dQuerys.getOneHoja, libroActual.path, paramsQuery, valuesParamsQuery, dQuerys.fieldHojaText)[0];
                doc.Blocks.Add(new System.Windows.Documents.Paragraph(new Run(rawHoja)) { BreakPageBefore = true });
            }
            DocumentPaginator paginator = ((IDocumentPaginatorSource)doc).DocumentPaginator;
            paginator.ComputePageCount();
            int pagesNumber = paginator.PageCount;
            bool aceptaImprimir = false;
            if (pagesNumber > hojaCantidad)
            {
                if (MessageBox.Show("Desbordamiento. Se van imprimir " + pagesNumber.ToString() + " hojas. Deberian de ser " + hojaCantidad.ToString() + "¿Desea Continuar?", "Aviso", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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
                    this.Close();
                }
            }
        }


        private List<int> getPrintRage(string firstNumber, string endNumber)
        {
            var response = new List<int>();
            var startNumberInt = 0;
            var endNumberInt = 0;

            var isIntStarting = Int32.TryParse(firstNumber, out startNumberInt);
            var isIntEnding = Int32.TryParse(endNumber, out endNumberInt);

            var areNumbers = false;
            var isInRangeStart = false;
            var isInRangeEnd = false;
            var areInDistance = false;
            if (isIntStarting && isIntEnding)
            {
                areNumbers = true;
                if(startNumberInt > 0 && startNumberInt <= libroActual.numeroHojaMaxima)
                {
                    isInRangeStart = true;
                }
                if (endNumberInt > 0 && endNumberInt <= libroActual.numeroHojaMaxima)
                {
                    isInRangeStart = true;
                }
            }
            if(isInRangeStart && isInRangeEnd)
            {
                if(startNumberInt <= endNumberInt)
                {
                    areInDistance = true;
                }
            }
            if (areInDistance)
            {
                response.Add(startNumberInt);
                response.Add(endNumberInt);
            }
            return response;
        }

        /// <summary>
        /// Changes to the next page of the book.
        /// </summary>
        private void CommandBinding_Executed_1(object sender, ExecutedRoutedEventArgs e)
        {
            var nH = 0;
            var esNumero = Int32.TryParse(numeroH.Text, out nH);
            if (esNumero)
            {
                CambiarHoja(nH + 1);
            }
        }
        /// <summary>
        /// Changes to the previous page of the book.
        /// </summary>
        private void CommandBinding_Executed_2(object sender, ExecutedRoutedEventArgs e)
        {
            var nH = 0;
            var esNumero = Int32.TryParse(numeroH.Text, out nH);
            if (esNumero)
            {
                CambiarHoja(nH - 1);
            }
        }
        /// <summary>
        /// Changes to the first page of the book.
        /// </summary>
        private void CommandBinding_Executed_3(object sender, ExecutedRoutedEventArgs e)
        {
            if (libroActual != null)
            {
                CambiarHoja(1);
            }
        }
        /// <summary>
        /// Changes to the last page of the book.
        /// </summary>
        private void CommandBinding_Executed_4(object sender, ExecutedRoutedEventArgs e)
        {
            if (libroActual != null)
            {
                CambiarHoja(libroActual.numeroHojaMaxima);
            }
        }
        /// <summary>
        /// Close application.
        /// </summary>
        private void CommandBinding_Executed_5(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void buscar_Click(object sender, RoutedEventArgs e)
        {
            var tieneLibro = false;
            var tieneInput = false;
            var tieneResultado = false;
            var textInput = "";
            var numberPage = 0;
            if(libroActual == null)
            {
                tieneLibro = true;
            }
            if (tieneLibro)
            {
                var inputWin = new windows.inputBuscar();
                inputWin.Owner = this; // So it centers on the main window
                if (inputWin.ShowDialog() == true)
                {
                    textInput = inputWin.InputText;
                    tieneInput = true;
                }
            }
            if (tieneInput)
            {
                var paramsQuery = new List<string> { dQuerys.oneParam };
                var valuesParamsQuery = new List<string> { textInput };
                var pageNumberRaw = theMDBConexion.getSimple(dQuerys.searchTextHojas, libroActual.path, paramsQuery, valuesParamsQuery, dQuerys.fieldHojaNumero);
                if (pageNumberRaw.Count > 0)
                {
                    tieneResultado = true;
                    numberPage = Int32.Parse(pageNumberRaw[0]);
                }
            }
            if (tieneResultado)
            {
                
                CambiarHoja(numberPage);
                HighlightText(ContenidoMDB, textInput, Brushes.Red);
            }

            if (!tieneLibro)
            {
                MessageBox.Show(dBuscar.errorLibro, dBuscar.errorTitle);
            }
            else if (!tieneInput)
            {
                MessageBox.Show(dBuscar.errorInput, dBuscar.errorTitle);
            }
            else if (!tieneResultado)
            {
                MessageBox.Show(dBuscar.errorNoMatch, dBuscar.errorTitle);
            }
        }
        private void HighlightText(System.Windows.Controls.RichTextBox richTextBox, string searchText, Brush color)
        {
            TextRange text = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);

            // Find the text
            TextPointer current = text.Start.GetInsertionPosition(LogicalDirection.Forward);
            while (current != null)
            {
                string textInRun = current.GetTextInRun(LogicalDirection.Forward);
                int indexInRun = textInRun.IndexOf(searchText, System.StringComparison.OrdinalIgnoreCase);
                if (indexInRun >= 0)
                {
                    TextPointer start = current.GetPositionAtOffset(indexInRun);
                    TextPointer end = start.GetPositionAtOffset(searchText.Length);

                    var selection = new TextRange(start, end);
                    selection.ApplyPropertyValue(TextElement.ForegroundProperty, color);
                }
                current = current.GetNextContextPosition(LogicalDirection.Forward);
            }
        }
    }
}
