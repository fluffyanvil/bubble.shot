using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Bubbleshot.Core.Portable.Common.Models;

namespace Bubbleshot.Core.Portable.Adapters.EventArgs
{
	public class NewPhotoAlertEventArgs : System.EventArgs
	{
		public int Count { get; set; }
		public List<PhotoItemModel> Photos { get; set; }
	}
}
