using System;
using System.Collections;
using System.Collections.Generic;
using Collections.DataStructure.Common;

namespace Collections.DataStructure.Lists;

public class DoublyLinkedList<T> : IEnumerable<T> where T : IComparable<T>
{
    private int _count;
    private DoublyLinkedListNode<T> _firstNode;
    private DoublyLinkedListNode<T> _lastNode;

    public virtual DoublyLinkedListNode<T> Head => _firstNode;
    public virtual int Count => _count;

    public virtual bool IsEmpty => Count == 0;

    public virtual T First
    {
        get
        {
            if (IsEmpty) throw new Exception("Empty list.");
            return _firstNode.Data;
        }
    }

    public virtual T Last
    {
        get
        {
            if (IsEmpty) throw new Exception("Empty list.");
            if (_lastNode != null) return _lastNode.Data;
            var currentNode = _firstNode;
            while (currentNode.Next != null) currentNode = currentNode.Next;
            _lastNode = currentNode;
            return currentNode.Data;
        }
    }

    public virtual T this[int index]
    {
        get => _getElementAt(index);
        set => _setElementAt(index, value);
    }

    /// <summary>
    /// Gets the element at the specified index
    /// </summary>
    /// <param name="index">Index of element</param>
    /// <returns>Element</returns>
    protected virtual T _getElementAt(int index)
    {
        if (IsEmpty || index < 0 || index >= Count)
            throw new IndexOutOfRangeException("List is empty.");
        if (index == 0) return First;
        if (index == Count - 1) return Last;
        DoublyLinkedListNode<T> currentNode;
        // Decide from which reference to traverse the list, and then move the currentNode reference to the index.
        // If index > half then traverse it from the end (_lastNode reference).
        // Otherwise, traverse it from the beginning (_firstNode reference).
        if (index > Count / 2)
        {
            currentNode = _lastNode;
            for (var i = Count - 1; i > index; --i) currentNode = currentNode.Previous;
        }
        else
        {
            currentNode = _firstNode;
            for (var i = 0; i < index; ++i) currentNode = currentNode.Next;
        }

        return currentNode.Data;
    }

    /// <summary>
    /// Sets the value of the element at the specified index
    /// </summary>
    /// <param name="index">Index of element to update.</param>
    /// <param name="value">The Data of the element.</param>
    /// <returns>Element</returns>
    protected virtual void _setElementAt(int index, T value)
    {
        if (IsEmpty || index < 0 || index >= Count)
            throw new IndexOutOfRangeException("List is empty.");
        if (index == 0)
        {
            _firstNode.Data = value;
        }
        else if (index == (Count - 1))
        {
            _lastNode.Data = value;
        }
        else
        {
            DoublyLinkedListNode<T> currentNode;
            // Decide from which reference to traverse the list, and then move the currentNode reference to the index.
            // If index > half then traverse it from the end (_lastNode reference).
            // Otherwise, traverse it from the beginning (_firstNode reference).
            if (index > Count / 2)
            {
                currentNode = _lastNode;
                for (var i = (Count - 1); i > index; --i) currentNode = currentNode.Previous;
            }
            else
            {
                currentNode = _firstNode;
                for (var i = 0; i < index; ++i) currentNode = currentNode.Next;
            }

            currentNode.Data = value;
        }
    }

    public virtual int IndexOf(T dataItem)
    {
        var i = 0;
        var found = false;
        var currentNode = _firstNode;
        while (i < Count)
        {
            if (currentNode.Data.IsEqualTo(dataItem))
            {
                found = true;
                break;
            }

            currentNode = currentNode.Next;
            i++;
        }

        return found ? i : -1;
    }

