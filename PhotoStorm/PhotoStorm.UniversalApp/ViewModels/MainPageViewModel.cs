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
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.AspNet.SignalR.Client;
using PhotoStorm.Core.Portable.Adapters.EventArgs;
using PhotoStorm.Core.Portable.Adapters.Manager;
using PhotoStorm.Core.Portable.Adapters.Rules;
using PhotoStorm.Core.Portable.Common.Models;
using PhotoStorm.Core.Portable.Works.Works;
using PhotoStorm.UniversalApp.Controls;
using PhotoStorm.UniversalApp.Extensions;
using PhotoStorm.UniversalApp.Helpers;
using PhotoStorm.UniversalApp.Models;
using Prism.Commands;
using Prism.Windows.Mvvm;



namespace PhotoStorm.UniversalApp.ViewModels
{
	public class MainPageViewModel : ViewModelBase
	{
	    private DelegateCommand _startManagerCommand;
		private DelegateCommand _stopManagerCommand;
	    private int _radius;
	    private PhotoWithUserLink _selectedItem;
		private DelegateCommand _cLoseDetails;
		private Geolocator _geolocator;
	    private double _availableModalSize;
		private Geopoint _mapCenterGeopoint;
		private string _searchAddress;
		private double _dynamicPhotoSize;
		private int _maximumColumns;

		private IAdapterManager _adapterManager;
		private DelegateCommand<object> _removeItemCommand;
		private DelegateCommand _removeAllItemsCommand;
		private bool _showOnMap;
	    private IEnumerable<MapLocation> _searchedLocations;
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

	    public bool IsStandalone
	    {
	        get { return _isStandalone; }
	        set { _isStandalone = value; OnPropertyChanged();}
	    }

	    private bool CanExecuteStopManagerCommand()
		{
	        if (IsStandalone)
	            return _adapterManager.CanStop;
	        else
	        {
                return _work != null;
            }
        }

		private void OnExecuteStopManagerCommand()
		{
		    if (IsStandalone)
		        _adapterManager.Stop();
		    else
		    {
                _hubProxy.Invoke("StopWork", _work).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {

                    }

                }).Wait();
                _work = null;
            }
            UpdateCommandAvailability();
		}

		public ICommand StartManagerCommand => _startManagerCommand ?? (_startManagerCommand = new DelegateCommand(OnExecuteStartManager, CanExecuteStartManager));

		private bool CanExecuteStartManager()
		{
            if(IsStandalone)
                return _adapterManager.CanStart;
            else
            {
                return _work == null;
            }
        }

		private IAdapterRule AdapterRule => new AdapterRule()
		{
			Latitude = MapCenterGeopoint.Position.Latitude,
			Longitude = MapCenterGeopoint.Position.Longitude,
			Radius = Radius
		};

		private void OnExecuteStartManager()
		{
		    if (IsStandalone)
		        _adapterManager.Start(AdapterRule);
		    else
		    {
                var work = new CreateWorkModel() { Longitude = MapCenterGeopoint.Position.Longitude, Latitude = MapCenterGeopoint.Position.Latitude, Radius = Radius };
                if (_hubConnection.State == ConnectionState.Connected)
                {
                    _hubProxy.Invoke("AddWork", work).ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {

                        }
                    }).Wait();
                }
            }

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
            AvailableModalSize = isLandscape ? e.NewSize.Height - 3 * borderWidth : e.NewSize.Width - borderWidth;

        }

		public int AppBarHeight
		{
			get { return _appBarHeight; }
			set { _appBarHeight = value; OnPropertyChanged();}
		}

		public bool IsShowLink => true;

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
            SelectionAddress = await GeocodingHelper.GetAddressByCoordinates(point);
        }

	    public ICommand MapDoubleTappedCommand => _mapDoubleTappedCommand ??
	                                              (_mapDoubleTappedCommand = new DelegateCommand<MapInputEventArgs>(OnExecuteMapDoubleTappedCommand));

	    private async void OnExecuteMapDoubleTappedCommand(MapInputEventArgs mapInputEventArgs)
	    {
	        MapCenterGeopoint = mapInputEventArgs.Location;
	        SelectionRadiusGeopoint = mapInputEventArgs.Location;
            SelectionAddress = await GeocodingHelper.GetAddressByCoordinates(mapInputEventArgs.Location);
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
			    GetAddresses();
			}
		}

	    public IEnumerable<MapLocation> SearchedLocations
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

		private async void GetAddresses()
		{
			SearchedLocations = await GeocodingHelper.DirectGeocoding(SearchAddress, MapCenterGeopoint);
		}

#endregion

#region Public methods

		public async 
#endregion

#region Public methods
		Task
GetUserLocation()
		{
			try
			{
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
					await AddNewPhoto(imageLink);
				}

			}
			catch (Exception )
			{
			}
		}

		private async Task AddNewPhoto(PhotoItemModel photoItem)
		{
		    if (Photos.Any(p => p.ImageLink.Equals(photoItem.ImageLink)))
		    {
		        return;
		    }
			await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
			{
				var bitmapImage = new BitmapImage(new Uri(photoItem.ImageLink));

				var item = new PhotoWithUserLink
				{
                    ImageLink = photoItem.ImageLink,
					Image = bitmapImage,
					UserLink = photoItem.ProfileLink,
					Longitude = photoItem.Longitude,
					Latitude = photoItem.Latitude,
					FormattedAddress = await GeocodingHelper.GetAddressByCoordinates(photoItem.Longitude, photoItem.Latitude),
                    Source = photoItem.Source
				};
				if (!Photos.Contains(item))
				{
					Photos.Add(item);
				}
			});
		}
