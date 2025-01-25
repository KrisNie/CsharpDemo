using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Collections.DataStructure.List;

/// <summary>
/// A single, fixed-size buffer as if it were connected end-to-end.
/// </summary>
/// <see href="https://en.wikipedia.org/wiki/Circular_buffer" />
/// <remarks>
/// Circular buffer. Given a fixed size, fills to capacity and then overwrites earliest item.
/// Microsoft.VisualStudio.Utilities.CircularBuffer.
/// </remarks>
public class CircularBuffer<T> : ICollection<T> where T : IComparable<T>
{
    private readonly T[] _buffer;
    private int _end;
    private int _start;
    private int _count;
    private const int DefaultBufferLength = 10;

    public int Capacity => _buffer.Length - 1;
    public int Count => _count;
    public bool IsReadOnly => false;
    public bool IsEmpty => _count == 0;

    public bool IsFull =>
        (_end + 1) % _buffer.Length == _start &&
        !_buffer[_start].Equals(_buffer[_end]);

    public bool CanOverride { get; }

    public CircularBuffer(bool canOverride = true) : this(DefaultBufferLength, canOverride)
    {
    }

    public CircularBuffer(int length, bool canOverride = true)
    {
        if (length < 1)
            throw new ArgumentOutOfRangeException(nameof(length));
        _buffer = new T[length + 1];
        _end = 0;
        _start = 0;
        CanOverride = canOverride;
    }

    public T this[int index]
    {
        get => _buffer[InternalIndex(index)];
        set => _buffer[InternalIndex(index)] = value;
    }

    private int InternalIndex(int index)
    {
        if (IsEmpty)
            throw new IndexOutOfRangeException($"Cannot access index {index}. Buffer is empty");
        if (index >= Count)
            throw new IndexOutOfRangeException(
                $"Cannot access index {index}. Buffer size is {Count}");
        return (_start + index) % Capacity;
    }

    public void Add(T item)
    {
        if (CanOverride == false && IsFull)
            throw new CircularBufferFullException();

        _buffer[_end] = item;
        _end = (_end + 1) % _buffer.Length;
        if (_end == _start) _start = (_start + 1) % _buffer.Length;

        // Count should not be greater than the length of the buffer when overriding 
        _count = _count < Capacity ? ++_count : _count;
    }

    public T Pop()
    {
        var result = _buffer[_start];
        _buffer[_start] = _buffer[_end];
        _start = (_start + 1) % _buffer.Length;
        //Count should not go below Zero when popping an empty buffer.
        _count = _count > 0 ? --_count : _count;
        return result;
    }

    public T[] ToArray()
    {
        if (IsEmpty) return [];
        var array = new T[Count];
        for (var index = 0; index < Count; ++index)
            array[index] = this[index];

        return array;
    }

    public void Clear()
    {
        _count = 0;
        _start = 0;
        _end = 0;
        Array.Clear(_buffer, 0, _buffer.Length);
    }

    public bool Contains(T item)
    {
        return _buffer.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        if (array == null) throw new ArgumentNullException(nameof(array));

        if (array.Length == 0 || arrayIndex >= array.Length || arrayIndex < 0)
            throw new IndexOutOfRangeException();

        // Get enumerator
        using var enumarator = GetEnumerator();

        // Copy elements if there is any in the buffer and if the index is within the valid range
        while (arrayIndex < array.Length)
            if (enumarator.MoveNext())
            {
                array[arrayIndex] = enumarator.Current;
                arrayIndex++;
            }
            else
            {
                break;
            }
    }

    public bool Remove(T item)
    {
        if (IsEmpty || !Contains(item)) return false;
        var sourceArray = _buffer.Except([item]).ToArray();
        Clear();
        Array.Copy(sourceArray, _buffer, sourceArray.Length);

        if (!Equals(item, default(T)))
        {
            _end = sourceArray.Length - 1;
            _count = sourceArray.Length - 1;
        }
        else
        {
            _end = sourceArray.Length;
            _count = sourceArray.Length;
        }

        return true;
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (var i = _start; i < Count; i++) yield return _buffer[i];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

// TODO: Move out and Multilingual.
public class CircularBufferFullException : Exception
{
    public CircularBufferFullException() : base(
        "Circular Buffer is filled up. Can not be added to the circular buffer")
    {
    }

    public CircularBufferFullException(string message) : base(message)
    {
    }

    public CircularBufferFullException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}