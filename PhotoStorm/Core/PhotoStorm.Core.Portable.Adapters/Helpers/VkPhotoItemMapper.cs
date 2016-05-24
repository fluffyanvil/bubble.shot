﻿using System;
using System.Collections.Generic;
using System.Linq;
using PhotoStorm.Core.Portable.Common.Enums;
using PhotoStorm.Core.Portable.Common.Models;
using PhotoStorm.Core.Portable.Common.Results.Vkontakte;

namespace PhotoStorm.Core.Portable.Adapters.Helpers
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
					ImageLink = i.Photo604,
					Latitude = i.Latitude,
					Longitude = i.Longitude,
					TimeCreated = i.Date,
					ProfileLink = i.OwnerId[0] == '-' ? "http://vk.com/club" + i.OwnerId.Substring(1) : "http://vk.com/id" + i.OwnerId
				});
			}
			catch (Exception)
			{
			}
			return null;
		}
	}
}
