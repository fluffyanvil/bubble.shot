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
		public IEnumerable<PhotoItemModel> MapVkPhotoItems(List<VkPhotosSearchResponsePhotoItem> items)
		{
			try
			{
				return items?.Select(i => new PhotoItemModel
				{
					ChannelType = ChannelType.Vkontakte,
					ImageLink = i.Photo130,
					Latitude = i.Latitude,
					Longitude = i.Longitude,
					TimeCreated = i.Date,
					ProfileLink = i.OwnerId[0] == '-' ? "http://vk.com/club" + i.OwnerId.Substring(1) : "http://vk.com/id" + i.OwnerId,
					Source = 0
				});
			}
			catch (Exception)
			{
			}
			return null;
		}
	}
}
