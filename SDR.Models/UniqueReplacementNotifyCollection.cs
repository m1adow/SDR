using SDR.Models.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SDR.Models;

public class UniqueReplacementNotifyCollection<T>(int size) : IEnumerable<T> where T : IPoint
{
    private readonly T[] vectors = new T[size];
    private int insertIndex = 0;
    private int actualCount;

    public event Action? Updated;

    public int Count => actualCount;

    public T this[int index] => vectors[index];

    public void Add(T value)
    {
        if (insertIndex >= vectors.Length)
        {
            insertIndex = 0;
        }

        vectors[insertIndex++] = value;

        if (actualCount < vectors.Length)
        {
            actualCount++;
        }

        Updated?.Invoke();
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < actualCount; i++)
        {
            yield return vectors[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}