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
        private void fontPlusBoton_Click(object sender, RoutedEventArgs e)
        {
            var newSize = Int32.Parse(fontTamaño.Text + plusFont);
            changeFontSize(newSize);
        }
        private void fontLessBoton_Click(object sender, RoutedEventArgs e)
        {
            var newSize = Int32.Parse(fontTamaño.Text + minusFont);
            changeFontSize(newSize);
        }
        private void fontCombo_DropDownClosed(object sender, EventArgs e)
        {
            if(fontCombo.SelectedItem is string)
            {
                var newSize = Int32.Parse(fontCombo.SelectedItem.ToString());
                changeFontSize(newSize);
            }  
        }
        private void fontCombo_KeyDown(object sender, KeyEventArgs e)
        {
            if (fontCombo.SelectedItem is string)
            {
                var newSize = Int32.Parse(fontCombo.SelectedItem.ToString());
                changeFontSize(newSize);
            }
        }
        private void fontCombo_KeyUp(object sender, KeyEventArgs e)
        {
            if (fontCombo.SelectedItem is string)
            {
                var newSize = Int32.Parse(fontCombo.SelectedItem.ToString());
                changeFontSize(newSize);
            }
        }
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        /*
         * Open file explorer to open MDB File. Acepts diferent types of MDB configurations.
         */
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
                    loadMDBFile(fileNameLimpio, pathMDB);
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

        private void CommandBinding_Executed_1(object sender, ExecutedRoutedEventArgs e)
        /*
         * Event press D key. Changes to next page
         */
        {
            if (textoHojaFinal.Text != "/0" & newMDB.numeroHojaMaxima != newMDB.numeroHojaActual)
            {
                CambiarHoja(newMDB.numeroHojaActual + 1);
            }
        }
        private void CommandBinding_Executed_2(object sender, ExecutedRoutedEventArgs e)
        /*
         * Event press A key. Changes to next page
         */
        {
            if (textoHojaFinal.Text != "/0" & newMDB.numeroHojaActual != 1)
            {
                CambiarHoja(newMDB.numeroHojaActual - 1);
            }
        }
        private void CommandBinding_Executed_3(object sender, ExecutedRoutedEventArgs e)
        /*
         * Event press W key. Changes to next page
         */
        {
            if (newMDB.numeroHojaMaxima != -1)
            {
                CambiarHoja(1);
            }
        }
        private void CommandBinding_Executed_4(object sender, ExecutedRoutedEventArgs e)
        /*
         * Event press S key. Changes to next page
         */
        {
            if (newMDB.numeroHojaMaxima != -1)
            {
                CambiarHoja(newMDB.numeroHojaMaxima);
            }

        }
        private void CommandBinding_Executed_5(object sender, ExecutedRoutedEventArgs e)
        /*
         *  Event when R key is pressed. Exit Application.
         */
        {
            Application.Current.Shutdown();
        }
        private void irAHoja_Click(object sender, RoutedEventArgs e)
        /*
         * Event when irAHoja is clicked. Changes to the given numer of page.
         */
        {
            int nH = Convert.ToInt32(numeroH.Text);
            if (nH <= newMDB.numeroHojaMaxima)
            {
                CambiarHoja(nH);
            }
            else
            {
                numeroH.Text = newMDB.numeroHojaActual.ToString();
            }
        }
        private void numeroH_PreviewTextInput(object sender, TextCompositionEventArgs e)
        /*
         *  Event before a value is given to numeroH. Deletes all values that aren`t a number.
         */
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void numeroH_KeyDown(object sender, KeyEventArgs e)
        /*
         * Event when a key is pressed in numeroH. If the key is return, changes sheet to the text in numeroH.
         */
        {
            if (e.Key == Key.Return)
            {
                int nH = Convert.ToInt32(numeroH.Text);
                if (nH <= newMDB.numeroHojaMaxima)
                {
                    CambiarHoja(nH);
                }
                else
                {
                    numeroH.Text = newMDB.numeroHojaActual.ToString();
                }
                irAHoja.Focus();
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
        private void loadMDBFile(string shortName, string pathToMDBWithInfo)
        {
            /*
             *  Refreshes nombreMDB and textoHojaFinal and set newMDB with the information of the new File.
             */
            nombreMDB.Text = shortName;
            newMDB.path = pathToMDBWithInfo;
            CambiarHoja(1);
            newMDB.setHojaMaxima();
            textoHojaFinal.Text = newMDB.numeroHojaMaxima.ToString();
        }
        private void CambiarHoja(int numeroDeHoja)
        /*
         *  Given a numeroDeHoja, changes sheet in newMDB and Contenido MDB.
         */
        {
            newMDB.darHoja(numeroDeHoja);
            numeroH.Text = numeroDeHoja.ToString();
            ContenidoMDB.SelectAll();
            ContenidoMDB.Selection.Text = "";
            ContenidoMDB.AppendText(newMDB.hojaActual);
            ContenidoMDB.Document.PageWidth = newMDB.newSizeRichBox();
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
    }
}
