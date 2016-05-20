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
                if (items.Any())
                {
                    foreach (var item in items)
                    {
                        collection.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }
    }
}
