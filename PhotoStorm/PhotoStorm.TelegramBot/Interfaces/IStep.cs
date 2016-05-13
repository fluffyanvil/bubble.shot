using System;

namespace PhotoStorm.TelegramBot.Interfaces
{
	public interface IStep
	{
		int Code { get; set; }
		Action Action { get; set; }
	}
}