using System.Collections.Generic;

namespace BubbleShot.Server.Common.Base
{
	public abstract class BaseRequestParameters
	{
		public abstract Dictionary<string,string> Parameters { get; }
	}
}
