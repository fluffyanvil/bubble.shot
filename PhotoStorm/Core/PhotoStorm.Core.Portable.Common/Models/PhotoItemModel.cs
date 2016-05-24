using System;
using PhotoStorm.Core.Portable.Common.Enums;

namespace PhotoStorm.Core.Portable.Common.Models
{
	public class PhotoItemModel
	{
		public string ImageLink { get; set; }
		public string ProfileLink { get; set; }
		public ChannelType Source { get; set; }
		public DateTime TimeCreated { get; set; }
		public double Longitude { get; set; }
		public double Latitude { get; set; }
	}
}
