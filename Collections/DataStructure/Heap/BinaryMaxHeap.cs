using System;
using System.Collections.Generic;
using Collections.DataStructure.Common;
using Collections.DataStructure.List;

namespace Collections.DataStructure.Heap;

/// <summary>
/// A Complete Binary Tree. For every node in the tree, the value of the node is greater than or equal to the values of its
/// children.
/// <list type="bullet">
///     <listheader>Time Complexity:</listheader>
///     <item>Find Max: O(1)</item>
///     <item>Extract Max: O(log(n))</item>
///     <item>Increase Key: O(log(n))</item>
///     <item>Insert: O(log(n))</item>
///     <item>Delete: O(log(n))</item>
///     <item>Merge: O(m+n)</item>
/// </list>
/// </summary>
/// <param name="capacity"></param>
/// <param name="comparer"></param>
/// <typeparam name="T"></typeparam>
public class BinaryMaxHeap<T>(int capacity, Comparer<T> comparer) : IMaxHeap<T>
    where T : IComparable<T>
{
    private ArrayList<T> _collection = new(capacity);
    private readonly Comparer<T> _heapComparer = comparer ?? Comparer<T>.Default;

    public int Count => _collection.Count;
    public bool IsEmpty => _collection.IsEmpty;

    public BinaryMaxHeap() : this(0, null)
    {
    }

    public BinaryMaxHeap(Comparer<T> comparer) : this(0, comparer)
    {
    }

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
                    _collection[(index - 1) / 2]) > 0)
                SiftUp(index);
            else
                MaxHeap(index, _collection.Count - 1);
        }
    }

    public void Initialize(IList<T> newCollection)
    {
        if (newCollection.Count <= 0) return;
        // Reset and reserve the size of the newCollection
        _collection = new ArrayList<T>(newCollection.Count);
        // Copy the elements from the newCollection to the inner collection
        for (var i = 0; i < newCollection.Count; ++i) _collection.InsertAt(newCollection[i], i);
        // Build the heap
        BuildMaxHeap();
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

    public void RemoveMax()
    {
        if (IsEmpty) throw new Exception("Heap is empty.");
        var max = 0;
        var last = _collection.Count - 1;
        _collection.Swap(max, last);
        _collection.RemoveAt(last);
        last--;
        MaxHeap(0, last);
    }

    public T ExtractMax()
    {
        var max = Peek();
        RemoveMax();
        return max;
    }

    public void Clear()
    {
        if (IsEmpty) throw new Exception("Heap is empty.");

        _collection.Clear();
    }

    public void RebuildHeap()
    {
        BuildMaxHeap();
    }

    public T[] ToArray()
    {
        return _collection.ToArray();
    }

    public List<T> ToList()
    {
        return _collection.ToList();
    }

    public IMinHeap<T> ToMinHeap()
    {
        var newMinHeap = new BinaryMinHeap<T>(Count, _heapComparer);
        newMinHeap.Initialize(_collection.ToArray());
        return newMinHeap;
    }

    /// <summary>
    /// Merges two heaps together, returns a new min-heap of both heaps' elements,
    /// ... and then destroys the original ones.
    /// </summary>
    public BinaryMaxHeap<T> Merge(
        ref BinaryMaxHeap<T> firstMaxHeap,
        ref BinaryMaxHeap<T> secondMaxHeap)
    {
        if (firstMaxHeap == null || secondMaxHeap == null)
            throw new ArgumentNullException(
                $"{nameof(firstMaxHeap)} or {nameof(secondMaxHeap)} is null");
        // Create a new heap with reserved size.
        var size = firstMaxHeap.Count + secondMaxHeap.Count;
        var newHeap = new BinaryMaxHeap<T>(size, Comparer<T>.Default);
        // Insert into the new heap.
        while (firstMaxHeap.IsEmpty == false) newHeap.Add(firstMaxHeap.ExtractMax());
        while (secondMaxHeap.IsEmpty == false)
            newHeap.Add(secondMaxHeap.ExtractMax());
        // Destroy the two heaps.
        firstMaxHeap = secondMaxHeap = null;
        return newHeap;
    }

    public string ToHumanReadable()
    {
        return _collection.ToHumanReadable();
    }

    /// <summary>
    /// Builds a max heap from the inner array-list _collection.
    /// </summary>
    private void BuildMaxHeap()
    {
        var lastIndex = _collection.Count - 1;
        var lastNodeWithChildren = (lastIndex / 2);
        for (var node = lastNodeWithChildren; node >= 0; node--) MaxHeap(node, lastIndex);
    }

    /// <summary>
    /// Used to restore heap condition after insertion
    /// </summary>
    private void SiftUp(int nodeIndex)
    {
        var parent = (nodeIndex - 1) / 2;
        while (_heapComparer.Compare(_collection[nodeIndex], _collection[parent]) > 0)
        {
            _collection.Swap(parent, nodeIndex);
            nodeIndex = parent;
            parent = (nodeIndex - 1) / 2;
        }
    }

    /// <summary>
    /// Used in Building a Max Heap. Time Complexity is O(n)
    /// </summary>
    /// <typeparam name="T">Type of Heap elements</typeparam>
    /// <param name="nodeIndex">The node index to operate at.</param>
    /// <param name="lastIndex">The last index of collection to stop at.</param>
    private void MaxHeap(int nodeIndex, int lastIndex)
    {
        // assume that the subtrees left(node) and right(node) are max-heaps
        var left = nodeIndex * 2 + 1;
        var right = left + 1;
        var largest = nodeIndex;
        // If collection[left] > collection[nodeIndex]
        if (left <= lastIndex &&
            _heapComparer.Compare(_collection[left], _collection[nodeIndex]) > 0)
            largest = left;
        // If collection[right] > collection[largest]
        if (right <= lastIndex &&
            _heapComparer.Compare(_collection[right], _collection[largest]) > 0)
            largest = right;
        if (largest == nodeIndex) return;
        _collection.Swap(nodeIndex, largest);
        MaxHeap(largest, lastIndex);
    }
}