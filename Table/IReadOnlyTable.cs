namespace TransCS.Table;

public interface IReadOnlyTable<TKey, TValue>
{
    int Count { get; }
    int Capacity { get; }
    bool TryGetValue(TKey key, out TValue value);
    bool ContainsKey(TKey key);
    TValue this[TKey key] { get; }
    IEnumerable<KeyValuePair<TKey, TValue>> GetAllKeyValuePairs();
    IEnumerable<TKey> FindKeysByValue(Func<TValue, bool> condition);
}