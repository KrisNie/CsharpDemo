using System;

namespace Collections.DataStructure.Lists;

public class SinglyLinkedListNode<T> : IComparable<SinglyLinkedListNode<T>> where T : IComparable<T>
{
    // The data being stored in the node.
    private T _data;

    // A reference to the next node, null for last node.
    private SinglyLinkedListNode<T> _next;

    public SinglyLinkedListNode(T dataItem)
    {
        Next = null;
        Data = dataItem;
    }

    public T Data
    {
        get => _data;
        set => _data = value;
    }

    public SinglyLinkedListNode<T> Next
    {
        get => _next;
        set => _next = value;
    }

    public int CompareTo(SinglyLinkedListNode<T> other)
    {
        if (other == null) return -1;
        return Data.CompareTo(other.Data);
    }
}