using System;
using BubbleShot.Server.Common.Enums;

namespace BubbleShot.Server.Common.Models
{
	public class PhotoItemModel
	{
		public int Source { get; set; }
		public string ImageLink { get; set; }
		public string InstagramProfileLink { get; set; }
		public ChannelType ChannelType { get; set; }
		public DateTime TimeCreated { get; set; }
		public double Longitude { get; set; }
		public double Latitude { get; set; }
	}
}
