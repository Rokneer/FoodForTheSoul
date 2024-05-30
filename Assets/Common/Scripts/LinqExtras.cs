using System.Collections.Generic;
using System.Linq;

public static class LinqExtras
{
    public static bool ContainsAllItems<T>(this IEnumerable<T> a, IEnumerable<T> b)
    {
        return !b.Except(a).Any();
    }
}
