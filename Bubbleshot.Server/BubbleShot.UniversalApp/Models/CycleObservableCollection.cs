using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace BubbleShot.UniversalApp.Models
{
	public class CycleObservableCollection<T> : ObservableCollection<T>
	{
		public int MaxItems { get; set; }
		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnCollectionChanged(e);
			if (Count > MaxItems)
				RemoveItem(0);
		}
	}
}
