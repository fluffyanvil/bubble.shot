using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BubbleShot.Server.WebServices.TestWcfService
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TestWcfService" in code, svc and config file together.
	// NOTE: In order to launch WCF Test Client for testing this service, please select TestWcfService.svc or TestWcfService.svc.cs at the Solution Explorer and start debugging.
	public class TestWcfService : ITestWcfService
	{
		public string SayHello()
		{
			return "Hello, Tofik!";
		}
	}
}
