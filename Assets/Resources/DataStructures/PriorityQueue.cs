using System;
using System.Collections.Generic;
using System.Linq;

public class PriorityQueue<T>
{
    private readonly SortedDictionary<int, Queue<T>> _storage;

    public PriorityQueue(IComparer<int> priorityComparer = null)
    {
        _storage = new SortedDictionary<int, Queue<T>>(priorityComparer ?? Comparer<int>.Default);
    }

    public int Count { get; private set; } = 0;

    public void Enqueue(T item, int priority)
    {
        if (!_storage.ContainsKey(priority))
        {
            _storage[priority] = new Queue<T>();
        }

        _storage[priority].Enqueue(item);
        Count++;
    }

    public T Dequeue()
    {
        if (Count == 0)
        {
            throw new InvalidOperationException("The priority queue is empty.");
        }

        int firstKey = _storage.Keys.Min();
        var queue = _storage[firstKey];
        T item = queue.Dequeue();
        Count--;

        if (queue.Count == 0)
        {
            _storage.Remove(firstKey);
        }

        return item;
    }

    public KeyValuePair<int, T> DequeueWithPrio()
    {
        if (Count == 0)
        {
            throw new InvalidOperationException("The priority queue is empty.");
        }

        int firstKey = _storage.Keys.Min();
        var queue = _storage[firstKey];
        T item = queue.Dequeue();
        Count--;

        if (queue.Count == 0)
        {
            _storage.Remove(firstKey);
        }

        return new KeyValuePair<int, T>(firstKey, item);
    }

    public T Peek()
    {
        if (Count == 0)
        {
            throw new InvalidOperationException("The priority queue is empty.");
        }

        int firstKey = _storage.Keys.Min();
        Queue<T> queue = _storage[firstKey];
        return queue.Peek();
    }

    public bool IsEmpty()
    {
        return Count == 0;
    }
}
