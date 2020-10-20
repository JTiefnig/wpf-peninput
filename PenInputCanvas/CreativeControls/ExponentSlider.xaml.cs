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

            var pos = expslid.ActualWidth / expslid.MaximumExponent * expslid.Value;


            // todo storryboard animation
            TranslateTransform transform = new TranslateTransform(pos, 0);

            expslid.Slider.RenderTransform = transform;

            expslid.ExpontentText.Text = expslid.Value.ToString();

        }





        #endregion



    }
}
