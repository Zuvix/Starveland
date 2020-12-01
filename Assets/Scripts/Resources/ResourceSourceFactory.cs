using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public enum RSObjects
{
    Forest,
    DeadAnimal,
    Stone,
    Bush,
    Bush_Berry_Red,
    Bush_Berry_Purple,
    Sapling
}

class ResourceSourceFactory : Singleton<ResourceSourceFactory>
{
    public GameObject forest;
    public GameObject deadAnimal;
    public GameObject stone;
    public GameObject bush;
    public GameObject bush_berry_red;
    public GameObject bush_berry_purple;
    public GameObject sapling;

    public GameObject ProduceResourceSource(int x, int y, RSObjects type, List<Resource> additionalResources=null)
    {
        GameObject Result = null;
        GameObject selectedPrefab = null;
        ResourceSource createdResourceSource = null;
        switch (type)
        {
            case RSObjects.Forest:
            {
                selectedPrefab = forest;
                break;
            }
            case RSObjects.DeadAnimal:
            {
                selectedPrefab = deadAnimal;
                break;
            }
            case RSObjects.Stone:
            {
                selectedPrefab = stone;
                break;
            }
            case RSObjects.Bush:
            {
                selectedPrefab = bush;
                break;
            }
            case RSObjects.Bush_Berry_Red:
            {
                selectedPrefab = bush_berry_red;
                break;
            }
            case RSObjects.Bush_Berry_Purple:
            {
                selectedPrefab = bush_berry_purple;
                break;
            }
            case RSObjects.Sapling:
            {
                selectedPrefab = sapling;
                break;
            }
            default:
                break;
        }
        Result = MapControl.Instance.CreateGameObject(x, y, selectedPrefab);
        if (Result != null)
        {
            createdResourceSource = Result.GetComponent<ResourceSource>();
            createdResourceSource?.GenerateResources();
        }
        if (additionalResources != null)
        {
            foreach(Resource newResource in additionalResources)
            {
                createdResourceSource.AddResource(newResource);
            }
        }
        if (createdResourceSource!=null)
        {
            if (createdResourceSource.Resources?.Count == 0)
            {
                Destroy(Result);
                Debug.LogWarning("Destroyed ResourceSource because it had no resources.");
                return null;
            }
        }
        return Result;
    }
}

