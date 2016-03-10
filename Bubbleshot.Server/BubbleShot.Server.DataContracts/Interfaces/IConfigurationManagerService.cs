using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleShot.Server.DataContracts.Interfaces
{
	public interface IConfigurationManagerService
	{
		void GetConfiguration();
		void GetSetting(string settingName);
		void AddConfiguration();
		void SaveConfiguration();
	}
}
