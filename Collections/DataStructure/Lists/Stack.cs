using System;
using System.Collections;
using System.Collections.Generic;

namespace Collections.DataStructure.Lists;

/// <summary>
/// A collection of elements with Pop & Push.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <see href="https://en.wikipedia.org/wiki/Stack_(abstract_data_type)" />
public class Stack<T> : IEnumerable<T> where T : IComparable<T>
{
    private readonly ArrayList<T> _collection;

    public int Count => _collection.Count;
    public bool IsEmpty => _collection.IsEmpty;

    public T Top
    {
        get
        {
            try
            {
                return _collection[^1];
            }
            catch (Exception)
            {
                throw new Exception("Stack is empty.");
            }
        }
    }

    public Stack()
    {
        // The internal collection is implemented as an array-based list.
        // See the ArrayList.cs for the list implementation.
        _collection = new ArrayList<T>();
    }


    public Stack(int initialCapacity)
    {
        if (initialCapacity < 0) throw new ArgumentOutOfRangeException();

        // The internal collection is implemented as an array-based list.
        // See the ArrayList.cs for the list implementation.
        _collection = new ArrayList<T>(initialCapacity);
    }

    public void Push(T dataItem)
    {
        _collection.Add(dataItem);
    }

    public T Pop()
    {
        if (Count <= 0) throw new Exception("Stack is empty.");
        var top = Top;
        _collection.RemoveAt(_collection.Count - 1);
        return top;
    }

    public T[] ToArray()
    {
        return _collection.ToArray();
    }

    /// <summary>
    /// Returns a human-readable, multi-line, print-out (string) of this stack.
    /// </summary>
    /// <returns>String.</returns>
    public string ToHumanReadable()
    {
        return _collection.ToHumanReadable();
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (var i = _collection.Count - 1; i >= 0; --i)
            yield return _collection[i];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}