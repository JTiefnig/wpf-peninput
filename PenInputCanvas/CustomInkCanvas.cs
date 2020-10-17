using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace PenInputCanvas
{
    public class CustomInkCanvas : InkCanvas
    {

        public CustomInkCanvas() : base()
        {
            // Use the custom dynamic renderer on the
            // custom InkCanvas.
            this.DynamicRenderer = new CustomDynamicRenderer(this);

            cx = this.ActualWidth / 2;
            cy = this.ActualHeight / 2;
        }



        public double cx { get; set; }
        public double cy { get; set; }


        public bool LiveRender { get; set; } = false;


        private int _multiplier = 3;
        public int Multiplier
        {
            get => _multiplier;
            set
            {
                _multiplier = value;
                UpdateDeviderLines();

            }
        }


        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);


            var oldcx = cx;
            var oldcy = cy;



            cx = this.ActualWidth / 2;
            cy = this.ActualHeight / 2;


            var mat = new Matrix();
            mat.Translate(cx - oldcx, cy - oldcy);

            foreach (var str in this.Strokes)
            {
                str.Transform(mat, false);
            }


            this.UpdateDeviderLines();
        }

        protected override void OnStrokeCollected(InkCanvasStrokeCollectedEventArgs e)
        {
            base.OnStrokeCollected(e);

            

            var ns = e.Stroke;

            var mat = new Matrix();



            int teiler = (int)Math.Pow(2, this.Multiplier);

            var cx = this.cx;
            var cy = this.cy;


            for (int i = 0; i < teiler; i++)
            {
                ns = ns.Clone();

                mat.RotateAt(360.0 / teiler, cx, cy);

                ns.Transform(mat, false);

                this.Strokes.Add(ns);
            }



            ns = ns.Clone();
            mat.ScaleAt(-1, 1, cx, cy);
            ns.Transform(mat, false);
            this.Strokes.Add(ns);



            mat = new Matrix();

            for (int i = 0; i < teiler; i++)
            {
                ns = ns.Clone();
                mat.RotateAt(360.0 / teiler, cx, cy);

                ns.Transform(mat, false);

                this.Strokes.Add(ns);

            }  

        }



        private void UpdateDeviderLines()
        {
            this.Children.Clear();

            var cx = this.cx;
            var cy = this.cy;

            var linebrush = new SolidColorBrush() { Color = Colors.White, Opacity = 0.1 };

            int teiler = (int)Math.Pow(2, Multiplier);

            for (int i = 0; i <= teiler / 2; i++)
            {

                var angle = (2 * Math.PI / teiler) * i + Math.PI / 2;

                var line = new System.Windows.Shapes.Line();

                line.IsHitTestVisible = false;
                line.Visibility = System.Windows.Visibility.Visible;
                line.StrokeThickness = 1;
                line.Stroke = linebrush;

                line.X1 = cx + Math.Cos(angle) * (cx + cy);
                line.X2 = cx - Math.Cos(angle) * (cx + cy);
                line.Y1 = cy + Math.Sin(angle) * (cx + cy);
                line.Y2 = cy - Math.Sin(angle) * (cx + cy);


                this.Children.Add(line);


            }



        }




        public void SaveToBitmap(String file)
        {
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)this.ActualWidth, (int)this.ActualHeight, 96d, 96d, PixelFormats.Default);
            rtb.Render(this);
            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            using (FileStream fs = File.Open(file, FileMode.Create))
            {
                encoder.Save(fs);
                fs.Close();
            }
               
        }




        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if(this.ActiveEditingMode == InkCanvasEditingMode.Ink)
            {
                // maby
            }

        }



    }
}
