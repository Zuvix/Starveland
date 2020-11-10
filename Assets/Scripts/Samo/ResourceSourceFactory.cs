using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ResourceSourceFactory : Singleton<ResourceSourceFactory>
{
    public GameObject ProduceResourceSource(int x, int y, ResourceType ResourceType)
    {
        GameObject Result = null;
        switch(ResourceType)
        {
            case ResourceType.Wood:
                Result = MapControl.Instance.CreateGameObject(x, y, MapControl.Instance.forest);
                Result.GetComponent<ResourceSource>().Resources.Add(new Resource("Wood", "temp", ResourceType.Wood, 4));
                break;
            default:
                break;
        }
        return Result;
    }
}
