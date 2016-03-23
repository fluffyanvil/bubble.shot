using System;
using System.Collections.Generic;
using System.Linq;
using Bubbleshot.Server.Common.Pcl.Enums;
using Bubbleshot.Server.Common.Pcl.Models;
using Bubbleshot.Server.Common.Pcl.Results.Vkontakte;

namespace Bubbleshot.Server.Adapters.Pcl.Helpers
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
					TimeCreated = i.Date,
					UserLink = i.OwnerId[0] == '-' ? string.Format("https://vk.com/club{0}", i.OwnerId.Substring(1)) : string.Format("https://vk.com/id{0}", i.OwnerId),
					Description = i.Text
				});
			}
			catch (Exception)
			{
			}
			return null;
		}
	}
}
