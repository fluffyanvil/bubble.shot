using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls.Maps;
using PhotoStorm.UniversalApp.Extensions;
using PhotoStorm.UniversalApp.Helpers;
using Prism.Commands;

namespace PhotoStorm.UniversalApp.ViewModels
{
	public class MapPivotItemViewModel : BaseViewModel
	{
        public event EventHandler OnRaiseNeedToRedrawCircle;
		public int ZoomLevel
		{
			get => _zoomLevel;
			set
			{
				_zoomLevel = value;
				RaisePropertyChanged();
				OnRaiseNeedToRedrawCircle?.Invoke(this, null);
			}
		}
		public bool ShowOnMap
		{
			get => _showOnMap;
			set
			{
				_showOnMap = value;
				RaisePropertyChanged();
			}
		}

		public Geopoint MapCenterGeopoint
		{
			get => _mapCenterGeopoint;
			set
			{
				_mapCenterGeopoint = value;
				RaisePropertyChanged();
				ZoomLevel = 11;
			}
		}

		public Geopoint SelectionRadiusGeopoint
		{
			get => _selectionRadiusGeopoint;
			set
			{
				_selectionRadiusGeopoint = value;
				RaisePropertyChanged();
				SelectionAreaCirclePath = new Geopath(SelectionRadiusGeopoint.GetCirclePoints(Radius));
			}
		}
		public Geopath SelectionAreaCirclePath
		{
			get => _selectionAreaCirclePath;
			set
			{
				_selectionAreaCirclePath = value;
				RaisePropertyChanged();
				OnRaiseNeedToRedrawCircle?.Invoke(this, null);
			}
		}
		public ICommand SearchLocationCommand => _searchLocationCommand ?? (_searchLocationCommand = new DelegateCommand<Geopoint>(OnExecuteSearchLocationCommand));

		private async void OnExecuteSearchLocationCommand(Geopoint point)
		{
		    IsSearchingLocation = true;
			MapCenterGeopoint = point;
			SelectionRadiusGeopoint = point;
			SelectionAddress = await GeocodingHelper.GetAddressByCoordinates(point);
		    IsSearchingLocation = false;
		}

		public ICommand MapDoubleTappedCommand => _mapDoubleTappedCommand ??
												  (_mapDoubleTappedCommand = new DelegateCommand<MapInputEventArgs>(OnExecuteMapDoubleTappedCommand));

		private async void OnExecuteMapDoubleTappedCommand(MapInputEventArgs mapInputEventArgs)
		{
			MapCenterGeopoint = mapInputEventArgs.Location;
			SelectionRadiusGeopoint = mapInputEventArgs.Location;
			SelectionAddress = await GeocodingHelper.GetAddressByCoordinates(mapInputEventArgs.Location);
		}

		public int Radius
		{
			get => _radius;
			set
			{
				_radius = value;
				RaisePropertyChanged();
				SelectionAreaCirclePath = new Geopath(SelectionRadiusGeopoint.GetCirclePoints(Radius));
			}
		}

		public string SelectionAddress
		{
			get => _selectionAddress;
			set
			{
				_selectionAddress = value;
				RaisePropertyChanged();
			}
		}

		public string SearchAddress
		{
			get => _searchAddress;
			set
			{
				_searchAddress = value;
				RaisePropertyChanged();
				GetAddresses();
			}
		}

		public IEnumerable<MapLocation> SearchedMapLocations
		{
			get => _searchedMapLocations;
			set
			{
				_searchedMapLocations = value;
				RaisePropertyChanged();
			}
		}

		private async void GetAddresses()
		{
			SearchedMapLocations = await GeocodingHelper.GetMapLocationByAddressString(SearchAddress, MapCenterGeopoint);
		}

		public bool IsSearchingLocation
		{
			get => _isSearchingLocation;
			set
			{
				_isSearchingLocation = value;
				RaisePropertyChanged();
			}
		}
		#region Public Methods
		public MapPivotItemViewModel()
		{
			_geolocator = new Geolocator();
			Radius = 5000;
		}
		public async Task GetUserLocation()
		{
			try
			{
				IsSearchingLocation = true;
				var geoposition = await _geolocator.GetGeopositionAsync();
				MapCenterGeopoint = geoposition.Coordinate.Point;
				SelectionRadiusGeopoint = geoposition.Coordinate.Point;
				SelectionAddress =
					await
						GeocodingHelper.GetAddressByCoordinates(geoposition.Coordinate.Point);
			}
			catch (Exception)
			{
				var dialog = new MessageDialog("Надо было разрешить");
				await dialog.ShowAsync();
			}
			IsSearchingLocation = false;
		}
		#endregion

		#region Private Fields
		private int _radius;
		private string _searchAddress;
		private bool _showOnMap;
		private int _zoomLevel;
		private string _selectionAddress;
		private Geopoint _mapCenterGeopoint;
		private readonly Geolocator _geolocator;
		private IEnumerable<MapLocation> _searchedMapLocations;
		private DelegateCommand<Geopoint> _searchLocationCommand;
		private Geopath _selectionAreaCirclePath;
		private ICommand _mapDoubleTappedCommand;
		private Geopoint _selectionRadiusGeopoint;
		private bool _isSearchingLocation;

		#endregion
	}
}
