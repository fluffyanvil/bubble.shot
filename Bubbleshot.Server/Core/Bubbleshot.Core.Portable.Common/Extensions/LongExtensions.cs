using System;

namespace Bubbleshot.Core.Portable.Common.Extensions
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
