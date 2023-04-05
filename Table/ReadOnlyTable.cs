
namespace TransCS.Table;

public class ReadOnlyTable<TKey, TValue> : IReadOnlyTable<TKey, TValue>
{
    private readonly Table<TKey, TValue> _table;

    public ReadOnlyTable(Table<TKey, TValue> table)
    {
        _table = table;
    }

    public int Count => _table.Count;
    public int Capacity => _table.Capacity;

    public bool TryGetValue(TKey key, out TValue value)
    {
        return _table.TryGetValue(key, out value);
    }

    public bool ContainsKey(TKey key)
    {
        return _table.ContainsKey(key);
    }

    public TValue this[TKey key] => _table[key];

    public IEnumerable<KeyValuePair<TKey, TValue>> GetAllKeyValuePairs()
    {
        return _table.GetAllKeyValuePairs();
    }

    public IEnumerable<TKey> FindKeysByValue(Func<TValue, bool> condition)
    {
        return _table.FindKeysByValue(condition);
    }
}