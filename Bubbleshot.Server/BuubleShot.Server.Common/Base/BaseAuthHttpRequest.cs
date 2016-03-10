using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BubbleShot.Server.Common.Base
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
