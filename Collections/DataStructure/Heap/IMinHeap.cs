namespace Collections.DataStructure.Heap;

public interface IMinHeap<T> where T : System.IComparable<T>
{
    int Count { get; }
    bool IsEmpty { get; }

    /// <summary>
    /// Overrides the current heap from the specified newCollection.
    /// </summary>
    /// <param name="newCollection">New collection.</param>
    void Initialize(System.Collections.Generic.IList<T> newCollection);

    /// <summary>
    /// Adding a new key to the heap.
    /// </summary>
    /// <param name="heapKey">Heap key.</param>
    void Add(T heapKey);

    /// <summary>
    /// Find the minimum node of a min heap.
    /// </summary>
    /// <returns>The minimum.</returns>
    T Peek();

    /// <summary>
    /// Removes the node of minimum value from a min heap.
    /// </summary>
    void RemoveMin();

    /// <summary>
    /// Returns the node of minimum value from a min heap after removing it from the heap.
    /// </summary>
    /// <returns>The min.</returns>
    T ExtractMin();

    /// <summary>
    /// Clear this heap.
    /// </summary>
    void Clear();

    /// <summary>
    /// Rebuilds the heap.
    /// </summary>
    void RebuildHeap();

    /// <summary>
    /// Returns an array version of this heap.
    /// </summary>
    /// <returns>The array.</returns>
    T[] ToArray();

    /// <summary>
    /// Returns a list version of this heap.
    /// </summary>
    /// <returns>The list.</returns>
    System.Collections.Generic.List<T> ToList();

    /// <summary>
    /// Returns a new min heap that contains all elements of this heap.
    /// </summary>
    /// <returns>The min heap.</returns>
    IMaxHeap<T> ToMaxHeap();
}