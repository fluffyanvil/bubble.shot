using System;
using Bubbleshot.TelegramBot.Interfaces;

namespace Bubbleshot.TelegramBot.Models
{
	public class SetRadiusStep : IStep
	{
		public int Code { get; set; }
		public Action Action { get; set; }
	}
}