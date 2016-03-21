using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using BubbleShot.UniversalApp.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BubbleShot.UniversalApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

	    protected override void OnApplyTemplate()
	    {
		    Map.Focus(FocusState.Keyboard);
		    base.OnApplyTemplate();
	    }

	    private MainPageViewModel ViewModel => (MainPageViewModel) DataContext;

	    private void Map_OnCenterChanged(MapControl sender, object args)
	    {
		    var geopoint = Map.Center;
		    ViewModel.Location = geopoint;
	    }

	    private void Map_OnMapDoubleTapped(MapControl sender, MapInputEventArgs args)
	    {
		    
	    }
    }
}
