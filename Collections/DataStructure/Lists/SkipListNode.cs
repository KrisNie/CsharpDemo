using System;

namespace Collections.DataStructure.Lists;

public sealed class SkipListNode<T> : IComparable<SkipListNode<T>> where T : IComparable<T>
{
    public SkipListNode(T value, int level)
    {
        if (level < 0)
            throw new ArgumentOutOfRangeException(nameof(level));

        Value = value;
        Forwards = new SkipListNode<T>[level];
    }

    public T Value { get; private init; }

    public SkipListNode<T>[] Forwards { get; private set; }

    public int Level => Forwards.Length;

    public int CompareTo(SkipListNode<T> other)
    {
        if (other == null)
            return -1;

        return Value.CompareTo(other.Value);
    }
}