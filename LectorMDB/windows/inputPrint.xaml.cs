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

namespace LectorMDB.windows
{
    /// <summary>
    /// Interaction logic for inputPrint.xaml
    /// </summary>
    public partial class inputPrint : Window
    {
        public Data.printInput theInput { get; private set; }
        public inputPrint(Data.printData theData)
        {
            InitializeComponent();
            tipoHoja.ItemsSource = theData.tiposHojas;
            orientacionHoja.ItemsSource = theData.orientaciones;
            fontPrint.ItemsSource = theData.fontSizes;

            tipoHoja.SelectedIndex = tipoHoja.Items.Count - 1;
            orientacionHoja.SelectedIndex = 0;
            fontPrint.SelectedIndex = 2;

            theInput = new Data.printInput();
            theInput.setVariables(theData.tiposHojas, theData.orientaciones, theData.fontSizes);
        }

        private void printEverything_Click(object sender, RoutedEventArgs e)
        {
            
            theInput.setSizePage(tipoHoja.SelectedValue.ToString());
            theInput.setOrientacion(orientacionHoja.SelectedValue.ToString());
            theInput.setSizeLetter(fontPrint.SelectedValue.ToString());
            theInput.setStartNumber(printNumeroHojaUno.ToString());
            theInput.setEndNumber(printNumeroHojaDos.ToString());
            DialogResult = true;
            Close();
        }
    }
}
