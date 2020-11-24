using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ResourcePack 
{
    public Resource resource;
    [Range(0.1f, 100.0f)]
    public float chance=100;
    [Range(0.0f, 100.0f)]
    public float variationPercentage=0;

    public Resource UnpackPack()
    {
        float randomNum = UnityEngine.Random.Range(0, 100f);
        if (randomNum <= chance)
        {
            float variation = resource.Amount * (variationPercentage / 100);
            int variatedAmount = (int)Mathf.Round(UnityEngine.Random.Range(-variation, variation));
            resource.Amount += variatedAmount;
            if (resource.Amount > 0)
            {
                return resource;
            }
        }
        return null;
    }
}
