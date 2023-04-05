namespace TransCS.Comparer;

public class SetsComparer
{
    public static bool PairsEqual<TKey, TValue>(
        IEnumerable<KeyValuePair<TKey, TValue>> list1,
        IEnumerable<KeyValuePair<TKey, TValue>> list2)
        where TKey : IComparable<TKey>
        where TValue : IComparable<TValue>
    {
        var comparer = new KeyValuePairComparer<TKey, TValue>();

        list1 = list1.OrderBy(x => x, comparer);
        list2 = list2.OrderBy(x => x, comparer);

        return list1.SequenceEqual(list2);
    }


    public static bool Equal<T>(IEnumerable<T> list1, IEnumerable<T> list2)
        where T : IComparable<T> =>
            list1.OrderBy(x => x)
                .SequenceEqual(
                    list2.OrderBy(x => x));
}