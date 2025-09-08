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
    /// Interaction logic for inputBuscar.xaml
    /// </summary>
    public partial class inputBuscar : Window
    {
        public string InputText { get; private set; }
        public inputBuscar()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            InputText = txtInput.Text;
            DialogResult = true;
            Close();
        }
    }
}
