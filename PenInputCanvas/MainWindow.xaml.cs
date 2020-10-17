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
using Microsoft.Ink;
using Microsoft.Win32;

namespace PenInputCanvas
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            var inkDA =   this.theInkCanvas.DefaultDrawingAttributes;
        }

  
        private void CearButton_Click(object sender, RoutedEventArgs e)
        {
            this.theInkCanvas.Strokes.Clear();
         }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Bitmap (*.bmp)|*.bmp";
            if (saveFileDialog.ShowDialog() == true)
                this.theInkCanvas.SaveToBitmap(saveFileDialog.FileName);

        }
    }
}
