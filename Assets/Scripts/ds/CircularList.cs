using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularList<T> : ICollection<T>
{
    List<T> list;
    int index = 0;

    public CircularList() 
    {
        list = new List<T>();
    }

    public CircularList(List<T> list) 
    {
        this.list = list;
    }

    public T GetNext() 
    {
        index %= list.Count;
        return list[index++];
    }

    public T GetPrev() 
    {
        index--;
        
        if (index < 0) 
        {
            index = list.Count - 1;
        }

        return list[index];
    }

    public T GetCurrent() 
    {
        return list[index];
    }

    public int Count => list.Count;

    public bool IsReadOnly => false;

    public void Add(T item)
    {
        list.Add(item);
    }

    public void Clear()
    {
        list.Clear();
    }

    public bool Contains(T item)
    {
        return list.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        list.CopyTo(array, arrayIndex);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return list.GetEnumerator();
    }

    public bool Remove(T item)
    {
        return list.Remove(item);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new System.NotImplementedException();
    }
}
