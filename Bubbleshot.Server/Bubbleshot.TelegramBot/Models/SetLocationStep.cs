using System;
using Bubbleshot.TelegramBot.Interfaces;

namespace Bubbleshot.TelegramBot.Models
{
	public class SetLocationStep : IStep
	{
		public int Code { get; set; }
		public Action Action { get; set; }
	}
}