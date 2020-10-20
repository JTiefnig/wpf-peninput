using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using PenInputCanvas.ViewModel;

namespace PenInputCanvas
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var app = new AppViewModel(); // possible to pass in arguments herer...

            var win = new MainWindow(app);

            win.Show();
        }


    }
}
