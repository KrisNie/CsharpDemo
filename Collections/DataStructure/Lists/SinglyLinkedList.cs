using System;
using System.Collections;
using System.Collections.Generic;
using Collections.DataStructure.Common;

namespace Collections.DataStructure.Lists;

/// <summary>
/// A Linked List with efficient insertion and deletion at the beginning.
/// While accessing elements in the middle or end can be less efficient than in arrays, its dynamic nature makes it
/// suitable for many applications where the size of the data needs to change frequently.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <see href="https://en.wikipedia.org/wiki/Linked_list#Singly_linked_lists" />
public class SinglyLinkedList<T> : IEnumerable<T> where T : IComparable<T>
{
    private int _count;
    private SinglyLinkedListNode<T> _firstNode;
    private SinglyLinkedListNode<T> _lastNode;

    public int Count => _count;
    public virtual SinglyLinkedListNode<T> Head => _firstNode;

    public virtual SinglyLinkedListNode<T> Tail
    {
        get
        {
            if (Count == 0) throw new Exception("Empty list.");
            if (_lastNode != null) return _lastNode;
            var currentNode = _firstNode;
            while (currentNode.Next != null) currentNode = currentNode.Next;
            _lastNode = currentNode;
            return _lastNode;
        }
    }

    public bool IsEmpty => Count == 0;
    public T First => _firstNode == null ? default : _firstNode.Data;

    public T Last
    {
        get
        {
            if (Count == 0) throw new Exception("Empty list.");

            if (_lastNode != null) return _lastNode.Data;
            var currentNode = _firstNode;
            while (currentNode.Next != null) currentNode = currentNode.Next;
            _lastNode = currentNode;
            return currentNode.Data;
        }
    }

    /// <summary>
    /// Warning: The Indexing Time Complexity of Singly Linked List is O(n).
    /// </summary>
    /// <param name="index"></param>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public T this[int index]
    {
        get
        {
            if (index == 0) return First;
            if (index == Count - 1) return Last;
            if (index <= 0 || index >= Count - 1) throw new IndexOutOfRangeException();
            var currentNode = _firstNode;
            for (var i = 0; i < index; ++i)
                currentNode = currentNode.Next;
            return currentNode.Data;
        }
        set
        {
            if (index == 0)
            {
                Prepend(value);
            }
            else if (index == Count)
            {
                Append(value);
            }
            else if (index > 0 && index < Count)
            {
                var currentNode = _firstNode;
                var newNode = new SinglyLinkedListNode<T>(value);
                for (var i = 1; i < index; ++i) currentNode = currentNode.Next;
                newNode.Next = currentNode.Next;
                currentNode.Next = newNode;
                _count++;
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }
    }

    /// <summary>
    /// Appends the specified dataItem at the beginning of the list.
    /// </summary>
    /// <param name="dataItem">The data value to be inserted to the list.</param>
    public void Prepend(T dataItem)
    {
        var newNode = new SinglyLinkedListNode<T>(dataItem);

        if (_firstNode == null)
        {
            _firstNode = _lastNode = newNode;
        }
        else
        {
            var currentNode = _firstNode;
            newNode.Next = currentNode;
            _firstNode = newNode;
        }

        _count++;
    }

    /// <summary>
    /// Appends the specified dataItem at the end of the list.
    /// </summary>
    /// <param name="dataItem">The data value to be inserted to the list.</param>
    public void Append(T dataItem)
    {
        var newNode = new SinglyLinkedListNode<T>(dataItem);

        if (_firstNode == null)
        {
            _firstNode = _lastNode = newNode;
        }
        else
        {
            var currentNode = _lastNode;
            currentNode.Next = newNode;
            _lastNode = newNode;
        }

        _count++;
    }

    /// <summary>
    /// Removes the item at the specified index.
    /// </summary>
    /// <param name="index">The index of the list node to be removed.</param>
    public void RemoveAt(int index)
    {
        if (IsEmpty || index < 0 || index >= Count)
            throw new IndexOutOfRangeException();
        if (index == 0)
        {
            _firstNode = _firstNode.Next;
            _count--;
        }
        else if (index == Count - 1)
        {
            var currentNode = _firstNode;
            while (currentNode.Next != null && currentNode.Next != _lastNode)
                currentNode = currentNode.Next;
            currentNode.Next = null;
            _lastNode = currentNode;
            _count--;
        }
        else
        {
            var i = 0;
            var currentNode = _firstNode;
            while (currentNode.Next != null)
            {
                if (i + 1 == index)
                {
                    currentNode.Next = currentNode.Next.Next;
                    _count--;
                    break;
                }

                ++i;
                currentNode = currentNode.Next;
            }
        }
    }

    public void Clear()
    {
        _firstNode = null;
        _lastNode = null;
        _count = 0;
    }


    /// <summary>
    /// Gets a number of elements as specified by countOfElements, starting from the specified index.
    /// </summary>
    /// <param name="index">Starting index.</param>
    /// <param name="countOfElements">The number of elements to return.</param>
    /// <returns>Singly-Linked List of elements</returns>
    public SinglyLinkedList<T> GetRange(int index, int countOfElements)
    {
        var newList = new SinglyLinkedList<T>();
        var currentNode = _firstNode;
        if (Count == 0) return newList;
        if (index < 0 || index > Count) throw new IndexOutOfRangeException();
        for (var i = 0; i < index; ++i) currentNode = currentNode.Next;

        // Append the elements to the new list using the currentNode reference
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

    public T[] ToArray()
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

    public List<T> ToList()
    {
        var list = new List<T>();
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
    public string ToHumanReadable()
    {
        var i = 0;
        var currentNode = _firstNode;
        var listAsString = string.Empty;

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
        return new SLinkedListEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private class SLinkedListEnumerator(SinglyLinkedList<T> list) : IEnumerator<T>
    {
        private SinglyLinkedListNode<T> _current = list.Head;
        private SinglyLinkedList<T> _doublyLinkedList = list;

        public T Current => _current.Data;
        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            _current = _current.Next;
            return _current != null;
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