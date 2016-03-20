using System;

namespace Bubbleshot.Server.Common.Pcl.Extensions
{
	public static class DateTimeExtensions
	{
		public static long ToUnixStyle(this DateTime dateTime)
		{
			return Convert.ToInt64(dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
		}
	}
}
