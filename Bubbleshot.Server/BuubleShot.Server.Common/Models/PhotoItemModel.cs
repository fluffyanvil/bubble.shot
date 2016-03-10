using System;
using BubbleShot.Server.Common.Enums;

namespace BubbleShot.Server.Common.Models
{
	public class PhotoItemModel
	{
		public string ImageLink { get; set; }
		public ChannelType ChannelType { get; set; }
		public DateTime TimeCreated { get; set; }
		public double Longitude { get; set; }
		public double Latitude { get; set; }
	}
}
