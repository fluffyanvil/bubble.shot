using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using PhotoStorm.Core.Portable.Adapters.EventArgs;
using PhotoStorm.Core.Portable.Adapters.Instagram;
using PhotoStorm.Core.Portable.Adapters.Manager;
using PhotoStorm.Core.Portable.Adapters.Rules;
using PhotoStorm.Core.Portable.Adapters.Vkontakte;
using Telegram.Bot.Types;

namespace PhotoStorm.TelegramBot
{
	class Program
	{
		static Telegram.Bot.Api _bot;

		static List<IWork> _works; 

		static void Main(string[] args)
		{
			InitBot();
			_works = new List<IWork>();
			Console.ReadLine();
		}

		private static void InitBot()
		{
			_bot = new Telegram.Bot.Api("199505686:AAGOH6bxRG72u5X80cRliLhW9wEWVK6eZ1E");
			var me = _bot.GetMe().Result;
			Console.WriteLine(me.Id);
			_bot.StartReceiving();
			_bot.MessageReceived += BotOnMessageReceived;
			_bot.InlineQueryReceived += BotOnInlineQueryReceived;
			
		}

		private static void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
		{
		}

		private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
		{
			switch (messageEventArgs.Message.Text)
			{
				case "/start":
					await _bot.SendTextMessage(messageEventArgs.Message.Chat.Id, "Hello, I'm bot", replyMarkup:new ReplyKeyboardMarkup() {Keyboard = GetKeyboard()});
					return;

				case "/run":
					await _bot.SendTextMessage(messageEventArgs.Message.Chat.Id, CreateWork(messageEventArgs.Message.Chat.Id), replyMarkup: new ReplyKeyboardMarkup() { Keyboard = GetKeyboard() });
					return;
				case "/stop":
					await _bot.SendTextMessage(messageEventArgs.Message.Chat.Id, RemoveWork(messageEventArgs.Message.Chat.Id), replyMarkup: new ReplyKeyboardMarkup() { Keyboard = GetKeyboard() });
					return;
				default:
					await _bot.SendTextMessage(messageEventArgs.Message.Chat.Id, "Unrecognized");
					return;
			}
		}

		private static KeyboardButton[][] GetKeyboard()
		{
            var firstRow = new[] { new KeyboardButton("/run"), new KeyboardButton("/stop"), };
            var secondRow = new[] { new KeyboardButton("/city"), new KeyboardButton("/address"), };
            var keyboard = new[] { firstRow, secondRow };
            return keyboard;
		}

		private static string CreateWork(long chatId)
		{
			if (_works.Any(w => w.ChatId == chatId))
				return "You have work already";

			var adapterManager = new AdapterManager();
			
			var work = new BotWork(chatId, adapterManager);
			work.OnNewPhotosReceived += WorkOnNewPhotosReceived;
			_works.Add(work);
			_works.FirstOrDefault(w => w.ChatId == chatId).Start(new AdapterRule() {Longitude = 37.620205, Latitude = 55.751151, Radius = 20000});

			return "Your work is created";
		}

		private static string RemoveWork(long chatId)
		{
			if (_works.All(w => w.ChatId != chatId))
				return "Work not found";
			_works.FirstOrDefault(w => w.ChatId == chatId).Stop();
			return "You work stopped";
		}

		private static async void WorkOnNewPhotosReceived(object sender, NewPhotoAlertEventArgs newPhotoAlertEventArgs)
		{
			var work = sender as IWork;
			if (work == null) return;
			var chatId = work.ChatId;
			try
			{
				foreach (var image in newPhotoAlertEventArgs.Photos)
				{
					var bytes = await new WebClient().DownloadDataTaskAsync(image.ImageLink);
					await _bot.SendPhoto(chatId, new FileToSend() { Content = new MemoryStream(bytes), Filename = $"{Guid.NewGuid().ToString("N")}.jpg" }, caption: image.ProfileLink);
				}
			}
			catch (Exception)
			{
				await _bot.SendTextMessage(chatId,"Что-то пошло не так", replyMarkup: new ReplyKeyboardMarkup() { Keyboard = GetKeyboard() });
			}
		}
	}
}
