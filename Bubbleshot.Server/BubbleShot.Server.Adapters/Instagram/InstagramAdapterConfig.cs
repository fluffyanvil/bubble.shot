using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BubbleShot.Server.Adapters.Base;

namespace BubbleShot.Server.Adapters.Instagram
{
	public class InstagramAdapterConfig : BaseAdapterConfig
	{
		public string ApiAddress { get; set; }
		public string AccessToken { get; set; }
	}
}
