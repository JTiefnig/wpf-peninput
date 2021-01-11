using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PenInputCanvas.CreativeControls
{
    /// <summary>
    /// Interaktionslogik für ExponentSlider.xaml
    /// </summary>
    public partial class ExponentSlider : UserControl
    {
        public ExponentSlider()
        {
            InitializeComponent();

            CreateTicks();


            // initialize all eventhaldlers 
            // could do this in a serperate method

            this.SliderBar.MouseLeftButtonDown += 
                (object sender, MouseButtonEventArgs e) => 
                this.SetValue(e.GetPosition(sender as UIElement).X);


            this.SliderBar.TouchDown += new EventHandler<TouchEventArgs>(
                (object sender, TouchEventArgs e) => this.SetValue(e.GetTouchPoint(sender as UIElement).Position.X)
                );


            this.Slider.TouchDown += new EventHandler<TouchEventArgs>((object sender, TouchEventArgs e) => this.isDragging = true);
                
            var touchmove  = new EventHandler<TouchEventArgs>((object sender, TouchEventArgs e) => 
            {
                if (isDragging)
                    this.SetValue(e.GetTouchPoint(this.SliderBar).Position.X);
            });

            this.Slider.TouchMove += touchmove;
            this.SliderBar.TouchMove += touchmove;

            this.Slider.TouchUp += new EventHandler<TouchEventArgs>((object sender, TouchEventArgs e) => this.isDragging = false);

            this.Slider.MouseDown += (object sender, MouseButtonEventArgs e) => this.isDragging = true;

            this.Slider.MouseMove += (object sender, MouseEventArgs e) =>
            {
                if (isDragging)
                    this.SetValue(e.GetPosition(this.SliderBar).X);
            };

            this.Slider.MouseUp +=  (object sender, MouseButtonEventArgs e) => this.isDragging = false;
            this.Slider.MouseLeave += (object sender, MouseEventArgs e) => this.isDragging = false;

            this.Loaded += (object sender, RoutedEventArgs a) => MoveSliderToPosition(); 
        }

        /// <summary>
        /// Indicates if slider is currently dragged
        /// </summary>
        private bool _isdragging = false;
        public bool isDragging {
            get =>_isdragging;
            private set
            {
                // if dragging ends -> move slider to position
                if (_isdragging && !value)
                    MoveSliderToPosition();
                _isdragging = value;
            }
        }

        /// <summary>
        /// Setting the value by
        /// </summary>
        /// <param name="pos"></param>
        private void SetValue(double pos)
        {
            // calculation relative screen position to expontent
            var val = Math.Floor(pos / this.SliderBar.ActualWidth * this.MaximumExponent)+1;

            // clamp value
            val = (val > MaximumExponent) ? MaximumExponent : val;
            val = (val < 1) ? 1 : val;

            this.Value = (uint)val;

            if (isDragging)
                SetSliderPosition(pos);

        }
        
        /// <summary>
        /// Moves slider to given X Positon relative to slider bar
        /// </summary>
        /// <param name="pos"></param>
        private void SetSliderPosition(double pos)
        {
            var maxmove = (this.SliderBar.ActualWidth - Slider.ActualWidth)/2;

            // adjust for offset center
            var spos = pos - this.SliderBar.ActualWidth / 2;

            //clamp value
            spos = (spos > maxmove) ? maxmove : spos;
            spos = (spos < -maxmove) ? -maxmove : spos;

            SliderPositionTransform.X = spos;
        }


        /// <summary>
        /// Positions the Slider according to the Value property
        /// </summary>
        private void MoveSliderToPosition()
        {
            var maxmove = this.SliderBar.ActualWidth;
            var screenpos = (maxmove * (((double)this.Value - 0.5) / MaximumExponent - 0.5));
            
           

            DoubleAnimationUsingKeyFrames animation = new DoubleAnimationUsingKeyFrames();
            animation.KeyFrames.Add(
                new SplineDoubleKeyFrame(
                    screenpos, // Target value (KeyValue)
                    KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(400)), // KeyTime
                    new KeySpline(0.1, 0.9, 0.2, 1) // KeySpline
                    ));

            // end Animation after set duration
            animation.FillBehavior = FillBehavior.Stop;

            // start animation
            this.SliderPositionTransform.BeginAnimation(TranslateTransform.XProperty, animation);

            // Explicitly set position after animation is finished
            SliderPositionTransform.X = screenpos;
        }


        /// <summary>
        /// Defines the maximum value of the slider
        /// </summary>
        private uint _maximumExponent = 4;
        public uint MaximumExponent
        {
            get => _maximumExponent;
            set
            {
                _maximumExponent = value;
                this.CreateTicks();
            }
        }

        /// <summary>
        /// The background
        /// </summary>
        private void CreateTicks()
        {
            this.SliderBarItemContainer.Items.Clear();

            for (uint i = 1; i <= MaximumExponent; i++)
            {
                var bt = new TextBlock() { Text = i.ToString() };

                SliderBarItemContainer.Items.Add(bt);
            }
        }


        #region Dependency Property

        public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register(nameof(Value),
        typeof(uint),
        typeof(ExponentSlider),
        new FrameworkPropertyMetadata((uint)1, new PropertyChangedCallback(ValuePropertyChanged)));


        /// <summary>
        /// Set Value of the Expontent, similar to Slider Value
        /// </summary>
        public uint Value
        {
            get => (uint)GetValue(ValueProperty);
            set
            {
                SetValue(ValueProperty, value);
            }
        }


        public static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            var expslid = d as ExponentSlider;

            if(!expslid.isDragging)
                expslid.MoveSliderToPosition();

            expslid.ExpontentText.Text = expslid.Value.ToString();
        }

        #endregion



    }
}
