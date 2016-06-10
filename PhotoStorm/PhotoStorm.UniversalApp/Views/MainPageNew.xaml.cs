using System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using PhotoStorm.UniversalApp.ViewModels;

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
            ViewModel.MapPivotItemViewModel.OnRaiseNeedToRedrawCircle += ViewModelOnOnRaiseNeedToRedrawCircle;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs backRequestedEventArgs)
        {
            try
            {
                if (backRequestedEventArgs.Handled) return;

                
                if (ViewModel.GalleryPivotItemViewModel.DetailsIsVisible)
                {
                    ViewModel.GalleryPivotItemViewModel.CloseDetails.Execute(null);
                    backRequestedEventArgs.Handled = true;
                    return;
                }

                if (ViewModel.SelectedPivotIndex != 0)
                {
                    ViewModel.SelectedPivotIndex = 0;
                    backRequestedEventArgs.Handled = true;
                }
            }
            catch (Exception ex)
            {
                
            }
            
        }

        private void ViewModelOnOnRaiseNeedToRedrawCircle(object sender, EventArgs eventArgs)
        {
            try
            {
                MapView.RedrawCircle(ViewModel.MapPivotItemViewModel.SelectionAreaCirclePath);
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
