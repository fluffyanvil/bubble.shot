using System;
using Telegram.Bot.Types;

namespace Bubbleshot.TelegramBot
{
	class Program
	{
		static Telegram.Bot.Api _bot;
		static  void Main(string[] args)
		{
			_bot = new Telegram.Bot.Api("199505686:AAGOH6bxRG72u5X80cRliLhW9wEWVK6eZ1E");
			var me = _bot.GetMe().Result;
			Console.WriteLine(me.Username);
			_bot.StartReceiving();
			_bot.MessageReceived += BotOnMessageReceived;
			Console.ReadLine();
		}

		private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
		{
			Console.WriteLine(messageEventArgs.Message.Text);
			var messageSender = messageEventArgs.Message.From;

			await _bot.SendTextMessage(messageEventArgs.Message.Chat.Id, messageEventArgs.Message.Text);
		}
	}
}
