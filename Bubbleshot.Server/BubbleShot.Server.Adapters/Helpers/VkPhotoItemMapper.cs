using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BubbleShot.Server.Common.Enums;
using BubbleShot.Server.Common.Models;
using BubbleShot.Server.Common.Results.Vkontakte;

namespace BubbleShot.Server.Adapters.Helpers
{
	public class VkPhotoItemMapper
	{
		public VkPhotoItemMapper()
		{
			
		}

		public IEnumerable<PhotoItemModel> MapVkPhotoItems(List<VkPhotosSearchResponsePhotoItem> items)
		{
			try
			{
				return items?.Select(i => new PhotoItemModel
				{
					ChannelType = ChannelType.Vkontakte,
					ImageLink = i.Photo604,
					Latitude = i.Latitude,
					Longitude = i.Longitude,
					TimeCreated = i.Date
				});
			}
			catch (Exception)
			{
			}
			return null;
		}
	}
}
