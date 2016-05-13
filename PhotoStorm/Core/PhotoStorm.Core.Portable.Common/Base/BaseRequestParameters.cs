using System.Collections.Generic;

namespace PhotoStorm.Core.Portable.Common.Base
{
	public abstract class BaseRequestParameters
	{
		public abstract Dictionary<string,string> Parameters { get; }
	}
}