    /// <summary>
    /// Prepend the specified dataItem at the beginning of the list.
    /// </summary>
    /// <param name="dataItem">Data item.</param>
    public virtual void Prepend(T dataItem)
    {
        var newNode = new DoublyLinkedListNode<T>(dataItem);
        if (_firstNode == null)
        {
            _firstNode = _lastNode = newNode;
        }
        else
        {
            var currentNode = _firstNode;
            newNode.Next = currentNode;
            currentNode.Previous = newNode;
            _firstNode = newNode;
        }

        _count++;
    }

    /// <summary>
    /// Append the specified dataItem at the end of the list.
    /// </summary>
    /// <param name="dataItem">Data item.</param>
    public virtual void Append(T dataItem)
    {
        DoublyLinkedListNode<T> newNode = new DoublyLinkedListNode<T>(dataItem);

        if (_firstNode == null)
        {
            _firstNode = _lastNode = newNode;
        }
        else
        {
            var currentNode = _lastNode;
            currentNode.Next = newNode;
            newNode.Previous = currentNode;
            _lastNode = newNode;
        }

        _count++;
    }

    /// <summary>
    /// Inserts the dataItem at the specified index.
    /// </summary>
    /// <param name="dataItem">Data item.</param>
    /// <param name="index">Index.</param>
    public virtual void InsertAt(T dataItem, int index)
    {
        if (index < 0 || index > Count) throw new IndexOutOfRangeException();
        if (index == 0)
        {
            Prepend(dataItem);
        }
        else if (index == Count)
        {
            Append(dataItem);
        }
        else
        {
            DoublyLinkedListNode<T> currentNode;
            var newNode = new DoublyLinkedListNode<T>(dataItem);
            currentNode = _firstNode;
            for (var i = 0; i < index - 1; ++i) currentNode = currentNode.Next;
            var oldNext = currentNode.Next;
            if (oldNext != null) currentNode.Next.Previous = newNode;
            newNode.Next = oldNext;
            currentNode.Next = newNode;
            newNode.Previous = currentNode;
            _count++;
        }
    }

    /// <summary>
    /// Inserts the dataItem after specified index.
    /// </summary>
    /// <param name="dataItem">Data item.</param>
    /// <param name="index">Index.</param>
    public virtual void InsertAfter(T dataItem, int index)
    {
        InsertAt(dataItem, index - 1);
    }

    /// <summary>
    /// Remove the specified dataItem.
    /// </summary>
    public virtual void Remove(T dataItem)
    {
        if (IsEmpty) throw new IndexOutOfRangeException();

        if (_firstNode.Data.IsEqualTo(dataItem))
        {
            _firstNode = _firstNode.Next;
            if (_firstNode != null) _firstNode.Previous = null;
        }
        else if (_lastNode.Data.IsEqualTo(dataItem))
        {
            _lastNode = _lastNode.Previous;
            if (_lastNode != null) _lastNode.Next = null;
        }
        else
        {
            var currentNode = _firstNode;
            while (currentNode.Next != null)
            {
                if (currentNode.Data.IsEqualTo(dataItem)) break;
                currentNode = currentNode.Next;
            }

            if (!currentNode.Data.IsEqualTo(dataItem)) throw new Exception("Item was not found!");
            var newPrevious = currentNode.Previous;
            var newNext = currentNode.Next;
            if (newPrevious != null) newPrevious.Next = newNext;
            if (newNext != null) newNext.Previous = newPrevious;
            // currentNode = newPrevious;
        }

        _count--;
    }

    /// <summary>
    /// Removes the specified dataItem.
    /// </summary>
    public virtual void RemoveFirst(Predicate<T> match)
    {
        if (IsEmpty) throw new IndexOutOfRangeException();
        if (match(_firstNode.Data))
        {
            _firstNode = _firstNode.Next;
            if (_firstNode != null) _firstNode.Previous = null;
        }
        else if (match(_lastNode.Data))
        {
            _lastNode = _lastNode.Previous;
            if (_lastNode != null) _lastNode.Next = null;
        }
        else
        {
            var currentNode = _firstNode;
            while (currentNode.Next != null)
            {
                if (match(currentNode.Data)) break;
                currentNode = currentNode.Next;
            }

            if (!match(currentNode.Data)) throw new Exception("Item was not found!");
            var newPrevious = currentNode.Previous;
            var newNext = currentNode.Next;
            if (newPrevious != null) newPrevious.Next = newNext;
            if (newNext != null) newNext.Previous = newPrevious;
            // currentNode = newPrevious;
        }

        _count--;
    }

