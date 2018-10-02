using System;
using System.Collections.Generic;
using System.Linq;
using PhotoStorm.Core.Portable.Adapters.Vk.Results;
using PhotoStorm.Core.Portable.Common.Enums;
using PhotoStorm.Core.Portable.Common.Models;

namespace PhotoStorm.Core.Portable.Adapters.Vk
{
	public class VkPhotoItemMapper
	{
		public IEnumerable<PhotoItemModel> MapVkPhotoItems(List<VkPhotosSearchResponsePhotoItem> items)
		{
			try
			{
				return items?.Select(i => new PhotoItemModel
				{
					Source = ChannelType.Vkontakte,
					ImageLink = i.Sizes.FirstOrDefault(s => s.InternalType.Equals("x"))?.Source,
					Latitude = i.Latitude,
					Longitude = i.Longitude,
					TimeCreated = i.Date,
					ProfileLink = i.OwnerId[0].Equals('-')
                        ? $"http://vk.com/club{i.OwnerId.Substring(1)}"
					    : $"http://vk.com/id{i.OwnerId}"
				});
			}
			catch (Exception)
			{
			}
			return null;
		}
	}
}
