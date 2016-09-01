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
			get { return _zoomLevel; }
			set
			{
				_zoomLevel = value;
				OnPropertyChanged();
				OnRaiseNeedToRedrawCircle?.Invoke(this, null);
			}
		}
		public bool ShowOnMap
		{
			get { return _showOnMap; }
			set
			{
				_showOnMap = value;
				OnPropertyChanged();
			}
		}

		public Geopoint MapCenterGeopoint
		{
			get { return _mapCenterGeopoint; }
			set
			{
				_mapCenterGeopoint = value;
				OnPropertyChanged();
				ZoomLevel = 11;
			}
		}

		public Geopoint SelectionRadiusGeopoint
		{
			get { return _selectionRadiusGeopoint; }
			set
			{
				_selectionRadiusGeopoint = value;
				OnPropertyChanged();
				SelectionAreaCirclePath = new Geopath(SelectionRadiusGeopoint.GetCirclePoints(Radius));
			}
		}
		public Geopath SelectionAreaCirclePath
		{
			get { return _selectionAreaCirclePath; }
			set
			{
				_selectionAreaCirclePath = value;
				OnPropertyChanged();
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
			get { return _radius; }
			set
			{
				_radius = value; OnPropertyChanged();
				SelectionAreaCirclePath = new Geopath(SelectionRadiusGeopoint.GetCirclePoints(Radius));
			}
		}

		public string SelectionAddress
		{
			get { return _selectionAddress; }
			set { _selectionAddress = value; OnPropertyChanged(); }
		}

		public string SearchAddress
		{
			get { return _searchAddress; }
			set
			{
				_searchAddress = value;
				OnPropertyChanged();
				GetAddresses();
			}
		}

		public IEnumerable<MapLocation> SearchedMapLocations
		{
			get
			{
				return _searchedMapLocations;
			}
			set
			{
				_searchedMapLocations = value;
				OnPropertyChanged();
			}
		}

		private async void GetAddresses()
		{
			SearchedMapLocations = await GeocodingHelper.GetMapLocationByAddressString(SearchAddress, MapCenterGeopoint);
		}

		public bool IsSearchingLocation
		{
			get { return _isSearchingLocation; }
			set
			{
				_isSearchingLocation = value;
				OnPropertyChanged();
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