    /// <summary>
    /// Removes the item at the specified index.
    /// </summary>
    /// <returns>True if removed successfully, false otherwise.</returns>
    /// <param name="index">Index of item.</param>
    public virtual void RemoveAt(int index)
    {
        if (IsEmpty || index < 0 || index >= Count) throw new IndexOutOfRangeException();
        if (index == 0)
        {
            _firstNode = _firstNode.Next;
            if (_firstNode != null)
                _firstNode.Previous = null;
        }
        else if (index == Count - 1)
        {
            _lastNode = _lastNode.Previous;
            if (_lastNode != null)
                _lastNode.Next = null;
        }
        else
        {
            var i = 0;
            var currentNode = _firstNode;
            while (i < index)
            {
                currentNode = currentNode.Next;
                i++;
            }

            var newPrevious = currentNode.Previous;
            var newNext = currentNode.Next;
            newPrevious.Next = newNext;
            if (newNext != null)
                newNext.Previous = newPrevious;
            // currentNode = newPrevious;
        }

        _count--;
    }

    public virtual void Clear()
    {
        _count = 0;
        _firstNode = _lastNode = null;
    }

    /// <summary>
    /// Checks whether the specified element exists in the list.
    /// </summary>
    /// <param name="dataItem">Value to check for.</param>
    /// <returns>True if found; false otherwise.</returns>
    public virtual bool Contains(T dataItem)
    {
        try
        {
            return Find(dataItem).IsEqualTo(dataItem);
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Finds the specified item in the list.
    /// </summary>
    /// <param name="dataItem">Value to find.</param>
    /// <returns>value.</returns>
    public virtual T Find(T dataItem)
    {
        if (IsEmpty) throw new Exception("List is empty.");
        var currentNode = _firstNode;
        while (currentNode != null)
        {
            if (currentNode.Data.IsEqualTo(dataItem)) return dataItem;
            currentNode = currentNode.Next;
        }

        throw new Exception("Item was not found.");
    }

    /// <summary>
    /// Tries to find a match for the predicate. Returns true if found; otherwise false.
    /// </summary>
    public virtual bool TryFindFirst(Predicate<T> match, out T found)
    {
        found = default;
        if (IsEmpty) return false;
        var currentNode = _firstNode;
        try
        {
            while (currentNode != null)
            {
                if (match(currentNode.Data))
                {
                    found = currentNode.Data;
                    return true;
                }

                currentNode = currentNode.Next;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    public virtual T FindFirst(Predicate<T> match)
    {
        if (IsEmpty) throw new Exception("List is empty.");
        var currentNode = _firstNode;
        while (currentNode != null)
        {
            if (match(currentNode.Data)) return currentNode.Data;

            currentNode = currentNode.Next;
        }

        throw new KeyNotFoundException();
    }

    /// <summary>
    /// Finds all elements in list that match the predicate.
    /// </summary>
    /// <param name="match">Predicate function.</param>
    /// <returns>List of elements.</returns>
    public virtual List<T> FindAll(Predicate<T> match)
    {
        if (IsEmpty) throw new Exception("List is empty.");
        var currentNode = _firstNode;
        var list = new List<T>();
        while (currentNode != null)
        {
            if (match(currentNode.Data)) list.Add(currentNode.Data);
            currentNode = currentNode.Next;
        }

        return list;
    }

    /// <summary>
    /// Gets a number of elements as specified by countOfElements, starting from the specified index.
    /// </summary>
    /// <param name="index">Starting index.</param>
    /// <param name="countOfElements">The number of elements to return.</param>
    /// <returns>Doubly-Linked List of elements</returns>
    public virtual DoublyLinkedList<T> GetRange(int index, int countOfElements)
    {
        DoublyLinkedListNode<T> currentNode;
        var newList = new DoublyLinkedList<T>();
        if (Count == 0) return newList;
        if (index < 0 || index > Count) throw new IndexOutOfRangeException();
        // Decide from which reference to traverse the list, and then move the currentNode reference to the index
        // If index > half then traverse it from the end (_lastNode reference)
        // Otherwise, traverse it from the beginning (_firstNode reference)
        if (index > Count / 2)
        {
            currentNode = _lastNode;
            for (var i = Count - 1; i > index; --i) currentNode = currentNode.Previous;
        }
        else
        {
            currentNode = _firstNode;
            for (var i = 0; i < index; ++i) currentNode = currentNode.Next;
        }

        while (currentNode != null && newList.Count <= countOfElements)
        {
            newList.Append(currentNode.Data);
            currentNode = currentNode.Next;
        }

        return newList;
    }

    /// <summary>
    /// Sorts the entire list using Selection Sort.
    /// </summary>
    public virtual void SelectionSort()
    {
        if (IsEmpty) return;
        var currentNode = _firstNode;
        while (currentNode != null)
        {
            var minNode = currentNode;
            var nextNode = currentNode.Next;
            while (nextNode != null)
            {
                if (nextNode.Data.IsLessThan(minNode.Data)) minNode = nextNode;
                nextNode = nextNode.Next;
            }

            if (minNode != currentNode)
                (minNode.Data, currentNode.Data) = (currentNode.Data, minNode.Data);
            currentNode = currentNode.Next;
        }
    }

    public virtual T[] ToArray()
    {
        var array = new T[Count];
        var currentNode = _firstNode;
        for (var i = 0; i < Count; ++i)
            if (currentNode != null)
            {
                array[i] = currentNode.Data;
                currentNode = currentNode.Next;
            }
            else
            {
                break;
            }

        return array;
    }

    /// <summary>
    /// Returns a System.List version of this DLList instance.
    /// </summary>
    /// <returns>System.List of elements</returns>
    public virtual List<T> ToList()
    {
        var list = new List<T>(Count);
        var currentNode = _firstNode;
        for (var i = 0; i < Count; ++i)
            if (currentNode != null)
            {
                list.Add(currentNode.Data);
                currentNode = currentNode.Next;
            }
            else
            {
                break;
            }

        return list;
    }

    /// <summary>
    /// Returns the list items as a readable multi--line string.
    /// </summary>
    /// <returns></returns>
    public virtual string ToHumanReadable()
    {
        var listAsString = string.Empty;
        var i = 0;
        var currentNode = _firstNode;
        while (currentNode != null)
        {
            listAsString = $"{listAsString}[{i}] => {currentNode.Data}\r\n";
            currentNode = currentNode.Next;
            ++i;
        }

        return listAsString;
    }

    public IEnumerator<T> GetEnumerator()
    {
        var node = _firstNode;
        while (node != null)
        {
            yield return node.Data;
            node = node.Next;
        }

        // Alternative: IEnumerator class instance
        // return new DLinkedListEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    internal class DoublyLinkedListEnumerator(DoublyLinkedList<T> list) : IEnumerator<T>
    {
        private DoublyLinkedListNode<T> _current = list.Head;
        private DoublyLinkedList<T> _doublyLinkedList = list;

        public T Current => _current.Data;
        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (_current.Next != null)
                _current = _current.Next;
            else
                return false;

            return true;
        }

        public bool MovePrevious()
        {
            if (_current.Previous != null)
                _current = _current.Previous;
            else
                return false;

            return true;
        }

        public void Reset()
        {
            _current = _doublyLinkedList.Head;
        }

        public void Dispose()
        {
            _current = null;
            _doublyLinkedList = null;
        }
    }
}