#endregion

	    public MainPageViewModel()
	    {
		    AuthInstagram().Wait(); // TODO: сделать заебись вход с инстаграмом
		    StartViewModel();
	    }

	    private async void StartViewModel()
	    {
	        var dialog = new StartContentDialog();
            var result = await dialog.ShowAsync();
	        switch (result)
	        {
	            case ContentDialogResult.None:
	                break;
	            case ContentDialogResult.Primary:
	                IsStandalone = await TryConnectToHub(dialog.Url);
                    break;
	            case ContentDialogResult.Secondary:
	                IsStandalone = true;
                    break;
	            default:
	                throw new ArgumentOutOfRangeException();
	        }
            await InitViewModel(IsStandalone);
        }

	    private async Task InitViewModel(bool isStandalone)
        {
            _geolocator = new Geolocator();
            await GetUserLocation();
            if (isStandalone)
            {
                _adapterManager = new AdapterManager() {InstagramAccessToken = _instagramAccessToken};
                _adapterManager.OnNewPhotosReceived += AdapterOnNewPhotoAlertEventHandler;
            }
            Radius = 5000;
            Photos = new ObservableCollection<PhotoWithUserLink>();
        }

        private async Task<bool> TryConnectToHub(string url)
        {
            var isStandalone = false;
	        try
	        {
	            _hubConnection = new HubConnection($"http://{url}:9000/signalr/hubs");
	            _hubProxy = _hubConnection.CreateHubProxy("notificationHub");
	            _hubProxy.On<NewPhotoAlertEventArgs>("notify", OnNotify);
	            _hubProxy.On<Work>("workAdded", OnWorkAdded);

	            isStandalone = await _hubConnection.Start().ContinueWith(task => task.IsFaulted);
	        }
	        catch (Exception ex)
	        {
	        }
            return isStandalone;
	    }

	    private IWork _work;

	    private void OnWorkAdded(Work work)
	    {
	        try
	        {
	            if (work != null)
	            {
	                _work = work;
	                //UpdateCommandAvailability();
	            }
	        }
	        catch (Exception ex)
	        {
	        }
	    }

	    private async void OnNotify(NewPhotoAlertEventArgs args)
	    {
	        try
	        {
	            var data = args;
		        if (data == null) return;
		        var imageLinks = data.Photos;
		        foreach (var imageLink in imageLinks)
		        {
			        await AddNewPhoto(imageLink);
		        }
	        }
	        catch (Exception ex)
	        {
	        }
	    }

		private string _instagramAccessToken;

		private async Task<string> AuthInstagram()
		{
			var startURL = "https://www.instagram.com/oauth/authorize/?client_id=b83a51ed034f4156a3fbb79d1fabd2e3&redirect_uri=http://localhost&response_type=token";
			var endURL = "http://localhost";

			var startURI = new System.Uri(startURL);
			var endURI = new System.Uri(endURL);

			string result;
			var vault = new Windows.Security.Credentials.PasswordVault();

			var vaultTokens = vault.RetrieveAll();
			var instagramAccessToken = vaultTokens.FirstOrDefault(t => t.Resource == "PhotoStorm");


			if (instagramAccessToken == null)
			{
				try
				{
					var webAuthenticationResult =
						await Windows.Security.Authentication.Web.WebAuthenticationBroker.AuthenticateAsync(
							Windows.Security.Authentication.Web.WebAuthenticationOptions.None,
							startURI,
							endURI);

					switch (webAuthenticationResult.ResponseStatus)
					{
						case Windows.Security.Authentication.Web.WebAuthenticationStatus.Success:
							// Successful authentication. 
							result = webAuthenticationResult.ResponseData.ToString();
							var accesTokenIntro = "#access_token=";
							var accessTokenString = result.Substring(result.IndexOf(accesTokenIntro) + accesTokenIntro.Length);


							vault.Add(new Windows.Security.Credentials.PasswordCredential(
								"PhotoStorm", accessTokenString, accessTokenString));
							_instagramAccessToken = accessTokenString;
							break;
						case Windows.Security.Authentication.Web.WebAuthenticationStatus.ErrorHttp:
							// HTTP error. 
							result = webAuthenticationResult.ResponseErrorDetail.ToString();
							break;
						default:
							// Other error.
							result = webAuthenticationResult.ResponseData.ToString();
							break;
					}
				}
				catch (Exception ex)
				{
					// Authentication failed. Handle parameter, SSL/TLS, and Network Unavailable errors here. 
					result = ex.Message;
				}
			}
			else
			{
				_instagramAccessToken = instagramAccessToken.UserName;
			}
			return _instagramAccessToken;
		}

	    public bool DetailsIsVisible
	    {
	        get { return _detailsIsVisible; }
	        set
	        {
	            _detailsIsVisible = value;
	            OnPropertyChanged();
	        }
	    }

	    public int SelectedPivotIndex
	    {
	        get { return _selectedPivotIndex; }
	        set
	        {
	            _selectedPivotIndex = value;
	            OnPropertyChanged();
	        }
	    }

        private HubConnection _hubConnection;
	    private IHubProxy _hubProxy;
	    private bool _isStandalone;
		private int _appBarHeight;
	}
}
