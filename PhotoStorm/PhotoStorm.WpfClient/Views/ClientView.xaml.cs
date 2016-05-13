using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BubbleShot.WpfClient.ViewModels;
using Microsoft.Maps.MapControl.WPF;

namespace BubbleShot.WpfClient.Views
{
	/// <summary>
	/// Interaction logic for ClientView.xaml
	/// </summary>
	public partial class ClientView : UserControl
	{
		ClientViewModel ViewModel => (ClientViewModel)DataContext;

		private void ViewModelOnLocationChangedEvent()
		{
			ChangeMapPosition(ViewModel.Latitude, ViewModel.Longitude);
		}
		public ClientView()
		{
			InitializeComponent();
			Map.Focus();
			Map.ViewChangeOnFrame += Target;
		}

		public override void OnApplyTemplate()
		{
			ViewModel.LocationChangedEvent += ViewModelOnLocationChangedEvent;
		}

		private void Target(object sender, MapEventArgs mapEventArgs)
		{
			// Gets the map object that raised this event.
			var map = sender as Map;
			// Determine if we have a valid map object.
			if (map != null)
			{
				// Gets the center of the current map view for this particular frame.
				var mapCenter = map.Center;

				// Updates the latitude and longitude values, in real time,
				// as the map animates to the new location.
				ViewModel.FromView = true;
				ViewModel.Latitude = mapCenter.Latitude;
				ViewModel.Longitude = mapCenter.Longitude;
				ViewModel.FromView = false;
			}
		}

		private void ChangeMapPosition(double latitude, double longitude)
		{
			var center = new Location(latitude, longitude);
			var zoom = Map.ZoomLevel;
			// Set the map view
			Map.SetView(center, zoom);
		}

		public static readonly DependencyProperty RemoveClientCommandProperty = DependencyProperty.Register(
			"RemoveClientCommand", typeof (ICommand), typeof (ClientView), new PropertyMetadata(default(ICommand)));

		public ICommand RemoveClientCommand
		{
			get { return (ICommand) GetValue(RemoveClientCommandProperty); }
			set { SetValue(RemoveClientCommandProperty, value); }
		}

		private void CloseClientInstanceButtonClick(object sender, RoutedEventArgs e)
		{
			RemoveClientCommand?.Execute(this);
		}
	}
}
