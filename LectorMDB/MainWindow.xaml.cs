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
        Clases.BaseMDB newMDB = new Clases.BaseMDB();
        Data.dataStorage theData = new Data.dataStorage();
        int plusFont;
        int minusFont;
        char dirSep;
        string defaultExt;
        string filterExt;
        string contentFile;
        string titleError;
        string errorFileFormat;
        string errorOpening;
        public MainWindow()
        {
            InitializeComponent();
            /// Set up defaults in BaseMDB
            newMDB.getDefaultsInts(theData.getBaseMBDInts());
            newMDB.getDefaultStrings(theData.getBaseMBDStrings());
            /// Set up fonts numbers
            var fontsNumbers = theData.getFonts();
            foreach(string oneFont in fontsNumbers)
            {
                fontCombo.Items.Add(oneFont);
            }
            fontCombo.SelectedItem = fontsNumbers[1];
            plusFont = theData.getChangePlus();
            minusFont = theData.getChangeMinus();
            dirSep = System.IO.Path.DirectorySeparatorChar;
            defaultExt = theData.getDefExt();
            filterExt = theData.getFilterExt();
            contentFile = theData.getContentFile();
            titleError = theData.getTitleError();
            errorFileFormat = theData.getErrorFileFormat();
            errorOpening = theData.getErrorOpening();

            Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }
        /// <summary>
        /// When the font plus button is press it changes the font size by +1.
        /// </summary>
        private void fontPlusBoton_Click(object sender, RoutedEventArgs e)
        {
            var newSize = Int32.Parse(fontTamaño.Text) + plusFont;
            changeFontSize(newSize);
        }
        /// <summary>
        /// When the font minus button is press it changes the font size by -1.
        /// </summary>
        private void fontLessBoton_Click(object sender, RoutedEventArgs e)
        {
            var newSize = Int32.Parse(fontTamaño.Text) + minusFont;
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
            dlg.DefaultExt = defaultExt;
            dlg.Filter = filterExt;
            Nullable<bool> result = dlg.ShowDialog();// Display OpenFileDialog by calling ShowDialog method 

            if (result == true)// Get the selected file name and display in a TextBox 
            {
                string fileNameLimpio = System.IO.Path.GetFileNameWithoutExtension(dlg.FileName);
                string pathMDB = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(dlg.FileName), fileNameLimpio, contentFile);     
                if (File.Exists(pathMDB))
                {
                    newMDB.setUpNewBook(pathMDB);
                    nombreMDB.Text = fileNameLimpio;
                    textoHojaFinal.Text = newMDB.numeroHojaMaxima.ToString();
                    CambiarHoja(1);
                }
                else
                {
                    MessageBox.Show($"{errorOpening}{pathMDB}.", titleError);
                }
            }
            else
            {
                MessageBox.Show(errorFileFormat, titleError);
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
            var isNumber = Int32.TryParse(numeroH.Text, out nH);
            var hasChange = false;
            if (isNumber)
            {
                if (nH <= newMDB.numeroHojaMaxima && nH >= 1)
                {
                    CambiarHoja(nH);
                    hasChange = true;
                }
            }
            if (!hasChange)
            {
                numeroH.Text = newMDB.numeroHojaActual.ToString();
            }
        }
        /// <summary>
        /// Given new font size it changes and reloads the container
        /// Doesnt allow fonts bellow or equal to 1.
        /// </summary>
        private void changeFontSize(int fontNew)
        {
            if (fontNew > 1)
            {
                newMDB.fontSize = fontNew;
                fontTamaño.Text = newMDB.fontSize.ToString();
                ContenidoMDB.Document.PageWidth = newMDB.newSizeRichBox();
                ContenidoMDB.FontSize = newMDB.fontSize;
            }
        }
        /// <summary>
        /// Given the new number of hoja it searchs for it in the book. If its valid
        /// it will reload the ContenidoMDB with that hoja.
        /// </summary>
        /// <param name="numeroDeHoja"></param>
        private void CambiarHoja(int numeroDeHoja)
        {
            var hasChange = newMDB.BuscarHoja(numeroDeHoja);
            if (hasChange)
            {
                numeroH.Text = numeroDeHoja.ToString();
                ContenidoMDB.SelectAll();
                ContenidoMDB.Selection.Text = "";
                ContenidoMDB.AppendText(newMDB.hojaActual);
                ContenidoMDB.Document.PageWidth = newMDB.newSizeRichBox();
            }
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
            PrintWindow pw = new PrintWindow(newMDB);
            pw.Show();
        }

        /// <summary>
        /// Changes to the next page of the book.
        /// </summary>
        private void CommandBinding_Executed_1(object sender, ExecutedRoutedEventArgs e)
        {
            CambiarHoja(newMDB.numeroHojaActual + 1);
        }
        /// <summary>
        /// Changes to the previous page of the book.
        /// </summary>
        private void CommandBinding_Executed_2(object sender, ExecutedRoutedEventArgs e)
        {
            CambiarHoja(newMDB.numeroHojaActual - 1);
        }
        /// <summary>
        /// Changes to the first page of the book.
        /// </summary>
        private void CommandBinding_Executed_3(object sender, ExecutedRoutedEventArgs e)
        {
            CambiarHoja(1);
        }
        /// <summary>
        /// Changes to the last page of the book.
        /// </summary>
        private void CommandBinding_Executed_4(object sender, ExecutedRoutedEventArgs e)
        {
            CambiarHoja(newMDB.numeroHojaMaxima);
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
            if(newMDB.path == null)
            {
                return;
            }
            var inputWin = new windows.inputBuscar();
            inputWin.Owner = this; // So it centers on the main window
            if (inputWin.ShowDialog() == true)
            {
                var foundText = false;
                var counter = newMDB.numeroHojaActual;
                while(counter <= newMDB.numeroHojaMaxima)
                {
                    if (newMDB.BuscarHojaRaw(counter).Contains(inputWin.InputText))
                    {
                        CambiarHoja(counter);
                        HighlightText(ContenidoMDB, inputWin.InputText, Brushes.Red);
                        foundText = true;
                        break;
                    }
                    counter++;
                }
                counter = 1;
                if (!foundText)
                {
                    
                    while(counter <= newMDB.numeroHojaActual)
                    {
                        if (newMDB.BuscarHojaRaw(counter).Contains(inputWin.InputText))
                        {
                            CambiarHoja(counter);
                            HighlightText(ContenidoMDB, inputWin.InputText, Brushes.Red);
                            break;
                        }
                        counter++;
                    }
                }
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
