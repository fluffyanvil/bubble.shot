using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media.Imaging;
using PhotoStorm.Core.Portable.Adapters.EventArgs;
using PhotoStorm.Core.Portable.Adapters.Instagram;
using PhotoStorm.Core.Portable.Adapters.Manager;
using PhotoStorm.Core.Portable.Adapters.Rules;
using PhotoStorm.Core.Portable.Adapters.Vkontakte;
using PhotoStorm.Core.Portable.Common.Models;
using PhotoStorm.UniversalApp.Extensions;
using PhotoStorm.UniversalApp.Models;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

namespace PhotoStorm.UniversalApp.ViewModels
{
	public class MainPageViewModel : ViewModelBase
	{
	    private DelegateCommand _startManagerCommand;
		private DelegateCommand _stopManagerCommand;
	    private int _radius;
	    private PhotoWithUserLink _selectedItem;
		private DelegateCommand _cLoseDetails;
		private readonly Geolocator _geolocator;
	    private double _availableModalSize;
		private Geopoint _mapCenterGeopoint;
		private string _searchAddress;
	    private DelegateCommand _showLinkCommand;
		private bool _isShowLink;
		private double _dynamicPhotoSize;
		private int _maximumColumns;

		private readonly IAdapterManager _adapterManager;
		private DelegateCommand<object> _removeItemCommand;
		private DelegateCommand _removeAllItemsCommand;
		private bool _showOnMap;
	    private List<MapLocation> _searchedLocations;
	    private DelegateCommand<Geopoint> _searchLocationCommand;
	    private int _zoomLevel;
	    private Geopath _selectionAreaCirclePath;
	    private ICommand _adaptWindowSizeCommand;
	    private ICommand _adaptWrapGridSizeCommand;
	    private ICommand _mapDoubleTappedCommand;
	    private Geopoint _selectionRadiusGeopoint;
	    private bool _detailsIsVisible;
	    private ICommand _showDetails;
	    private string _selectionAddress;
	    private int _selectedPivotIndex;

	    public event EventHandler OnRaiseNeedToRedrawCircle;

	    #region Commands

		public ICommand StopManagerCommand => _stopManagerCommand ?? (_stopManagerCommand = new DelegateCommand(OnExecuteStopManagerCommand, CanExecuteStopManagerCommand));

		private bool CanExecuteStopManagerCommand()
		{
			return _adapterManager.CanStop;
		}

		private void OnExecuteStopManagerCommand()
		{
			_adapterManager.Stop();
			UpdateCommandAvailability();
		}

		public ICommand StartManagerCommand => _startManagerCommand ?? (_startManagerCommand = new DelegateCommand(OnExecuteStartManager, CanExecuteStartManager));

		private bool CanExecuteStartManager()
		{
			return _adapterManager.CanStart;
		}

		private IAdapterRule AdapterRule => new AdapterRule()
		{
			Latitude = MapCenterGeopoint.Position.Latitude,
			Longitude = MapCenterGeopoint.Position.Longitude,
			Radius = Radius
		};

		private void OnExecuteStartManager()
		{
			_adapterManager.Start(AdapterRule);
			UpdateCommandAvailability();
		}

		private void UpdateCommandAvailability()
		{
			_startManagerCommand.RaiseCanExecuteChanged();
			_stopManagerCommand.RaiseCanExecuteChanged();
		}

		public ICommand RemoveItemCommand => _removeItemCommand ??
		                                     (_removeItemCommand = new DelegateCommand<object>(OnExecuteRemoveItemCommand));

		private void OnExecuteRemoveItemCommand(object parameter)
		{
			var photo = parameter as PhotoWithUserLink;
			if (SelectedItem != photo)
			{
				Photos.Remove(photo);
			}
		}

		public ICommand RemoveAllItemsCommand => _removeAllItemsCommand ?? (_removeAllItemsCommand = new DelegateCommand(OnExecuteRemoveAllItemsCommand));

		private void OnExecuteRemoveAllItemsCommand()
		{
			SelectedItem = null;
			Photos.Clear();
		}

		public ICommand ShowLinkCommand => _showLinkCommand ?? (_showLinkCommand = new DelegateCommand(OnExecuteShowLinkCommand, CanExecuteShowLinkCommand));

