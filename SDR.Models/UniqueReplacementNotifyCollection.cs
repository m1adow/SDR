using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SDR.Models;

public class UniqueReplacementNotifyCollection<TKey, TValue>(int size) : IEnumerable<KeyValuePair<TKey, TValue>>
{
    private readonly Dictionary<TKey, TValue> vectors = new(size);

    public event Action? Updated;

    public int Count => vectors.Count;

    public KeyValuePair<TKey, TValue> this[int index] => vectors.ElementAt(index); 

    public void Add(TKey key, TValue value)
    {
        vectors[key] = value;
        Updated?.Invoke();
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        foreach (var vector in vectors)
        {
            yield return vector;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}