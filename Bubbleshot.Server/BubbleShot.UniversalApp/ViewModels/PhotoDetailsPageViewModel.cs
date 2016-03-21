using BubbleShot.UniversalApp.Models;
using Prism.Windows.Mvvm;

namespace BubbleShot.UniversalApp.ViewModels
{
	public class PhotoDetailsPageViewModel : ViewModelBase
	{
		private VkPhotoWithUserLink _photoWithUserLink;

		public VkPhotoWithUserLink PhotoWithUserLink
		{
			get { return _photoWithUserLink; }
			set
			{
				_photoWithUserLink = value; 
				OnPropertyChanged();
			}
		}
	}
}
