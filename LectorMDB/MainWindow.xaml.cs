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
        Clases.BaseMDB newMDB = new Clases.BaseMDB(12);
        
        public MainWindow()
        /*
         *  Carga componentes y pueblo el combo del tamaño de las letras.
         */
        {
            InitializeComponent();
            fontCombo.Items.Add("8");
            fontCombo.Items.Add("12");
            fontCombo.Items.Add("24");
            fontCombo.Items.Add("36");
            fontCombo.Items.Add("72");
            fontCombo.SelectedItem = "12";
            Application.Current.MainWindow.WindowState = WindowState.Maximized;


        }
        private void fontPlusBoton_Click(object sender, RoutedEventArgs e)
        /*
         * Click event Botton of increased font size.
         */
        {
            changeFontSize(fontTamaño.Text, 1);
        }
        private void fontLessBoton_Click(object sender, RoutedEventArgs e)
        {
        /*
        * Click event Botton of decreased font size.
        */
            changeFontSize(fontTamaño.Text, - 1);
        }
        
        private void fontCombo_DropDownClosed(object sender, EventArgs e)
        /*
         *  Event Combo-font is closed. Change fontsize
         */
        {
            changeFontSize(fontCombo.SelectedItem.ToString(), 0);
        }

        private void fontCombo_KeyDown(object sender, KeyEventArgs e)
        /*
         *  Event Combo-font down botton is pressed. Change fontsize
         */
        {
            changeFontSize(fontCombo.SelectedItem.ToString(), 0);
        }

        private void fontCombo_KeyUp(object sender, KeyEventArgs e)
        /*
         *  Event Combo-font up botton is pressed. Change fontsize
         */
        {
            changeFontSize(fontCombo.SelectedItem.ToString(), 0);
        }
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        /*
         * Open file explorer to open MDB File. Acepts diferent types of MDB configurations.
         */
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();// Set filter for file extension and default file extension
            dlg.DefaultExt = ".mdb";
            dlg.Filter = "Base Access|*.mdb";
            Nullable<bool> result = dlg.ShowDialog();// Display OpenFileDialog by calling ShowDialog method 

            if (result == true)// Get the selected file name and display in a TextBox 
            {
                char sepOS = System.IO.Path.DirectorySeparatorChar;
                string[] pathSplited = dlg.FileName.Split(sepOS);
                int indexNameFile = pathSplited.Length - 1;
                string fileName = dlg.FileName.Split(sepOS)[indexNameFile];
                string fileNameLimpio = fileName.Replace(".MDB", "").Replace(".mdb", "");
                string pathMDB = dlg.FileName.Replace(fileName, "") + fileNameLimpio + sepOS + "H.MDB";
                string pathMDBDesastre = dlg.FileName.Replace(fileName, "") + sepOS + "H.MDB";
                if (File.Exists(pathMDB))
                {
                    loadMDBFile(fileName, pathMDB);
                    EnableButtons();
                }
                else if(newMDB.isMastracho(dlg.FileName)){
                    loadMDBFile(dlg.FileName, dlg.FileName);
                    EnableButtons();
                }
                else if (File.Exists(pathMDBDesastre))
                {
                    loadMDBFile(fileName, pathMDBDesastre);
                    EnableButtons();
                }
                else
                {
                    MessageBox.Show("Archivo MDB no valido.", "Error");
                }

            }
            else
            {
                MessageBox.Show("Error Archivo no valido", "Error");
            }
        }
        private void EnableButtons()
        {
            printOpen.IsEnabled = true;
            irABusqueda.IsEnabled = true;
            copiarTodo.IsEnabled = true;
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
        private void irABusqueda_Click(object sender, RoutedEventArgs e)
        /*
         *  Event When irABusqueda is clicked. Searches the mdb given the condition in claveBusqueda and checkBoxRegex.
         */
        {
            if (newMDB.path != null)
            {
                listaBusqueda.Items.Clear();
                List<string> listaResultados = new List<string>();
                if (checkBoxRegex.IsChecked.Value)
                {
                    listaResultados = newMDB.searchLines(claveBusqueda.Text, claveBusqueda.Text);
                }
                else
                {
                    listaResultados = newMDB.searchLines(claveBusqueda.Text);
                }

                foreach (string linea in listaResultados)
                {
                    listaBusqueda.Items.Add(linea);
                }

            }
        }
        private void copiarItemBusqueda(object sender, RoutedEventArgs e)
        /*
         *  Event When a item in listaBusqueda is right clicked. Copy the item info to the clipboard.
         */
        {
            Clipboard.SetText(listaBusqueda.SelectedItem.ToString().Split("|-=-|")[1], TextDataFormat.Text);
        }

        private void irAHojaBusqueda(object sender, RoutedEventArgs e)
        /*
         *  Event When a item in listaBusqueda is right clicked. Goes to the sheet that the item comes from.
         */
        {
            string numeroHoja = listaBusqueda.SelectedItem.ToString().Split("|-=-|")[0].Split("Número de Hoja: ")[1];
            tabLibro.IsSelected = true;
            numeroH.Text = numeroHoja;
            ButtonAutomationPeer peer = new ButtonAutomationPeer(irAHoja);
            IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
            invokeProv.Invoke();
            
        }
        private void changeFontSize(string source, int change)
        /*
         *  Change font size in newMDB and ContenidoMDB, doesn't allow a value of fontsize below 1.
         */
        {
            if (newMDB.fontSize != 1)
            {
                newMDB.fontSize = Convert.ToInt32(source) + change;
                fontTamaño.Text = newMDB.fontSize.ToString();
                ContenidoMDB.Document.PageWidth = newMDB.newSizeRichBox();
                ContenidoMDB.FontSize = newMDB.fontSize;
            }
        }
        private void loadMDBFile(string pathToFile, string pathToMDBWithInfo)
        {
            /*
             *  Refreshes nombreMDB and textoHojaFinal and set newMDB with the information of the new File.
             */
            nombreMDB.Text = pathToFile;
            newMDB.path = pathToMDBWithInfo;
            CambiarHoja(1);
            newMDB.setHojaMaxima();
            textoHojaFinal.Text = "/" + newMDB.numeroHojaMaxima.ToString();
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

        private void copiarTodo_Click(object sender, RoutedEventArgs e)
        {
            string textoACopiar = "";
            foreach(string line in listaBusqueda.Items)
            {
                textoACopiar += line.ToString().Split("|-=-|")[1] + "\n";
            }
            Clipboard.SetText(textoACopiar, TextDataFormat.Text);
        }
    }
}
