using System;
using System.Collections.Generic;
using Windows.Devices.Geolocation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Navigation;
using BubbleShot.UniversalApp.Extensions;
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
            InitializeComponent();
	        ApplyTemplate();
        }

	    protected override void OnNavigatedTo(NavigationEventArgs e)
	    {

		    base.OnNavigatedTo(e);
			Map.Focus(FocusState.Keyboard);
			ViewModel.GetLocationEvent += GetLocationEvent;
			ViewModel.RadiusChangedEvent += ViewModelOnRadiusChangedEvent;
			ViewModel.GetPosition();
	    }

	    private void ViewModelOnRadiusChangedEvent()
	    {
		    MarkAndDrawCircle(Map, ViewModel.Radius);
	    }

	    private async void GetLocationEvent()
	    {
		    await Map.TrySetViewAsync(new Geopoint(new BasicGeoposition() {Latitude = ViewModel.Geoposition.Coordinate.Latitude, Longitude = ViewModel.Geoposition.Coordinate.Longitude}));
		    await Map.TryZoomToAsync(18);
			MarkAndDrawCircle(Map, ViewModel.Radius);
		}

	    private MainPageViewModel ViewModel => (MainPageViewModel) DataContext;

	    private void Map_OnCenterChanged(MapControl sender, object args)
	    {
		    var geopoint = Map.Center;
		    ViewModel.Location = geopoint;
	    }

	    private async void Map_OnMapDoubleTapped(MapControl sender, MapInputEventArgs args)
	    {
		    await Map.TrySetViewAsync(args.Location);
			MarkAndDrawCircle(Map, ViewModel.Radius);
	    }

	    private void MarkAndDrawCircle(MapControl mapControl, int radius)
	    {
		    mapControl.MapElements.Clear();
			var icon = new MapIcon { Location = Map.Center};
			Map.MapElements.Add(icon);
			var fill = Colors.Purple;
			var stroke = Colors.Red;
			fill.A = 80;
			stroke.A = 100;
		    var circle = new MapPolygon
		    {
			    StrokeThickness = 2,
			    FillColor = fill,
			    StrokeColor = stroke,
			    StrokeDashed = false,
			    Path = new Geopath(mapControl.Center.GetCirclePoints(radius))
		    };


		    mapControl.MapElements.Add(circle);
		}
	}
}
