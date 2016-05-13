using System;
using System.Collections.Generic;
using System.Linq;
using PhotoStorm.Core.Portable.Common.Enums;
using PhotoStorm.Core.Portable.Common.Models;
using PhotoStorm.Core.Portable.Common.Results.Instagram;

namespace PhotoStorm.Core.Portable.Adapters.Helpers
{
	public class InstagramPhotoItemMapper
	{
		public IEnumerable<PhotoItemModel> MapVkPhotoItems(List<InstagramPhotosSearchResultPhotoItem> items)
		{
			try
			{
				return items?.Select(i => new PhotoItemModel
				{
					ChannelType = ChannelType.Vkontakte,
					ProfileLink = i.Link,
					ImageLink = i.Images.StandardResolution.Url,
					Latitude = i.Location.Latitude,
					Longitude = i.Location.Longitude,
					TimeCreated = i.Date,
					Source = 1

				});
			}
			catch (Exception)
			{
			}
			return null;
		}
	}
}
