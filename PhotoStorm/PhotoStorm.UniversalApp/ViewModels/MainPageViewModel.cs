using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Practices.Unity;
using PhotoStorm.Core.Portable.Adapters.EventArgs;
using PhotoStorm.Core.Portable.Adapters.Manager;
using PhotoStorm.Core.Portable.Adapters.Rules;
using PhotoStorm.Core.Portable.Common.Models;
using PhotoStorm.Core.Portable.Logging;
using PhotoStorm.Core.Portable.Works.Works;
using PhotoStorm.UniversalApp.Controls;
using PhotoStorm.UniversalApp.Helpers;
using PhotoStorm.UniversalApp.Models;
using Prism.Commands;
using Prism.Windows.Mvvm;



namespace PhotoStorm.UniversalApp.ViewModels
{
	public class MainPageViewModel : BaseViewModel
	{
	    private DelegateCommand _startManagerCommand;
		private DelegateCommand _stopManagerCommand;
		private IAdapterManager _adapterManager;
	    private int _selectedPivotIndex;
	    private ILogger _logger;
	    

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
			Latitude = MapPivotItemViewModel.MapCenterGeopoint.Position.Latitude,
			Longitude = MapPivotItemViewModel.MapCenterGeopoint.Position.Longitude,
			Radius = MapPivotItemViewModel.Radius
		};

		private void OnExecuteStartManager()
		{
		    if (IsStandalone)
		        _adapterManager.Start(AdapterRule);
		    else
		    {
                var work = new CreateWorkModel() { Longitude = MapPivotItemViewModel.MapCenterGeopoint.Position.Longitude, Latitude = MapPivotItemViewModel.MapCenterGeopoint.Position.Latitude, Radius = MapPivotItemViewModel.Radius };
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
		    if (GalleryPivotItemViewModel.Photos.Any(p => p.Source.ImageLink.Equals(photoItem.ImageLink)))
		    {
		        return;
		    }
			await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
			{
				var item = new PhotoWithUserLink
				{
					Image = new BitmapImage(new Uri(photoItem.ImageLink)),
					FormattedAddress = await GeocodingHelper.GetAddressByCoordinates(photoItem.Longitude, photoItem.Latitude),
                    Source = photoItem
				};
				if (!GalleryPivotItemViewModel.Photos.Contains(item))
				{
					GalleryPivotItemViewModel.Photos.Add(item);
				}
			});
		}
#endregion

		public MapPivotItemViewModel MapPivotItemViewModel { get;set; }
		public GalleryPivotItemViewModel GalleryPivotItemViewModel { get; set; }
	    public MainPageViewModel()
	    {
	        _logger = Container.Resolve<ILogger>();
            _logger.Info("MainPageViewModel created");
			MapPivotItemViewModel = new MapPivotItemViewModel();
			GalleryPivotItemViewModel = new GalleryPivotItemViewModel();
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

            await MapPivotItemViewModel.GetUserLocation();
            if (isStandalone)
            {
                _adapterManager = new AdapterManager {InstagramAccessToken = _instagramAccessToken};
                _adapterManager.OnNewPhotosReceived += AdapterOnNewPhotoAlertEventHandler;
            }
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
							result = webAuthenticationResult.ResponseData;
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
							result = webAuthenticationResult.ResponseData;
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
	}
}
