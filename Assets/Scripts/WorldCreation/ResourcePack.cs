using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ResourcePack 
{
    public Item item;
    public int minValue=1;
    public int maxValue =10;
    [Range(0.1f, 100.0f)]
    public float chance=100;

    public Resource UnpackPack()
    {
        float randomNum = UnityEngine.Random.Range(0, 100f);
        if (randomNum <= chance)
        {
            int variatedAmount = UnityEngine.Random.Range(minValue, maxValue);
            Resource createdResource = new Resource(item, variatedAmount);
            if (createdResource.Amount > 0)
            {
                return createdResource;
            }
        }
        return null;
    }
}
