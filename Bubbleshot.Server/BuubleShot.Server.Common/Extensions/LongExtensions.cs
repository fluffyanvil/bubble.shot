using System;

namespace BubbleShot.Server.Common.Extensions
{
	public static class LongExtensions
	{
		public static DateTime ToDateTime(this long timestamp)
		{
			var time = TimeSpan.FromSeconds(timestamp);
			return new DateTime() + time;
		}
	}
}
