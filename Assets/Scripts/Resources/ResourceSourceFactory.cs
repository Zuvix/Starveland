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
}

class ResourceSourceFactory : Singleton<ResourceSourceFactory>
{
    public GameObject forest;
    public GameObject deadAnimal;
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
        if (createdResourceSource.Resources.Count==0)
        {
            Destroy(Result);
            Debug.LogWarning("Destroyed ResourceSource because it had no resources.");
            return null;
        }
        return Result;
    }
}

