using System;
using PhotoStorm.TelegramBot.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PhotoStorm.TelegramBot.Helpers
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

		private static KeyboardButton[][] GetKeyboard(KeyboardType type)
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
			var firstRow = new[] {new KeyboardButton("/run"), new KeyboardButton("/stop"), };
			var secondRow = new[] {new KeyboardButton("/city"), new KeyboardButton("/address"), };
			var keyboard = new[] {firstRow, secondRow};
			return keyboard;
		}
	}
}
