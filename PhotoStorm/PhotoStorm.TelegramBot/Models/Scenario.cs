using System.Collections.Generic;
using Bubbleshot.TelegramBot.Interfaces;

namespace Bubbleshot.TelegramBot.Models
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