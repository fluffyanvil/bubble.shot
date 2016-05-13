using System.Collections.Generic;
using PhotoStorm.TelegramBot.Interfaces;

namespace PhotoStorm.TelegramBot.Models
{
	public class Scenario : IScenario
	{
		public List<IStep> Actions { get; set; }

		public Scenario()
		{
			Actions = new List<IStep>();
		}
		public void Run()
		{
			
		}
	}
}