using System;

namespace Bubbleshot.TelegramBot.Interfaces
{
	public interface IStep
	{
		int Code { get; set; }
		Action Action { get; set; }
	}
}