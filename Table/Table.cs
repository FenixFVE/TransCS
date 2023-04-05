namespace TransCS.Table;

public class Table<TKey, TValue> : IReadOnlyTable<TKey, TValue>
{

    private const int _initialCapacity = 8;

    private int _bucketCapacity;
    private int _bucketSize;
    private LinkedList<KeyValuePair<TKey, TValue>>[] _buckets;

    public Table()
    {
        _bucketCapacity = _initialCapacity;
        _bucketSize = 0;
        _buckets = new LinkedList<KeyValuePair<TKey, TValue>>[_initialCapacity];
    }

    public int Count => _bucketSize;
    public int Capacity => _bucketCapacity;

    private int GetBucketIndex(TKey key)
    {
        int hash = key.GetHashCode();
        int bucketIndex = hash % _bucketCapacity;
        if (bucketIndex < 0)
        {
            bucketIndex += _bucketCapacity;
        }
        return bucketIndex;
    }

    public void Resize()
    {
        _bucketCapacity *= 2;
        var newBuckets = new LinkedList<KeyValuePair<TKey, TValue>>[_bucketCapacity];
        foreach (var bucket in _buckets)
        {
            if (bucket is null)
            {
                continue;
            }

            foreach (var pair in bucket)
            {
                int newBucketIndex = GetBucketIndex(pair.Key);

                if (newBuckets[newBucketIndex] is null)
                {
                    newBuckets[newBucketIndex] = new LinkedList<KeyValuePair<TKey, TValue>>();
                }

                newBuckets[newBucketIndex].AddLast(pair);
            }
        }

        _buckets = newBuckets;
    }

    public void Clear()
    {
        _bucketCapacity = _initialCapacity;
        _bucketSize = 0;
        _buckets = new LinkedList<KeyValuePair<TKey, TValue>>[_initialCapacity];
    }

    public void Add(TKey key, TValue value)
    {
        int bucketIndex = GetBucketIndex(key);

        if (_buckets[bucketIndex] is null)
        {
            _buckets[bucketIndex] = new LinkedList<KeyValuePair<TKey, TValue>>();
        }

        foreach (var pair in _buckets[bucketIndex])
        {
            if (pair.Key.Equals(key))
            {
                throw new ArgumentException("An element with the same key already exists in the hashtable");
            }
        }

        _buckets[bucketIndex].AddLast(new KeyValuePair<TKey, TValue>(key, value));
        _bucketSize++;

        if (_bucketSize >= _bucketCapacity - 1)
        {
            Resize();
        }
    }

    public void Set(TKey key, TValue value)
    {
        int bucketIndex = GetBucketIndex(key);
        if (_buckets[bucketIndex] is null)
        {
            Add(key, value);
            return;
        }

        for (var pair = _buckets[bucketIndex].First; pair is not null; pair = pair.Next)
        {
            if (pair.Value.Key.Equals(key))
            {
                var newNode = new LinkedListNode<KeyValuePair<TKey, TValue>>(new KeyValuePair<TKey, TValue>(key, value));
                _buckets[bucketIndex].AddBefore(pair, newNode);
                _buckets[bucketIndex].Remove(pair);
                return;
            }
        }

        Add(key, value);
    }

    public bool Remove(TKey key)
    {
        int bucketIndex = GetBucketIndex(key);
        if (_buckets[bucketIndex] is null)
        {
            return false;
        }

        foreach (var pair in _buckets[bucketIndex])
        {
            if (pair.Key.Equals(key))
            {
                _buckets[bucketIndex].Remove(pair);
                _bucketSize--;
                return true;
            }
        }

        return false;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        int bucketIndex = GetBucketIndex(key);
        if (_buckets[bucketIndex] is null)
        {
            value = default(TValue);
            return false;
        }

        foreach (var pair in _buckets[bucketIndex])
        {
            if (pair.Key.Equals(key))
            {
                value = pair.Value;
                return true;
            }
        }

        value = default(TValue);
        return false;
    }

    public bool ContainsKey(TKey key)
    {
        int bucketIndex = GetBucketIndex(key);
        if (_buckets[bucketIndex] is null)
        {
            return false;
        }

        foreach (var pair in _buckets[bucketIndex])
        {
            if (pair.Key.Equals(key))
            {
                return true;
            }
        }

        return false;
    }

    public TValue this[TKey key]
    {
        get
        {
            TValue value;
            if (!TryGetValue(key, out value))
            {
                throw new KeyNotFoundException("The specified key was not found in the hashtable");
            }

            return value;
        }
        set => Set(key, value);
    }

    public IEnumerable<KeyValuePair<TKey, TValue>> GetAllKeyValuePairs()
    {
        foreach (var backet in _buckets)
        {
            if (backet is null) { continue; }

            foreach (var pair in backet)
            {
                yield return pair;
            }
        }
    }

    public IEnumerable<TKey> FindKeysByValue(Func<TValue, bool> condition)
    {
        foreach (var bucket in _buckets)
        {
            if (bucket is null) { continue; }

            foreach (var pair in bucket)
            {
                if (condition(pair.Value))
                {
                    yield return pair.Key;
                }
            }
        }
    }
}