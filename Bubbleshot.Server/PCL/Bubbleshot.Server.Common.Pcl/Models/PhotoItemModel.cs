using System;
using Bubbleshot.Server.Common.Pcl.Enums;

namespace Bubbleshot.Server.Common.Pcl.Models
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
