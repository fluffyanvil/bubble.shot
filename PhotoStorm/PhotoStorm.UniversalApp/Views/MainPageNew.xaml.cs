using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PhotoStorm.UniversalApp.ViewModels;
using WinRTXamlToolkit.Controls.Extensions;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PhotoStorm.UniversalApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPageNew : Page
    {
        private MainPageViewModel ViewModel => (MainPageViewModel)DataContext;
        public MainPageNew()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.OnRaiseNeedToRedrawCircle += ViewModelOnOnRaiseNeedToRedrawCircle;
        }

        private void ViewModelOnOnRaiseNeedToRedrawCircle(object sender, EventArgs eventArgs)
        {
            try
            {
                MapView.RedrawCircle(ViewModel.SelectionAreaCirclePath);
            }
            catch (Exception)
            {
                
            }
        }
    }
}
