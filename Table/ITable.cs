namespace TransCS.Table;

public interface ITable<TKey, TValue> : IReadOnlyTable<TKey, TValue>
{
    void Add(TKey key, TValue value);
    void Set(TKey key, TValue value);
    bool Remove(TKey key);
    void Clear();
    TValue this[TKey key] { set; }
}