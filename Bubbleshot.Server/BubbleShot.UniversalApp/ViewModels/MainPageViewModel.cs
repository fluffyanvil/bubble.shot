using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Windows.Networking.BackgroundTransfer;
using Windows.Services.Maps;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;
using Bubbleshot.Server.Adapters.Pcl.Base;
using Bubbleshot.Server.Adapters.Pcl.Vkontakte;
using Bubbleshot.Server.Common.Pcl.Models;
using BubbleShot.UniversalApp.Models;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

namespace BubbleShot.UniversalApp.ViewModels
{
	public class MainPageViewModel : ViewModelBase
	{
		private readonly VkAdapter _adapter;
		private readonly BackgroundDownloader _backgroundDownloader;
		private DelegateCommand _startAdapterCommand;
		private DelegateCommand _stopAdapterCommand;
		private double _longitude;
		private double _latitude;
		private int _radius;
		private Geopoint _location;
		private readonly IStorageFolder _installedLocation = Windows.ApplicationModel.Package.Current.InstalledLocation;
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

		public MapLocation SearchedLocation
		{
			get { return _searchedLocation; }
			set
			{
				_searchedLocation = value; 
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
			}
		}

		public Geoposition Geoposition
		{
			get { return _geoposition; }
			set
			{
				_geoposition = value;
				OnPropertyChanged();
			}
		}

		public delegate void GetLocation();

		public delegate void RadiusChanged();

		public event GetLocation GetLocationEvent;
		public event RadiusChanged RadiusChangedEvent;

		public bool ItemIsSelected => SelectedItem != null;

		public VkPhotoWithUserLink SelectedItem
		{
			get { return _selectedItem; }
			set
			{
				_selectedItem = value;
				OnPropertyChanged();
				if (value != null)
					OnGoToAddressEvent(_selectedItem.PositionGeopoint);
			}
		}

		public MainPageViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
			var adapterConfig = new VkAdapterConfig { ApiAddress = "https://api.vk.com/method/photos.search" };
			_adapter = new VkAdapter(adapterConfig);
			_adapter.NewPhotoAlertEventHandler += AdapterOnNewPhotoAlertEventHandler;
			Radius = 5000;
			Photos = new ObservableCollection<VkPhotoWithUserLink>();
			_backgroundDownloader = new BackgroundDownloader();
			_geolocator = new Geolocator();
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

		public async void GetPosition()
		{
			try
			{
				Geoposition = await _geolocator.GetGeopositionAsync();
				Longitude = _geoposition.Coordinate.Longitude;
				Latitude = _geoposition.Coordinate.Latitude;
				DeviceLocation = new Geopoint(new BasicGeoposition() {Longitude = Geoposition.Coordinate.Longitude, Latitude = Geoposition.Coordinate.Latitude});
				OnOnGetLocation();
			}
			catch (Exception)
			{
				var dialog = new MessageDialog("Надо было разрешить");
				await dialog.ShowAsync();
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

		private async void AdapterOnNewPhotoAlertEventHandler(object sender, NewPhotoAlertEventArgs e)
		{
			try
			{
				var imageLinks = (List<PhotoItemModel>) e.Photos;
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
			var file = await _installedLocation.CreateFileAsync(string.Format("{0}.{1}", Guid.NewGuid().ToString("N"), "jpg"), CreationCollisionOption.GenerateUniqueName);
			var downloadOperation = _backgroundDownloader.CreateDownload(new Uri(photoItem.ImageLink), file);
			await downloadOperation.StartAsync();
			var stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read);


			await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
			{
				var bitmapImage = new BitmapImage();
				bitmapImage.SetSource(stream);
				Photos.Add(new VkPhotoWithUserLink {Image = bitmapImage, UserLink = photoItem.UserLink, Longitude = photoItem.Longitude, Latitude = photoItem.Latitude, FormattedAddress = await ReverseGeocoding(photoItem.Longitude, photoItem.Latitude)});
				
			});
			await file.DeleteAsync();
		}


		public ObservableCollection<VkPhotoWithUserLink> Photos { get; set; }

		public int Radius
		{
			get { return _radius; }
			set { _radius = value; OnPropertyChanged(); OnRadiusChangedEvent(); }
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

		public ICommand StopAdapterCommand => _stopAdapterCommand ?? (_stopAdapterCommand = new DelegateCommand(OnExecuteStopAdapterCommand, CanExecuteStopAdapterCommand));

		private bool CanExecuteStopAdapterCommand()
		{
			return _adapter.Active;
		}

		private void OnExecuteStopAdapterCommand()
		{
			_adapter.Stop();
			UpdateCommandsAvailability();
		}

		public ICommand StartAdapterCommand => _startAdapterCommand ?? (_startAdapterCommand = new DelegateCommand(OnExecuteStartAdapter, CanExecuteStartAdapter));

		private bool CanExecuteStartAdapter()
		{
			return !_adapter.Active;
		}

		private void OnExecuteStartAdapter()
		{
			_adapter?.Start(Location.Position.Latitude, Location.Position.Longitude, Radius);
			UpdateCommandsAvailability();
		}

		private void UpdateCommandsAvailability()
		{
			_startAdapterCommand.RaiseCanExecuteChanged();
			_stopAdapterCommand.RaiseCanExecuteChanged();
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

		public ICommand CLoseDetails => _cLoseDetails ?? (_cLoseDetails = new DelegateCommand(OnExecuteCloseDetails));

		private void OnExecuteCloseDetails()
		{
			SelectedItem = null;
		}

		protected virtual void OnOnGetLocation()
		{
			GetLocationEvent?.Invoke();
		}

		protected virtual void OnRadiusChangedEvent()
		{
			RadiusChangedEvent?.Invoke();
		}

		public string SearchAddress
		{
			get { return _searchAddress; }
			set
			{
				_searchAddress = value;
				OnPropertyChanged();
			}
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

		public ICommand GoToSelectedItemAddress => _goToSelectedItemAddress ??
		                                           (_goToSelectedItemAddress = new DelegateCommand(OnExecuteGoToSelectedItemAddress, CanExecuteGoToSelectedItemAddress));

		private bool CanExecuteGoToSelectedItemAddress()
		{
			if (SelectedItem != null)
				return !string.IsNullOrEmpty(SelectedItem.FormattedAddress);
			return false;
		}

		private void OnExecuteGoToSelectedItemAddress()
		{
			OnGoToAddressEvent(SelectedItem.PositionGeopoint);
		}

		public delegate void GoToAddress(Geopoint positionGeopoint);

		public event GoToAddress GoToAddressEvent;

		protected virtual void OnGoToAddressEvent(Geopoint positionGeopoint)
		{
			GoToAddressEvent?.Invoke(positionGeopoint);
		}

		public bool SelectedItemMarkerVisibility { get; set; }
	}
}
