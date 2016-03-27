using System;
using System.Collections.Generic;
using System.Linq;
using BubbleShot.Server.Common.Enums;
using BubbleShot.Server.Common.Models;
using BubbleShot.Server.Common.Results.Instagram;

namespace BubbleShot.Server.Adapters.Helpers
{
	public class InstagramPhotoItemMapper
	{
		public InstagramPhotoItemMapper()
		{

		}

		public IEnumerable<PhotoItemModel> MapVkPhotoItems(List<InstagramPhotosSearchResultPhotoItem> items)
		{
			try
			{
				return items?.Select(i => new PhotoItemModel
				{
					ChannelType = ChannelType.Vkontakte,
					InstagramProfileLink = i.Link,
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