	    public ICommand AdaptWindowSizeCommand => _adaptWindowSizeCommand ?? (_adaptWindowSizeCommand = new DelegateCommand<SizeChangedEventArgs>(OnExecuteAdaptWindowSizeCommand));

	    public ICommand AdaptWrapGridSizeCommand => _adaptWrapGridSizeCommand ?? (_adaptWrapGridSizeCommand = new DelegateCommand<SizeChangedEventArgs>(OnExecuteAdaptWrapGridSizeCommand));

	    private void OnExecuteAdaptWrapGridSizeCommand(SizeChangedEventArgs e)
	    {
            var isLandscape = e.NewSize.Width > e.NewSize.Height;
            DynamicPhotoSize = isLandscape ? e.NewSize.Width / 10 : e.NewSize.Width / 5;
            MaximumColumns = (int)(e.NewSize.Width / DynamicPhotoSize);
        }

	    private void OnExecuteAdaptWindowSizeCommand(SizeChangedEventArgs e)
	    {
	        var borderWidth = 30;
            var isLandscape = e.NewSize.Width > e.NewSize.Height;
            AvailableModalSize = isLandscape ? e.NewSize.Height - borderWidth : e.NewSize.Width - borderWidth;

        }

	    private bool CanExecuteShowLinkCommand()
		{
			return SelectedItem != null;
		}

		public bool IsShowLink
		{
			get { return _isShowLink; }
			set
			{
				_isShowLink = value;
				OnPropertyChanged();
			}
		}

		private void OnExecuteShowLinkCommand()
		{
			IsShowLink = !IsShowLink;
		}

		public ICommand CloseDetails => _cLoseDetails ?? (_cLoseDetails = new DelegateCommand(OnExecuteCloseDetails));

		private void OnExecuteCloseDetails()
		{
			//SelectedItem = null;
		    DetailsIsVisible = false;
		}

	    public ICommand ShowDetails => _showDetails ?? (_showDetails = new DelegateCommand(OnExecuteShowDetails));

	    private void OnExecuteShowDetails()
	    {
	        if (!DetailsIsVisible)
	            DetailsIsVisible = true;
	    }

	    public ICommand SearchLocationCommand => _searchLocationCommand ?? (_searchLocationCommand = new DelegateCommand<Geopoint>(OnExecuteSearchLocationCommand));

	    private async void OnExecuteSearchLocationCommand(Geopoint point)
	    {
	        MapCenterGeopoint = point;
	        SelectionRadiusGeopoint = point;
            SelectionAddress = await ReverseGeocoding(point.Position.Longitude,
                   point.Position.Latitude);
        }

	    public ICommand MapDoubleTappedCommand => _mapDoubleTappedCommand ??
	                                              (_mapDoubleTappedCommand = new DelegateCommand<MapInputEventArgs>(OnExecuteMapDoubleTappedCommand));

	    private async void OnExecuteMapDoubleTappedCommand(MapInputEventArgs mapInputEventArgs)
	    {
	        MapCenterGeopoint = mapInputEventArgs.Location;
	        SelectionRadiusGeopoint = mapInputEventArgs.Location;
            SelectionAddress = await ReverseGeocoding(SelectionRadiusGeopoint.Position.Longitude,
                   SelectionRadiusGeopoint.Position.Latitude);
        }

	    #endregion

		#region Public fields

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

		public PhotoWithUserLink SelectedItem
		{
			get { return _selectedItem; }
			set
			{
				_selectedItem = value;
				OnPropertyChanged();
			}
		}

		public double AvailableModalSize
		{
			get { return _availableModalSize; }
			set
			{
				_availableModalSize = value;
				OnPropertyChanged();
			}
		}

		public double DynamicPhotoSize
		{
			get { return _dynamicPhotoSize; }
			set
			{
				_dynamicPhotoSize = value;
				OnPropertyChanged();
			}
		}

