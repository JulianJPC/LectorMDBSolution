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
            dInputs = theData.getInputs();
            theMDBConexion.setConextionString(theData.getReaderString());


            // Get screen dimensions
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;

            // Set window size (80% of screen)
            Width = screenWidth * 0.8;
            Height = screenHeight * 0.8;

            // Center the window
            WindowStartupLocation = WindowStartupLocation.Manual;
            Left = (screenWidth - Width) / 2;
            Top = (screenHeight - Height) / 2;
        }
        /// <summary>
        /// Changes to the first page of the book.
        /// </summary>
        private void goToFirstPage_Click(object sender, RoutedEventArgs e)
        {
            CambiarHoja(1);
        }
        /// <summary>
        /// Changes to the previous page of the book.
        /// </summary>
        private void goToPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            var nH = 0;
            var esNumero = Int32.TryParse(numeroH.Text, out nH);
            if (esNumero)
            {
                CambiarHoja(nH - 1);
            }
        }
        /// <summary>
        /// Changes to the next page of the book.
        /// </summary>
        private void goToNextPage_Click(object sender, RoutedEventArgs e)
        {
            var nH = 0;
            var esNumero = Int32.TryParse(numeroH.Text, out nH);
            if (esNumero)
            {
                CambiarHoja(nH + 1);
            }
        }
        /// <summary>
        /// Changes to the last page of the book.
        /// </summary>
        private void goToLastPage_Click(object sender, RoutedEventArgs e)
        {
            var maxHoja = 0;
            if (libroActual != null)
            {
                maxHoja = libroActual.numeroHojaMaxima;
            }
            CambiarHoja(maxHoja);
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
        /// Shows the dialog to open the MDB file. If correct its set up the file in the newMDB and set up variables in the window
        /// and change the page number to one.
        /// </summary>
        private async void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
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
                LoaderOverlay.Visibility = Visibility.Visible; // 👈 Show loader
                await Task.Run(() =>
                {
                    setUpNewLibro(pathMDB, maxHoja, fileNameLimpio);
                });
                LoaderOverlay.Visibility = Visibility.Collapsed;
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
        private void irAHoja_Click(object sender, RoutedEventArgs e)
        {
            int nH = 0;
            var inputWin = new windows.inputNumberPage(dInputs.textInputNP, dInputs.titleInputNP);
            inputWin.Owner = this; // So it centers on the main window

            if (inputWin.ShowDialog() == true)
            {
                var numberPage = inputWin.InputText;
                var isNumber = Int32.TryParse(numberPage, out nH);
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
        /// <summary>
        /// Opens the inputPrint Window that gets the input of the user.
        /// Then checks if the input is ok.
        /// If is ok goes to print the pages.
        /// </summary>
        private async void printOpen_Click(object sender, RoutedEventArgs e)
        {
            var aPrintWindow = new windows.inputPrint(dPrint);
            aPrintWindow.Owner = this; // So it centers on the main window
            var okInput = false;
            Data.printInput inputPrint = null;


            if (aPrintWindow.ShowDialog() == true)
            {
                inputPrint = aPrintWindow.theInput;
                var printRange = getPrintRage(inputPrint.startNumber, inputPrint.endNumber);
                inputPrint.setUpFinalValues(printRange, dPrint.fontFamily);

                okInput = inputPrint.areFinalValuesOK(); 
            }


            if (okInput)
            {
                try
                {
                    LoaderOverlay.Visibility = Visibility.Visible; // 👈 Show loader
                    
                    await Task.Run(() =>
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            var newPrint = new Data.readyToPrintData();
                            var continueOverflowing = false;
                            newPrint.setDlg(new PrintDialog(), inputPrint);
                            newPrint.setDoc(new FlowDocument(), inputPrint);
                            newPrint.addPagesToDoc(getPagesRange(inputPrint));
                            newPrint.setUpPaginator();
                            newPrint.setPagesQ(inputPrint);
                            if (newPrint.isOverflowing)
                            {
                                var message = string.Format(dPrint.desbordamientoError, newPrint.qPrintingPages, newPrint.qPages);
                                var continuarDesbordado = MessageBox.Show(message, dPrint.titleDesbordamiento, MessageBoxButton.YesNo);
                                if (continuarDesbordado == MessageBoxResult.Yes)
                                {
                                    continueOverflowing = true;
                                }
                            }
                            if (!newPrint.isOverflowing || (newPrint.isOverflowing && continueOverflowing))
                            {
                                if (newPrint.dlg.ShowDialog() == true)
                                {
                                    newPrint.dlg.PrintDocument(newPrint.paginator, dPrint.printTitle);
                                }
                            }
                        });
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, dPrint.errorTitle);
                }
                finally
                {
                    LoaderOverlay.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                MessageBox.Show(dPrint.errorInput, dPrint.errorTitle);
            }
        }
        /// <summary>
        /// Opens the inputBuscar Window that gets the input of the user.
        /// Then checks if the input is ok.
        /// If is ok goes to page that has a match with the input.
        /// </summary>
        private async void buscar_Click(object sender, RoutedEventArgs e)
        {
            var tieneInput = false;
            var tieneResultado = false;
            var textInput = "";
            var numberPage = 0;

            var inputWin = new windows.inputBuscar();
            inputWin.Owner = this; // So it centers on the main window
            if (inputWin.ShowDialog() == true)
            {
                textInput = inputWin.InputText;
                tieneInput = true;
            }
            
            if (tieneInput && libroActual != null)
            {
                LoaderOverlay.Visibility = Visibility.Visible; // 👈 Show loader
                await Task.Run(() =>
                {
                    var paramsQuery = new List<string> { dQuerys.oneParam };
                    var valuesParamsQuery = new List<string> { textInput };
                    var pageNumberRaw = theMDBConexion.getWithRegex(dQuerys.searchTextHojas, libroActual.path, paramsQuery, valuesParamsQuery, dQuerys.fieldHojaNumero);
                    var numberOfPageNow = 0;
                    var isIntNumberNow = false;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                      isIntNumberNow = Int32.TryParse(numeroH.Text, out numberOfPageNow);
                    });
                    if (pageNumberRaw.Count > 0 && isIntNumberNow)
                    {
                        foreach(string oneStringNumber in pageNumberRaw)//Gets the page that has match and is bigger than the actual number of page
                        {
                            var pageListNumber = Int32.Parse(oneStringNumber);
                            if(pageListNumber > numberOfPageNow)
                            {
                                numberPage = pageListNumber;
                                break;
                            }
                        }
                        if(numberPage == 0)// If it doesnt find a page with a bigger number it gets the first page
                        {
                            numberPage = Int32.Parse(pageNumberRaw[0]);
                        }
                        tieneResultado = true;
                    }
                });
                LoaderOverlay.Visibility = Visibility.Collapsed;
            }

            if (tieneResultado)
            {
                CambiarHoja(numberPage);
                HighlightText(ContenidoMDB, textInput, Brushes.Red);
            }
            else if (!tieneResultado && tieneInput)
            {
                MessageBox.Show(dBuscar.errorNoMatch, dBuscar.errorTitle);
            }
            
        }
        /// <summary>
        /// Gets the basic parameters of a new Libro
        /// creates the class and gives them values
        /// also changes the values of the mainWindow values.
        /// </summary>
        /// <param name="path">Path where the mdb is located</param>
        /// <param name="maxHoja">Max number of pages in the MDB</param>
        /// <param name="shortName">Short Name of the MDB</param>
        private void setUpNewLibro(string path, int maxHoja, string shortName)
        {
            libroActual = new Clases.Libro();
            libroActual.setPath(path);
            libroActual.setHojaMax(maxHoja);
            Application.Current.Dispatcher.Invoke(() =>
            {
            nombreMDB.Text = shortName;
            textoHojaFinal.Text = libroActual.numeroHojaMaxima.ToString();
            CambiarHoja(1);
            });
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
        /// <param name="numeroDeHoja">Number of the Hoja to change</param>
        private void CambiarHoja(int numeroDeHoja)
        {
            var isValidNumber = false;
            if (libroActual != null)
            {
                isValidNumber = libroActual.isValidNumberHoja(numeroDeHoja);
            }
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
        /// <summary>
        /// Given the text of a hoja ir calculates
        /// the new largo of the mainWindow text box and returns it
        /// </summary>
        /// <param name="rawHoja">Full text of a hoja of a libro</param>
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
        /// <summary>
        /// Takes the text of a hoja in the mainWindow text box
        /// calculetes the largo of the text box and returns it.
        /// </summary>
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
        private List<string> getPagesRange(Data.printInput input)
        {
            var response = new List<string>();
            var valuesParamsQuery = new List<string> { input.finalPrintRange[0].ToString(), input.finalPrintRange[1].ToString() };
            var paramsQuery = new List<string> { dQuerys.oneParam, dQuerys.oneParam };
            response = theMDBConexion.getSimple(dQuerys.getAllHojaRange, libroActual.path, paramsQuery, valuesParamsQuery, dQuerys.fieldHojaText);
            return response;
        }
        /// <summary>
        /// Given two numbers checks if they are whole numbers.
        /// Then if they are positive and less than the max number of pages.
        /// Then checks if the first is equal or less than the end Number.
        /// If passes all those checks then returns a List<int> with the first and end number.
        /// </summary>
        /// <param name="firstNumber">First whole number</param>
        /// <param name="endNumber">End whole number</param>
        /// <returns></returns>
        private List<int> getPrintRage(string firstNumber, string endNumber)
        {
            var response = new List<int>();
            var startNumberInt = 0;
            var endNumberInt = 0;

            var isIntStarting = Int32.TryParse(firstNumber, out startNumberInt);
            var isIntEnding = Int32.TryParse(endNumber, out endNumberInt);

            var isInRangeStart = false;
            var isInRangeEnd = false;
            var areInDistance = false;
            if (isIntStarting && isIntEnding && libroActual != null)
            {
                if(startNumberInt > 0 && startNumberInt <= libroActual.numeroHojaMaxima)
                {
                    isInRangeStart = true;
                }
                if (endNumberInt > 0 && endNumberInt <= libroActual.numeroHojaMaxima)
                {
                    isInRangeEnd = true;
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
        /// Close application.
        /// </summary>
        private void CommandBinding_Executed_5(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        /// <summary>
        /// Highlights the matching text in the richTextBox
        /// </summary>
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
