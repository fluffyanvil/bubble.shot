using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using BubbleShot.UniversalApp.Converters;
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

	    private double _x1, _x2, _y1, _y2;

	    private void OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
	    {
		    _x1 = e.Position.X;
		    _y1 = e.Position.Y;
	    }

		private void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
		{
			_x2 = e.Position.X;
			_y2 = e.Position.Y;
			if (_x1 > _x2 && Math.Abs(_y1 - _y2) < 50)
			{
				ViewModel.NextVictimCommand.Execute(null);
				return;
			}
			if (_x1 < _x2 && Math.Abs(_y1 - _y2) < 50)
			{
				ViewModel.PrevVictimCommand.Execute(null);
				return;
			}

			if (_y2 - _y1 > 50)
			{
				ViewModel.CLoseDetails.Execute(null);
			}
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
	    {

		    base.OnNavigatedTo(e);
			Map.Focus(FocusState.Keyboard);
			ViewModel.GetLocationEvent += GetLocationEvent;
			ViewModel.RadiusChangedEvent += ViewModelOnRadiusChangedEvent;
			ViewModel.GoToAddressEvent += ViewModelOnGoToAddressEvent;
			ViewModel.GetPosition();
	    }

	    private async void ViewModelOnGoToAddressEvent(Geopoint geopoint)
	    {
		    await Map.TrySetViewAsync(geopoint);
	    }

	    private void ViewModelOnRadiusChangedEvent()
	    {
		    MarkAndDrawCircle(Map, ViewModel.Radius);
	    }

	    private async void GetLocationEvent()
	    {
		    await Map.TrySetViewAsync(new Geopoint(new BasicGeoposition() {Latitude = ViewModel.Geoposition.Coordinate.Latitude, Longitude = ViewModel.Geoposition.Coordinate.Longitude}));
		    await Map.TryZoomToAsync(11);
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
			//var icon = new MapIcon { Location = Map.Center};

			//Map.MapElements.Add(icon);
			var fill = Colors.LawnGreen;
			var stroke = Colors.Green;
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

	    private void MainPage_OnSizeChanged(object sender, SizeChangedEventArgs e)
	    {
		    var isLandscape = e.NewSize.Width > e.NewSize.Height;
			ViewModel.AvailableModalSize = isLandscape ? e.NewSize.Height - 125 : e.NewSize.Width - 125;
	    }
		private async void AutoSuggestBox_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
	    {
			if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
			{
				var result = await GetPositionFromAddressAsync(sender.Text);
				sender.ItemsSource = result.ToList();
			}
		}

		private async Task<IList<MapLocation>> GetPositionFromAddressAsync(string address)
		{
			if (string.IsNullOrEmpty(address))
				return new List<MapLocation>();
			var mapLocationFinderResult = await MapLocationFinder.FindLocationsAsync(address, Map.Center, 10);
			return mapLocationFinderResult.Status == MapLocationFinderStatus.Success ? mapLocationFinderResult.Locations.ToList() : new List<MapLocation>();
		}

	    private async void AutoSuggestBox_OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
	    {
		    var chosen = args.ChosenSuggestion as MapLocation;
		    ViewModel.SearchedLocation = chosen;
		    if (chosen != null) await Map.TrySetViewAsync(chosen.Point);
			MarkAndDrawCircle(Map, ViewModel.Radius);
		}

	    private void AutoSuggestBox_OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
	    {
		    var mapLocation = args.SelectedItem as MapLocation;
		    if (mapLocation != null)
			    sender.Text = mapLocation.Address.FormattedAddress;
	    }

	    private void WrapGridSizeChanged(object sender, SizeChangedEventArgs e)
	    {
			var isLandscape = e.NewSize.Width > e.NewSize.Height;
			ViewModel.DynamicPhotoSize = isLandscape ? e.NewSize.Width / 10 : e.NewSize.Width / 5;
		    ViewModel.MaximumColumns = (int) (e.NewSize.Width/ViewModel.DynamicPhotoSize);
	    }
    }
}
