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
    /// Interaction logic for inputNumberPage.xaml
    /// </summary>
    public partial class inputNumberPage : Window
    {
        public string InputText { get; private set; }
        public inputNumberPage(string text, string aTitle)
        {
            messageText.Text = text;
            Title = aTitle;
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
