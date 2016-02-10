using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.ViewModel.Extensions
{
    public static class FindFirstInstance
    {
        public static bool FindFirst<T>(this IEnumerable<T> list, Predicate<T> condition)
        {
            foreach(T item in list)
            {
                if (condition(item))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
