using System.Net;

namespace Bubbleshot.Server.Common.Pcl.Base
{
	public abstract class BaseAuthHttpRequest
	{
		protected HttpWebRequest Request;
		protected string Address;

		protected BaseAuthHttpRequest(string address)
		{
			Address = address;
		}

		public abstract string Auth();
	}
}
