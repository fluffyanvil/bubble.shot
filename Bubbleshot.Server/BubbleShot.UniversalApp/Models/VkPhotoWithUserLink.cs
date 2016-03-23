using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace BubbleShot.UniversalApp.Models
{
	public class VkPhotoWithUserLink
	{
		public ImageSource Image { get; set; }
		public string UserLink { get; set; }
		public string ImageDescription { get; set; }
	}
}
