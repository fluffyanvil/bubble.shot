using System.Collections.Generic;

namespace Bubbleshot.TelegramBot.Interfaces
{
	public interface IScenario
	{
		List<IStep> Actions { get; set; }
		void Run();
	}
}