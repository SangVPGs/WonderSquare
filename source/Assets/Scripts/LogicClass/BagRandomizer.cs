using System.Collections.Generic;
using UnityEngine;

public class BagRandomizer
{
    private Queue<int> queue = new Queue<int>();

    public int Next()
    {
        EnsureBag(1);
        return queue.Dequeue();
    }

    public int Peek(int index = 0)
    {
        EnsureBag(index + 1);
        return GetFromQueue(index);
    }

    private void EnsureBag(int count)
    {
        while (queue.Count < count)
            FillBag();
    }

    private int GetFromQueue(int index)
    {
        int i = 0;
        foreach (var item in queue)
        {
            if (i == index)
                return item;
            i++;
        }
        return -1;
    }

    private void FillBag()
    {
        List<int> bag = new List<int>();

        for (int i = 0; i < 7; i++)
            bag.Add(i);

        for (int i = bag.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (bag[i], bag[j]) = (bag[j], bag[i]);
        }

        foreach (var piece in bag)
            queue.Enqueue(piece);
    }
}