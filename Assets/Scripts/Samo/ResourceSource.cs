using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSource : CellObject
{
    public List<Resource> resources;
    
    //Adding few resources as en example
    override protected  void Awake()
    {
        base.Awake();
        resources = new List<Resource>();
        resources.Add(new Resource("Wood", "temp"));
        resources.Add(new Resource("Wood", "temp"));
        resources.Add(new Resource("Wood", "temp"));
    }
    public Resource Gather()
    {
        if (resources.Count > 1)
        {
            Flash();
            int random = Random.Range(0, resources.Count);
            Resource itemToGive = resources[random];
            resources.Remove(itemToGive);
            return itemToGive;
        }
        else
        {
            Debug.Log("Resource source delpeted");
            Destroy(this.gameObject);
            return null;
        }
    }


}
