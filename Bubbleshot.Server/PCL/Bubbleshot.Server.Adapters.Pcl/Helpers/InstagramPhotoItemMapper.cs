using System;
using System.Collections.Generic;
using System.Linq;
using Bubbleshot.Server.Common.Pcl.Enums;
using Bubbleshot.Server.Common.Pcl.Models;
using Bubbleshot.Server.Common.Pcl.Results.Instagram;

namespace Bubbleshot.Server.Adapters.Pcl.Helpers
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
