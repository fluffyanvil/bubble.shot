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
	    private DelegateCommand _startAdapterCommand;
		private DelegateCommand _stopAdapterCommand;
		private double _longitude;
		private double _latitude;
		private int _radius;
		private Geopoint _location;
        private readonly INavigationService _navigationService;
		private VkPhotoWithUserLink _selectedItem;
		private DelegateCommand _cLoseDetails;
		private readonly Geolocator _geolocator;
		private Geoposition _geoposition;
		private double _availableModalSize;
		private Geopoint _deviceLocation;
		private string _searchAddress;
		private MapLocation _searchedLocation;
		private DelegateCommand _showLinkCommand;
		private bool _isShowLink;
		private DelegateCommand _nextVictimCommand;
		private DelegateCommand _prevVictimCommand;
		private double _dynamicPhotoSize;
		private int _maximumColumns;
		private DelegateCommand _goToSelectedItemAddress;
		private Geopoint _selectedItemGeopoint;
		private bool _instagram;
		private bool _vkontakte;

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

	    #region Commands

		public ICommand GoToSelectedItemAddress => _goToSelectedItemAddress ??
												   (_goToSelectedItemAddress = new DelegateCommand(OnExecuteGoToSelectedItemAddress, CanExecuteGoToSelectedItemAddress));

		private bool CanExecuteGoToSelectedItemAddress()
		{
			return !string.IsNullOrEmpty(SelectedItem?.FormattedAddress);
		}

		private void OnExecuteGoToSelectedItemAddress()
		{
			OnGoToAddressEvent(SelectedItem.PositionGeopoint);
		}

		public ICommand StopAdapterCommand => _stopAdapterCommand ?? (_stopAdapterCommand = new DelegateCommand(OnExecuteStopAdapterCommand, CanExecuteStopAdapterCommand));

		private bool CanExecuteStopAdapterCommand()
		{
			return _adapterManager.CanStop;
		}

		private void OnExecuteStopAdapterCommand()
		{
			_adapterManager.Stop();
			UpdateCommandAvailability();
		}

		public ICommand StartAdapterCommand => _startAdapterCommand ?? (_startAdapterCommand = new DelegateCommand(OnExecuteStartAdapter, CanExecuteStartAdapter));

		private bool CanExecuteStartAdapter()
		{
			return _adapterManager.CanStart;
		}

		private IAdapterRule AdapterRule => new AdapterRule()
		{
			Latitude = DeviceLocation.Position.Latitude,
			Longitude = DeviceLocation.Position.Longitude,
			Radius = Radius
		};

		private void OnExecuteStartAdapter()
		{
			_adapterManager.Start(AdapterRule);
			UpdateCommandAvailability();
		}

		private void UpdateCommandAvailability()
		{
			_startAdapterCommand.RaiseCanExecuteChanged();
			_stopAdapterCommand.RaiseCanExecuteChanged();
		}

		public ICommand RemoveItemCommand => _removeItemCommand ??
		                                     (_removeItemCommand = new DelegateCommand<object>(OnExecuteRemoveItemCommand));

		private void OnExecuteRemoveItemCommand(object parameter)
		{
			var photo = parameter as VkPhotoWithUserLink;
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

		public ICommand NextVictimCommand => _nextVictimCommand ?? (_nextVictimCommand = new DelegateCommand(OnExecuteNextVictimCommand));

		private void OnExecuteNextVictimCommand()
		{
			var index = Photos.IndexOf(SelectedItem);
			if (index + 1 < Photos.Count)
			{
				SelectedItem = Photos[index + 1];
			}
		}

		public ICommand PrevVictimCommand => _prevVictimCommand ?? (_prevVictimCommand = new DelegateCommand(OnExecutePrevVictimCommand));

		private void OnExecutePrevVictimCommand()
		{
			var index = Photos.IndexOf(SelectedItem);
			if (index != 0)
			{
				SelectedItem = Photos[index - 1];
			}
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
            var isLandscape = e.NewSize.Width > e.NewSize.Height;
            AvailableModalSize = isLandscape ? e.NewSize.Height - 125 : e.NewSize.Width - 125;
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
			SelectedItem = null;
		}

	    public ICommand SearchLocationCommand => _searchLocationCommand ?? (_searchLocationCommand = new DelegateCommand<Geopoint>(OnExecuteSearchLocationCommand));

	    private void OnExecuteSearchLocationCommand(Geopoint point)
	    {
	        DeviceLocation = point;
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

		public Geopoint Location
		{
			get { return _location; }
			set
			{
				_location = value;
				OnPropertyChanged();
			}
		}
		public bool SelectedItemMarkerVisibility { get; set; }

		public bool Instagram
		{
			get { return _instagram; }
			set
			{
				_instagram = value;
				OnPropertyChanged();
			}
		}

		public bool Vkontakte
		{
			get { return _vkontakte; }
			set
			{
				_vkontakte = value;
				OnPropertyChanged();
			}
		}

		public Geopoint DeviceLocation
		{
			get { return _deviceLocation; }
			set
			{
				_deviceLocation = value;
				OnPropertyChanged();
			    ZoomLevel = 11;
			    SelectionAreaCirclePath = new Geopath(DeviceLocation.GetCirclePoints(Radius));
			}
		}

	    public Geopath SelectionAreaCirclePath
	    {
	        get { return _selectionAreaCirclePath; }
	        set { _selectionAreaCirclePath = value; OnPropertyChanged(); }
	    }

	    public Geopoint SelectedItemGeopoint
		{
			get { return _selectedItemGeopoint; }
			set
			{
				_selectedItemGeopoint = value ?? new Geopoint(new BasicGeoposition());
				if (value != null)
					OnGoToAddressEvent(_selectedItem.PositionGeopoint);
				OnPropertyChanged();
			}
		}

		public VkPhotoWithUserLink SelectedItem
		{
			get { return _selectedItem; }
			set
			{
				_selectedItem = value;
				SelectedItemGeopoint = _selectedItem?.PositionGeopoint;
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

		public ObservableCollection<VkPhotoWithUserLink> Photos { get; set; }

		public int Radius
		{
			get { return _radius; }
			set
            {
                _radius = value; OnPropertyChanged();
                SelectionAreaCirclePath = new Geopath(DeviceLocation.GetCirclePoints(Radius));
            }
		}

		public double Latitude
		{
			get { return _latitude; }
			set
			{
				if (_latitude == value)
					return;
				_latitude = value;
				OnPropertyChanged();
			}
		}

		public double Longitude
		{
			get { return _longitude; }
			set
			{
				if (_longitude == value)
					return;
				_longitude = value;
				OnPropertyChanged();
			}
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

        #region Delegates

        public delegate void GetLocation();
		public delegate void RadiusChanged();
		public delegate void GoToAddress(Geopoint positionGeopoint);

		public event GetLocation GetLocationEvent;
		public event RadiusChanged RadiusChangedEvent;
		public event GoToAddress GoToAddressEvent;

		protected virtual void OnGoToAddressEvent(Geopoint positionGeopoint)
		{
			GoToAddressEvent?.Invoke(positionGeopoint);
		}

		protected virtual void OnOnGetLocation()
		{
			GetLocationEvent?.Invoke();
		}

		protected virtual void OnRadiusChangedEvent()
		{
			RadiusChangedEvent?.Invoke();
		}

		#endregion

		#region Public methods

		public async void GetPosition()
		{
			try
			{
				var geoposition = await _geolocator.GetGeopositionAsync();
			    DeviceLocation = geoposition.Coordinate.Point;
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

				var item = new VkPhotoWithUserLink
				{
					Image = bitmapImage,
					UserLink = photoItem.ProfileLink,
					Longitude = photoItem.Longitude,
					Latitude = photoItem.Latitude,
					FormattedAddress = await ReverseGeocoding(photoItem.Longitude, photoItem.Latitude)
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

				return result.Status == MapLocationFinderStatus.Success ? result.Locations[0].Address.FormattedAddress : string.Empty;
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
                
            var mapLocationFinderResult = await MapLocationFinder.FindLocationsAsync(address, DeviceLocation, 10);
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
            GetPosition();

            _adapterManager = new AdapterManager();
			
			_adapterManager.AddAdapter(vkAdapter);
			_adapterManager.AddAdapter(instagramAdapter);
			_adapterManager.OnNewPhotosReceived += AdapterOnNewPhotoAlertEventHandler;
			Radius = 5000;
			Photos = new ObservableCollection<VkPhotoWithUserLink> ();
			


		}
}
}
