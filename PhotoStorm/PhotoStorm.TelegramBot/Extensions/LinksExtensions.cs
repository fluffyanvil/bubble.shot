using System;

namespace PhotoStorm.TelegramBot.Extensions
{
	public static class LinksExtensions
	{
		public static string ToFilename(this string link)
		{
			var index = link.LastIndexOf("/", StringComparison.InvariantCultureIgnoreCase);

			return link.Substring(index);
		}
	}
}
