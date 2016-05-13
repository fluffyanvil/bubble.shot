using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bubbleshot.TelegramBot.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bubbleshot.TelegramBot.Helpers
{
	public class TelegramApiHelper
	{
		private Api _api;

		public TelegramApiHelper(Api api)
		{
			_api = api;
		}

		public void SetLocation(long chatId)
		{
			_api.SendTextMessage(chatId, "Что-то пошло не так", replyMarkup: new ReplyKeyboardMarkup() { Keyboard = GetKeyboard(KeyboardType.SetLocation) });
		}

		private static string[][] GetKeyboard(KeyboardType type)
		{
			switch (type)
			{
				case KeyboardType.SetLocation:
					break;
				case KeyboardType.SetRadius:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
			var firstRow = new[] {"/run", "/stop"};
			var secondRow = new[] {"/city", "/address"};
			var keyboard = new[] {firstRow, secondRow};
			return keyboard;
		}
	}
}
