using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStorm.UniversalApp.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            try
            {
	            var enumerable = items as T[] ?? items.ToArray();
	            if (!enumerable.Any()) return;
	            foreach (var item in enumerable)
	            {
		            collection.Add(item);
	            }
            }
            catch (Exception)
            {
                
            }
        }
    }
}
