using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Media;

namespace PenInputCanvas
{
    class CustomDynamicRenderer : DynamicRenderer
    {
        [ThreadStatic]
        static private Brush brush = null;

        [ThreadStatic]
        static private Pen pen = null;

        private Point prevPoint;

        public CustomDynamicRenderer(CustomInkCanvas canvas) : base()
        {
            InkCanvas = canvas;
        }


        protected CustomInkCanvas InkCanvas { get;}

        protected override void OnStylusDown(RawStylusInput rawStylusInput)
        {
            // Allocate memory to store the previous point to draw from.
            prevPoint = new Point(double.NegativeInfinity, double.NegativeInfinity);
            base.OnStylusDown(rawStylusInput);
        }

        protected override void OnDraw(DrawingContext drawingContext,
                                       StylusPointCollection stylusPoints,
                                       Geometry geometry, Brush fillBrush)
        {


            //var pointcol = new PointCollection()
            //{

            //}


            //var poy = new PolyBezierSegment() { Points =  };



            var str = new Stroke(stylusPoints);

            var mat = new Matrix();

            int teiler = (int)Math.Pow(2, InkCanvas.Multiplier);

            
            
            str.Draw(drawingContext, this.DrawingAttributes);

            if (!InkCanvas.LiveRender)
                return;

            var cx = InkCanvas.cx;
            var cy = InkCanvas.cy;


            for (int i = 0; i < teiler; i++)
            {


                mat.RotateAt(360.0 / teiler, cx, cy);
                str = str.Clone();
                str.Transform(mat, false);

                str.Draw(drawingContext, this.DrawingAttributes);
            }



            str = str.Clone();
            mat.ScaleAt(-1, 1, cx, cy);
            str.Transform(mat, false);
            str.Draw(drawingContext, this.DrawingAttributes);
            


            mat = new Matrix();

            for (int i = 0; i < teiler; i++)
            {

                mat.RotateAt(360.0 / teiler, cx, cy);
                str = str.Clone();
                str.Transform(mat, false);

                str.Draw(drawingContext, this.DrawingAttributes);

            }

        }
    }
}