		public int MaximumColumns
		{
			get { return _maximumColumns; }
			set
			{
				_maximumColumns = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<PhotoWithUserLink> Photos { get; set; }

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
	        set { _selectionAddress = value; OnPropertyChanged();}
	    }

	    public string SearchAddress
		{
			get { return _searchAddress; }
			set
			{
				_searchAddress = value;
				OnPropertyChanged();
			    DirectGeocoding(SearchAddress);
			}
		}

	    public List<MapLocation> SearchedLocations
	    {
	        get
	        {
	            return _searchedLocations;
	        }
	        set
	        {
	            _searchedLocations = value; 
	            OnPropertyChanged();
	        }
	    }

	    #endregion

		#region Public methods

		public async void GetUserLocation()
		{
			try
			{
				var geoposition = await _geolocator.GetGeopositionAsync();
			    MapCenterGeopoint = geoposition.Coordinate.Point;
                SelectionRadiusGeopoint = geoposition.Coordinate.Point;
			    SelectionAddress =
			        await
			            ReverseGeocoding(geoposition.Coordinate.Point.Position.Longitude,
			                geoposition.Coordinate.Point.Position.Latitude);
			}
			catch (Exception)
			{
                var dialog = new MessageDialog("Надо было разрешить");
                await dialog.ShowAsync();
            }

		}

		#endregion

		#region Private methods

		private async void AdapterOnNewPhotoAlertEventHandler(object sender, NewPhotoAlertEventArgs e)
		{
			try
			{
				var imageLinks = e.Photos;
				foreach (var imageLink in imageLinks)
				{
					await DownloadPhoto(imageLink);
				}

			}
			catch (Exception exception)
			{
			}
		}

		private async Task DownloadPhoto(PhotoItemModel photoItem)
		{
			await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
			{
				var bitmapImage = new BitmapImage(new Uri(photoItem.ImageLink));

				var item = new PhotoWithUserLink
				{
					Image = bitmapImage,
					UserLink = photoItem.ProfileLink,
					Longitude = photoItem.Longitude,
					Latitude = photoItem.Latitude,
					FormattedAddress = await ReverseGeocoding(photoItem.Longitude, photoItem.Latitude),
                    Source = photoItem.Source
				};
				if (!Photos.Contains(item))
				{
					Photos.Add(item);
				}
			});
		}

		private async Task<string> ReverseGeocoding(double longitude, double latitude)
		{
			try
			{
				var location = new BasicGeoposition
				{
					Latitude = latitude,
					Longitude = longitude
				};
				var pointToReverseGeocode = new Geopoint(location);
				var result =
					  await MapLocationFinder.FindLocationsAtAsync(pointToReverseGeocode);

				return result.Status == MapLocationFinderStatus.Success && result.Locations.Any() ? result.Locations[0].Address.FormattedAddress : string.Empty;
			}
			catch (Exception ex)
			{
				var message = new MessageDialog(ex.Message);
				await message.ShowAsync();
			}
			return string.Empty;
		}

        private async void DirectGeocoding(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                SearchedLocations = new List<MapLocation>();
                return;
            }
                
            var mapLocationFinderResult = await MapLocationFinder.FindLocationsAsync(address, MapCenterGeopoint, 10);
            SearchedLocations = mapLocationFinderResult.Status == MapLocationFinderStatus.Success ?  mapLocationFinderResult.Locations.ToList() : new List<MapLocation>();
        }
        #endregion

        public MainPageViewModel()
		{
			var vkAdapterConfig = new VkAdapterConfig { ApiAddress = "https://api.vk.com/method/photos.search" };
			var instagramAdapterConfig = new InstagramAdapterConfig() { ApiAddress = "https://api.instagram.com/v1/media/search", AccessToken = "241559688.1677ed0.4b7b8ad7ea8249a39e94fde279cca059" };

			var vkAdapter = new VkAdapter(vkAdapterConfig);
			var instagramAdapter = new InstagramAdapter(instagramAdapterConfig);
            _geolocator = new Geolocator();
            GetUserLocation();

            _adapterManager = new AdapterManager();
			
			_adapterManager.AddAdapter(vkAdapter);
			_adapterManager.AddAdapter(instagramAdapter);
			_adapterManager.OnNewPhotosReceived += AdapterOnNewPhotoAlertEventHandler;
			Radius = 5000;
			Photos = new ObservableCollection<PhotoWithUserLink> ();
        }

	    public bool DetailsIsVisible
	    {
	        get { return _detailsIsVisible; }
	        set { _detailsIsVisible = value; OnPropertyChanged();}
	    }

	    public int SelectedPivotIndex
	    {
	        get { return _selectedPivotIndex; }
	        set { _selectedPivotIndex = value; OnPropertyChanged();}
	    }
	}
}
