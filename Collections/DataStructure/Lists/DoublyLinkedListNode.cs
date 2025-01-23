using System;

namespace Collections.DataStructure.Lists;

public class DoublyLinkedListNode<T> : IComparable<DoublyLinkedListNode<T>> where T : IComparable<T>
{
    private T _data;
    private DoublyLinkedListNode<T> _next;
    private DoublyLinkedListNode<T> _previous;

    public virtual T Data
    {
        get => _data;
        set => _data = value;
    }

    public virtual DoublyLinkedListNode<T> Next
    {
        get => _next;
        set => _next = value;
    }

    public virtual DoublyLinkedListNode<T> Previous
    {
        get => _previous;
        set => _previous = value;
    }

    public DoublyLinkedListNode(
        T dataItem,
        DoublyLinkedListNode<T> next = null,
        DoublyLinkedListNode<T> previous = null)
    {
        Data = dataItem;
        Next = next;
        Previous = previous;
    }

    public int CompareTo(DoublyLinkedListNode<T> other)
    {
        if (other == null) return -1;
        return Data.CompareTo(other.Data);
    }
}