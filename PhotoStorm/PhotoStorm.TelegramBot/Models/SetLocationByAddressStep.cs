using System;
using PhotoStorm.TelegramBot.Interfaces;

namespace PhotoStorm.TelegramBot.Models
{
	public class SetLocationByAddressStep : IStep
	{
		public int Code { get; set; }
		public Action Action { get; set; }
	}
}