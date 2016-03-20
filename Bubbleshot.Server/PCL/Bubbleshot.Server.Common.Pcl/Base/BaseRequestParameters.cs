using System.Collections.Generic;

namespace Bubbleshot.Server.Common.Pcl.Base
{
	public abstract class BaseRequestParameters
	{
		public abstract Dictionary<string,string> Parameters { get; }
	}
}
