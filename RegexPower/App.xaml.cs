using RegexPower.ViewModel;
using System;
using System.Diagnostics;
using System.Windows;

namespace RegexPower
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                new MainViewModel();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
