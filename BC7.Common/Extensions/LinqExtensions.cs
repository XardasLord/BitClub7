using System.Collections.Generic;
using System.Linq;

namespace BC7.Common.Extensions
{
    public static class LinqExtensions
    {
        public static bool ContainsAll<T>(this IEnumerable<T> containingList, IEnumerable<T> lookupList)
        {
            return !lookupList.Except(containingList).Any();
        }
    }
}
