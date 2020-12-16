using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemChoice
{
    public static T SelectRandomOutputItem<T>(IEnumerable<RandomOutputItem> Options)
    {
        T Result = default;

        int SelectedValue = Random.Range(0, 100);
        int Accumulator = 0;

        foreach (RandomOutputItem Item in Options)
        {
            Accumulator += Item.Probability;
            if (Accumulator >= SelectedValue)
            {
                Result = (T)Item.GetStoredItem();
                break;
            }
        }

        return Result;
    }
}
