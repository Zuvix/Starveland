using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ResourceSourceFactory : Singleton<ResourceSourceFactory>
{
    public GameObject ProduceResourceSource(int x, int y, String name)
    {
        GameObject Result = null;
        switch(name)
        {
            case "Wood":
                Result = MapControl.Instance.CreateGameObject(x, y, MapControl.Instance.forest);
                Result.GetComponent<ResourceSource>().Resources = new List<Resource>();
                Result.GetComponent<ResourceSource>().Resources.Add(new Resource(ItemManager.Instance.GetItem("Wood"), 4));
                break;
            case "DeadAnimal":
                Result = MapControl.Instance.CreateGameObject(x, y, MapControl.Instance.animal_dead);
                Result.GetComponent<ResourceSource>().Resources = new List<Resource>();
                Result.GetComponent<ResourceSource>().Resources.Add(new Resource(ItemManager.Instance.GetItem("Steak"), 4));
                break;
            default:
                break;
        }
        return Result;
    }
}
