using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using PenInputCanvas.CreativeControls;

namespace PenInputCanvas.ViewModel
{
    public class AppViewModel : ViewModel
    {


        public AppViewModel()
        {

        }



        public void SaveCanvasToFile(CustomInkCanvas canvas)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Bitmap (*.bmp)|*.bmp";
            if (saveFileDialog.ShowDialog() == true)
                canvas.SaveToBitmap(saveFileDialog.FileName);
        }




        #region public properties


        public ICommand SaveCommand => new RelayCommand<CustomInkCanvas>(SaveCanvasToFile);

        public ICommand NewCanvasCommand => 
            new RelayCommand<CustomInkCanvas> (
            (CustomInkCanvas canv) => canv.Strokes.Clear()
            );



        #endregion


    }
}
