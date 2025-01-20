using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Collections.DataStructure.Lists;

/// <summary>
/// An Array-Based List Data Structure.
/// </summary>
/// <typeparam name="T"></typeparam>
public class ArrayList<T> : IEnumerable<T>
{
    private bool _defaultMaxCapacityIsX64 = true;
    private bool _isMaximumCapacityReached;
    private readonly T[] _emptyArray = new T[0];
    private const int _defaultCapacity = 8;
    private T[] _collection;
    private int _size;

    // The C# Maximum Array Length (before encountering overflow)
    // Reference: http://referencesource.microsoft.com/#mscorlib/system/array.cs,2d2b551eabe74985
    public const int MaximumArrayLengthX64 = 0X7FEFFFFF; //x64
    public const int MaximumArrayLengthX86 = 0x8000000;  //x86
    public int Count => _size;
    public int Capacity => _collection.Length;
    public bool IsEmpty => Count == 0;

    public T First
    {
        get
        {
            if (Count == 0) throw new IndexOutOfRangeException("List is empty.");
            return _collection[0];
        }
    }

    public T Last
    {
        get
        {
            if (IsEmpty) throw new IndexOutOfRangeException("List is empty.");
            return _collection[Count - 1];
        }
    }

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= _size) throw new IndexOutOfRangeException();
            return _collection[index];
        }

        set
        {
            if (index < 0 || index >= _size) throw new IndexOutOfRangeException();
            _collection[index] = value;
        }
    }

    public ArrayList() : this(capacity: 0)
    {
    }

    public ArrayList(int capacity)
    {
        _collection = capacity switch
        {
            < 0 => throw new ArgumentOutOfRangeException(),
            0   => _emptyArray,
            _   => new T[capacity]
        };

        _size = 0;
    }

    public void Add(T dataItem)
    {
        if (_size == _collection.Length) EnsureCapacity(_size + 1);
        _collection[_size++] = dataItem;
    }

    public void AddRange(IEnumerable<T> elements)
    {
        if (elements == null)
            throw new ArgumentNullException();
        var enumerable = elements.ToList();
        if ((uint)_size + enumerable.Count > MaximumArrayLengthX64)
            throw new OverflowException();
        if (!enumerable.Any()) return;
        EnsureCapacity(_size + enumerable.Count);
        foreach (var element in enumerable)
            Add(element);
    }


    public void AddRepeatedly(T value, int count)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException();
        if ((uint)_size + count > MaximumArrayLengthX64)
            throw new OverflowException();
        if (count <= 0) return;
        EnsureCapacity(_size + count);
        for (var i = 0; i < count; i++)
            Add(value);
    }

    public void InsertAt(T dataItem, int index)
    {
        if (index < 0 || index > _size)
            throw new IndexOutOfRangeException("Please provide a valid index.");
        if (_size == _collection.Length) EnsureCapacity(_size + 1);
        if (index < _size) Array.Copy(_collection, index, _collection, index + 1, (_size - index));
        _collection[index] = dataItem;
        _size++;
    }

    public bool Remove(T dataItem)
    {
        int index = IndexOf(dataItem);
        if (index < 0) return false;
        RemoveAt(index);
        return true;
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= _size)
            throw new IndexOutOfRangeException("Please pass a valid index.");
        _size--;
        if (index < _size) Array.Copy(_collection, index + 1, _collection, index, (_size - index));
        _collection[_size] = default;
    }

    public void Clear()
    {
        if (_size <= 0) return;
        _size = 0;
        Array.Clear(_collection, 0, _size);
        _collection = _emptyArray;
    }

    public void Resize(int newSize, T defaultValue = default)
    {
        int currentSize = Count;

        if (newSize < currentSize)
        {
            EnsureCapacity(newSize);
        }
        else if (newSize > currentSize)
        {
            // Optimisation step.
            // This is just to avoid multiple automatic capacity changes.
            if (newSize > _collection.Length)
                EnsureCapacity(newSize + 1);

            AddRange(Enumerable.Repeat(defaultValue, newSize - currentSize));
        }
    }

    public void Reverse()
    {
        Reverse(0, _size);
    }

    private void Reverse(int startIndex, int count)
    {
        if (startIndex < 0 || startIndex >= _size)
            throw new IndexOutOfRangeException("Please pass a valid starting index.");
        if (count < 0 || startIndex > _size - count) throw new ArgumentOutOfRangeException();
        Array.Reverse(_collection, startIndex, count);
    }

    public void ForEach(Action<T> action)
    {
        if (action == null) throw new ArgumentNullException();
        for (var i = 0; i < _size; ++i) action(_collection[i]);
    }

    public bool Contains(T dataItem)
    {
        // Null-value check
        if (dataItem == null)
        {
            for (var i = 0; i < _size; ++i)
                if (_collection[i] == null)
                    return true;
        }
        else
        {
            // Construct a default equality comparer for this Type T
            // Use it to get the equal match for the dataItem
            var comparer = EqualityComparer<T>.Default;
            for (var i = 0; i < _size; ++i)
                if (comparer.Equals(_collection[i], dataItem))
                    return true;
        }

        return false;
    }

    /// <summary>
    /// Ensures the capacity.
    /// </summary>
    /// <param name="minCapacity">Minimum capacity.</param>
    private void EnsureCapacity(int minCapacity)
    {
        if (_collection.Length >= minCapacity || _isMaximumCapacityReached) return;
        var capacity = _collection.Length == 0 ? _defaultCapacity : _collection.Length * 2;

        var maxCapacity = _defaultMaxCapacityIsX64
            ? MaximumArrayLengthX64
            : MaximumArrayLengthX86;

        if (capacity < minCapacity)
            capacity = minCapacity;

        if (capacity >= maxCapacity)
        {
            capacity = maxCapacity - 1;
            _isMaximumCapacityReached = true;
        }

        ResizeCapacity(capacity);
    }

    public bool Contains(T dataItem, IEqualityComparer<T> comparer)
    {
        // Null comparers are not allowed.
        if (comparer == null) throw new ArgumentNullException();
        // Null-value check
        if (dataItem == null)
        {
            for (var i = 0; i < _size; ++i)
                if (_collection[i] == null)
                    return true;
        }
        else
        {
            for (var i = 0; i < _size; ++i)
                if (comparer.Equals(_collection[i], dataItem))
                    return true;
        }

        return false;
    }

    public bool Exists(Predicate<T> searchMatch)
    {
        // Use the FindIndex to look through the collection
        // If the returned index != -1 then it does exist, otherwise it doesn't.
        return (FindIndex(searchMatch) != -1);
    }

    public int FindIndex(Predicate<T> searchMatch)
    {
        return FindIndex(0, _size, searchMatch);
    }

    public int FindIndex(int startIndex, Predicate<T> searchMatch)
    {
        return FindIndex(startIndex, _size - startIndex, searchMatch);
    }

    private int FindIndex(int startIndex, int count, Predicate<T> searchMatch)
    {
        // Check bound of startIndex
        if (startIndex < 0 || startIndex > _size)
            throw new IndexOutOfRangeException("Please pass a valid starting index.");
        // CHeck the bounds of count and startIndex with respect to _size
        if (count < 0 || startIndex > (_size - count)) throw new ArgumentOutOfRangeException();
        // Null match-predicates are not allowed
        if (searchMatch == null) throw new ArgumentNullException();
        // Start the search
        var endIndex = startIndex + count;
        for (var index = startIndex; index < endIndex; ++index)
            if (searchMatch(_collection[index]))
                return index;
        // Not found, return -1
        return -1;
    }

    public int IndexOf(T dataItem)
    {
        return IndexOf(dataItem, 0, _size);
    }

    public int IndexOf(T dataItem, int startIndex)
    {
        return IndexOf(dataItem, startIndex, _size);
    }

    private int IndexOf(T dataItem, int startIndex, int count)
    {
        // Check the bound of the starting index.
        if (startIndex < 0 || (uint)startIndex > (uint)_size)
            throw new IndexOutOfRangeException("Please pass a valid starting index.");

        // Check the bounds of count and starting index with respect to _size.
        if (count < 0 || startIndex > (_size - count)) throw new ArgumentOutOfRangeException();

        // Everything is cool, start looking for the index
        // Use the Array.IndexOf
        // Array.IndexOf has a O(n) running time complexity, where: "n = count - size".
        // Array.IndexOf uses EqualityComparer<T>.Default to return the index of element which loops
        // ... over all the elements in the range [startIndex,count) in the array.
        return Array.IndexOf(_collection, dataItem, startIndex, count);
    }

    public T Find(Predicate<T> searchMatch)
    {
        // Null Predicate functions are not allowed. 
        if (searchMatch == null) throw new ArgumentNullException();
        // Begin searching, and return the matched element
        for (var i = 0; i < _size; ++i)
            if (searchMatch(_collection[i]))
                return _collection[i];
        // Not found, return the default value of the type T.
        return default(T);
    }

    public ArrayList<T> FindAll(Predicate<T> searchMatch)
    {
        // Null Predicate functions are not allowed. 
        if (searchMatch == null) throw new ArgumentNullException();
        var matchedElements = new ArrayList<T>();
        // Begin searching, and add the matched elements to the new list.
        for (var i = 0; i < _size; ++i)
            if (searchMatch(_collection[i]))
                matchedElements.Add(_collection[i]);
        // Return the new list of elements.
        return matchedElements;
    }

    public ArrayList<T> GetRange(int startIndex, int count)
    {
        // Handle the bound errors of startIndex
        if (startIndex < 0 || (uint)startIndex > (uint)_size)
            throw new IndexOutOfRangeException("Please provide a valid starting index.");
        // Handle the bound errors of count and startIndex with respect to _size
        if (count < 0 || startIndex > _size - count) throw new ArgumentOutOfRangeException();
        var newArrayList = new ArrayList<T>(count);
        // Use Array.Copy to quickly copy the contents from this array to the new list's inner array.
        Array.Copy(_collection, startIndex, newArrayList._collection, 0, count);
        // Assign count to the new list's inner _size counter.
        newArrayList._size = count;
        return newArrayList;
    }

    public T[] ToArray()
    {
        var newArray = new T[Count];
        if (Count > 0) Array.Copy(_collection, 0, newArray, 0, Count);
        return newArray;
    }

    public List<T> ToList()
    {
        var list = new List<T>(Count);
        if (Count <= 0) return list;
        for (var i = 0; i < Count; ++i)
            list.Add(_collection[i]);
        return list;
    }

    /// <summary>
    /// Return a human-readable, multi-line, print-out (string) of this list.
    /// </summary>
    /// <returns>The human-readable string.</returns>
    /// <param name="addHeader">
    /// If set to <c>true</c> a header with count and Type is added; otherwise, only elements are
    /// printed.
    /// </param>
    public string ToHumanReadable(bool addHeader = false)
    {
        int i;
        var listAsString = string.Empty;
        var preLineIndent = addHeader == false ? "" : "\t";
        for (i = 0; i < Count; ++i)
            listAsString = $"{listAsString}{preLineIndent}[{i}] => {_collection[i]}\r\n";
        if (addHeader)
            listAsString = $"ArrayList of count: {Count}.\r\n(\r\n{listAsString})";
        return listAsString;
    }

    /// <summary>
    /// Resizes the collection to a new maximum number of capacity.
    /// </summary>
    /// <param name="newCapacity">New capacity.</param>
    private void ResizeCapacity(int newCapacity)
    {
        if (newCapacity != _collection.Length && newCapacity > _size)
            try
            {
                Array.Resize(ref _collection, newCapacity);
            }
            catch (OutOfMemoryException)
            {
                if (!_defaultMaxCapacityIsX64) throw;
                _defaultMaxCapacityIsX64 = false;
                EnsureCapacity(newCapacity);
                throw;
            }
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (var i = 0; i < Count; i++) yield return _collection[i];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}