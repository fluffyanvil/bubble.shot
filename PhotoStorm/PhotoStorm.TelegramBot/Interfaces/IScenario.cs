using System.Collections.Generic;

namespace PhotoStorm.TelegramBot.Interfaces
{
	public interface IScenario
	{
		List<IStep> Actions { get; set; }
		void Run();
	}
}