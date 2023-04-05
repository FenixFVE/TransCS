namespace TransCS.Comparer;

public class KeyValuePairComparer<TKey, TValue> : IComparer<KeyValuePair<TKey, TValue>>
    where TKey : IComparable<TKey>
    where TValue : IComparable<TValue>
{
    public int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
    {
        var keyComparison = x.Key.CompareTo(y.Key);

        if (keyComparison != 0)
        {
            return keyComparison;
        }

        return x.Value.CompareTo(y.Value);
    }
}