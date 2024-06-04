using System.Collections.Generic;
using System.Linq;

public static class LinqExtras
{
    public static bool ContainsAllItems<T>(this IEnumerable<T> first, IEnumerable<T> second)
    {
        return !second.Except(first).Any();
    }
}
