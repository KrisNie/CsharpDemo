using System;
using System.Collections;
using System.Collections.Generic;

namespace Collections.DataStructure.Lists;

/// <summary>
/// A FIFO collection of elements with Enqueue & Dequeue.
/// </summary>
/// <see href="https://en.wikipedia.org/wiki/Queue_(abstract_data_type)" />
public class Queue<T> : IEnumerable<T> where T : IComparable<T>
{
    private int _size;
    private int _headPointer;
    private int _tailPointer;
    private T[] _collection;
    private const int DefaultCapacity = 8;
    private bool _defaultMaxCapacityIsX64 = true;
    private bool _isMaximumCapacityReached;
    private const int MaximumArrayLengthX64 = 0X7FEFFFFF; //x64
    private const int MaximumArrayLengthX86 = 0x8000000;  //x86

    public int Count => _size;
    public bool IsEmpty => _size == 0;
    public T Top => IsEmpty ? throw new Exception("Queue is empty.") : _collection[_headPointer];

    public Queue() : this(DefaultCapacity)
    {
    }

    public Queue(int initialCapacity)
    {
        if (initialCapacity < 0) throw new ArgumentOutOfRangeException();
        _size = 0;
        _headPointer = 0;
        _tailPointer = 0;
        _collection = new T[initialCapacity];
    }

    private void _resize(int newSize)
    {
        if (newSize <= _size || _isMaximumCapacityReached) return;
        var capacity = _collection.Length == 0 ? DefaultCapacity : _collection.Length * 2;
        // Allow the list to grow to maximum possible capacity (~2G elements) before encountering overflow.
        // Note that this check works even when _items.Length overflowed thanks to the (uint) cast
        var maxCapacity = _defaultMaxCapacityIsX64
            ? MaximumArrayLengthX64
            : MaximumArrayLengthX86;
        if (capacity < newSize) capacity = newSize;

        if (capacity >= maxCapacity)
        {
            capacity = maxCapacity - 1;
            _isMaximumCapacityReached = true;
        }

        try
        {
            var tempCollection = new T[newSize];
            Array.Copy(_collection, _headPointer, tempCollection, 0, _size);
            _collection = tempCollection;
        }
        catch (OutOfMemoryException)
        {
            if (_defaultMaxCapacityIsX64)
            {
                _defaultMaxCapacityIsX64 = false;
                _resize(capacity);
            }
            else
            {
                throw;
            }
        }
    }

    /// <summary>
    /// Inserts an element at the end of the queue
    /// </summary>
    /// <param name="dataItem">Element to be inserted.</param>
    public void Enqueue(T dataItem)
    {
        if (_size == _collection.Length) _resize(_collection.Length * 2);
        _collection[_tailPointer++] = dataItem;
        if (_tailPointer == _collection.Length)
            _tailPointer = 0;
        _size++;
    }

    /// <summary>
    /// Removes the Top Element from queue, and assigns it's value to the "top" parameter.
    /// </summary>
    /// <return>The top element container.</return>
    public T Dequeue()
    {
        if (IsEmpty)
            throw new Exception("Queue is empty.");

        var topItem = _collection[_headPointer];
        _collection[_headPointer] = default(T);
        // Decrement the size
        _size--;
        // Increment the head pointer
        _headPointer++;
        // Reset the pointer
        if (_headPointer == _collection.Length)
            _headPointer = 0;
        // Shrink the internal collection
        if (_size <= 0 || _collection.Length <= DefaultCapacity || _size > _collection.Length / 4)
            return topItem;
        // Get head and tail
        var head = _collection[_headPointer];
        var tail = _collection[_tailPointer];
        // Shrink
        _resize((_collection.Length / 3) * 2);
        // Update head and tail pointers
        _headPointer = Array.IndexOf(_collection, head);
        _tailPointer = Array.IndexOf(_collection, tail);
        return topItem;
    }

    public T[] ToArray()
    {
        var array = new T[_size];
        var j = 0;
        for (var i = 0; i < _size; ++i)
        {
            array[j] = _collection[_headPointer + i];
            j++;
        }

        return array;
    }

    /// <summary>
    /// Returns a human-readable, multi-line, print-out (string) of this queue.
    /// </summary>
    public string ToHumanReadable()
    {
        var array = ToArray();
        var listAsString = string.Empty;
        for (var i = 0; i < Count; ++i)
            listAsString = $"{listAsString}[{i}] => {array[i]}\r\n";
        return listAsString;
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (var i = _collection.Length - 1; i >= 0; --i)
            yield return _collection[i];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}