using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using PhotoStorm.Core.Portable.Adapters.EventArgs;
using PhotoStorm.Core.Portable.Adapters.Manager;
using PhotoStorm.Core.Portable.Adapters.Rules;
using PhotoStorm.Core.Portable.Common.Models;
using PhotoStorm.UniversalApp.Helpers;
using PhotoStorm.UniversalApp.Models;
using Prism.Commands;


namespace PhotoStorm.UniversalApp.ViewModels
{
	public class MainPageViewModel : BaseViewModel
	{
	    private DelegateCommand _startManagerCommand;
		private DelegateCommand _stopManagerCommand;
		private IAdapterManager _adapterManager;
	    private int _selectedPivotIndex;

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
			Latitude = MapPivotItemViewModel.MapCenterGeopoint.Position.Latitude,
			Longitude = MapPivotItemViewModel.MapCenterGeopoint.Position.Longitude,
			Radius = MapPivotItemViewModel.Radius
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
#endregion

#region Private methods

		private async void AdapterOnNewPhotoAlertEventHandler(object sender, NewPhotoAlertEventArgs e)
		{
			try
			{
				var imageLinks = e.Photos;
			    if (SelectedPivotIndex == 0)
			    {
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        NewPhotosCount += e.Photos.Count;
                    });
                    
			    }

				foreach (var imageLink in imageLinks)
				{
					await AddNewPhoto(imageLink);
				}

			}
			catch (Exception ex)
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
			MapPivotItemViewModel = new MapPivotItemViewModel();
			GalleryPivotItemViewModel = new GalleryPivotItemViewModel();
            _adapterManager = new AdapterManager { InstagramAccessToken = string.Empty };
            _adapterManager.OnNewPhotosReceived += AdapterOnNewPhotoAlertEventHandler;
            StartViewModel();
            NewPhotosNotificationVisibility = Visibility.Collapsed;
        }

	    private async void StartViewModel()
	    {
            await MapPivotItemViewModel.GetUserLocation();
        }

	    public int SelectedPivotIndex
	    {
	        get { return _selectedPivotIndex; }
	        set
	        {
	            _selectedPivotIndex = value;
	            OnPropertyChanged();
	            if (_selectedPivotIndex == 1)
	                NewPhotosCount = 0;
	        }
	    }

        private Visibility _newPhotosNotificationVisibility;
        private int _newPhotosCount;

        public Visibility NewPhotosNotificationVisibility
        {
            get { return _newPhotosNotificationVisibility; }
            set
            {
                _newPhotosNotificationVisibility = value;
                OnPropertyChanged();
            }
        }

        public int NewPhotosCount
        {
            get { return _newPhotosCount; }
            set
            {
                _newPhotosCount = value;
                if (_newPhotosCount > 0)
                    NewPhotosNotificationVisibility = Visibility.Visible;
                else
                {
                    NewPhotosNotificationVisibility = Visibility.Collapsed;
                }
                OnPropertyChanged();
            }
        }
    }
}
