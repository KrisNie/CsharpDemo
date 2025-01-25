using System;
using System.Collections.Generic;
using Collections.DataStructure.Common;
using Collections.DataStructure.List;

namespace Collections.DataStructure.Heap;

/// <summary>
/// A Complete Binary Tree. For every node in the tree, the value of the node is less than or equal to the values of its
/// children.
/// <list type="bullet">
///     <listheader>Time Complexity:</listheader>
///     <item>Find Min: O(1)</item>
///     <item>Extract Min: O(log(n))</item>
///     <item>Increase Key: O(log(n))</item>
///     <item>Insert: O(log(n))</item>
///     <item>Delete: O(log(n))</item>
///     <item>Merge: O(m+n)</item>
/// </list>
/// </summary>
/// <param name="capacity"></param>
/// <param name="comparer"></param>
/// <typeparam name="T"></typeparam>
public class BinaryMinHeap<T>(int capacity, Comparer<T> comparer) : IMinHeap<T>
    where T : IComparable<T>
{
    private ArrayList<T> _collection = new(capacity);
    private readonly Comparer<T> _heapComparer = comparer ?? Comparer<T>.Default;
    public int Count => _collection.Count;
    public bool IsEmpty => _collection.IsEmpty;

    public T this[int index]
    {
        get
        {
            if (index < 0 || index > Count || Count == 0) throw new IndexOutOfRangeException();
            return _collection[index];
        }
        set
        {
            if (index < 0 || index >= Count) throw new IndexOutOfRangeException();
            _collection[index] = value;
            if (index != 0 && _heapComparer.Compare(
                    _collection[index],
                    _collection[(index - 1) / 2]) < 0)
                SiftUp(index);
            else
                MinHeap(index, _collection.Count - 1);
        }
    }

    public BinaryMinHeap() : this(0, null)
    {
    }

    public BinaryMinHeap(Comparer<T> comparer) : this(0, comparer)
    {
    }

    public void Initialize(IList<T> newCollection)
    {
        if (newCollection.Count <= 0) return;
        _collection = new ArrayList<T>(newCollection.Count);
        for (var i = 0; i < newCollection.Count; ++i) _collection.InsertAt(newCollection[i], i);
        BuildMinHeap();
    }

    public void Add(T heapKey)
    {
        _collection.Add(heapKey);
        if (!IsEmpty) SiftUp(_collection.Count - 1);
    }

    public T Peek()
    {
        if (IsEmpty) throw new Exception("Heap is empty.");
        return _collection.First;
    }

    public void RemoveMin()
    {
        if (IsEmpty) throw new Exception("Heap is empty.");
        var last = _collection.Count - 1;
        _collection.Swap(0, last);
        _collection.RemoveAt(last);
        last--;
        MinHeap(0, last);
    }

    public T ExtractMin()
    {
        var min = Peek();
        RemoveMin();
        return min;
    }

    public void Clear()
    {
        if (IsEmpty) throw new Exception("Heap is empty.");
        _collection.Clear();
    }

    public void RebuildHeap()
    {
        BuildMinHeap();
    }

    public T[] ToArray()
    {
        return _collection.ToArray();
    }

    public List<T> ToList()
    {
        return _collection.ToList();
    }

    public IMaxHeap<T> ToMaxHeap()
    {
        var newMaxHeap = new BinaryMaxHeap<T>(Count, _heapComparer);
        newMaxHeap.Initialize(_collection.ToArray());
        return newMaxHeap;
    }

    /// <summary>
    /// Merges two heaps together, returns a new min-heap of both heaps' elements,
    /// ... and then destroys the original ones.
    /// </summary>
    public BinaryMinHeap<T> Merge(
        ref BinaryMinHeap<T> firstMinHeap,
        ref BinaryMinHeap<T> secondMinHeap)
    {
        if (firstMinHeap == null || secondMinHeap == null)
            throw new ArgumentNullException(
                $"{nameof(firstMinHeap)} or {nameof(secondMinHeap)} is null");
        var size = firstMinHeap.Count + secondMinHeap.Count;
        var newHeap = new BinaryMinHeap<T>(size, Comparer<T>.Default);
        // Insert into the new heap.
        while (firstMinHeap.IsEmpty == false) newHeap.Add(firstMinHeap.ExtractMin());
        while (secondMinHeap.IsEmpty == false) newHeap.Add(secondMinHeap.ExtractMin());
        // Destroy the two heaps.
        firstMinHeap = secondMinHeap = null;
        return newHeap;
    }

    public string ToHumanReadable()
    {
        return _collection.ToHumanReadable();
    }

    /// <summary>
    /// Builds a min heap from the inner array-list _collection.
    /// </summary>
    private void BuildMinHeap()
    {
        var lastIndex = _collection.Count - 1;
        var lastNodeWithChildren = lastIndex / 2;
        for (var node = lastNodeWithChildren; node >= 0; node--) MinHeap(node, lastIndex);
    }

    /// <summary>
    /// Used to restore heap condition after insertion
    /// </summary>
    private void SiftUp(int nodeIndex)
    {
        var parent = (nodeIndex - 1) / 2;
        while (_heapComparer.Compare(_collection[nodeIndex], _collection[parent]) < 0)
        {
            _collection.Swap(parent, nodeIndex);
            nodeIndex = parent;
            parent = (nodeIndex - 1) / 2;
        }
    }

    /// <summary>
    /// Used in Building a Min Heap.
    /// </summary>
    /// <typeparam name="T">Type of Heap elements</typeparam>
    /// <param name="nodeIndex">The node index to operate at.</param>
    /// <param name="lastIndex">The last index of collection to stop at.</param>
    private void MinHeap(int nodeIndex, int lastIndex)
    {
        // assume that the subtrees left(node) and right(node) are max-heaps
        var left = nodeIndex * 2 + 1;
        var right = left + 1;
        var smallest = nodeIndex;
        // If collection[left] < collection[nodeIndex]
        if (left <= lastIndex &&
            _heapComparer.Compare(_collection[left], _collection[nodeIndex]) < 0)
            smallest = left;
        // If collection[right] < collection[smallest]
        if (right <= lastIndex &&
            _heapComparer.Compare(_collection[right], _collection[smallest]) < 0)
            smallest = right;
        if (smallest == nodeIndex) return;
        _collection.Swap(nodeIndex, smallest);
        MinHeap(smallest, lastIndex);
    }
}