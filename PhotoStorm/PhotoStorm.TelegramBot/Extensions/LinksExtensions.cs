using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bubbleshot.TelegramBot.Extensions
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
