using System.Collections.Generic;
using PhotoStorm.Core.Portable.Common.Models;

namespace PhotoStorm.Core.Portable.Adapters.EventArgs
{
	public class NewPhotoAlertEventArgs : System.EventArgs
	{
		public int Count { get; set; }
		public List<PhotoItemModel> Photos { get; set; }
	}
}
