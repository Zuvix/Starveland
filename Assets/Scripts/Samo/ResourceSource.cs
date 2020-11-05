using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSource : CellObject
{
    public List<Resource> Resources;

    //Adding few Resources as en example
    override protected  void Awake()
    {
        base.Awake();
        Resources = new List<Resource>();
        Resources.Add(new Resource("Wood", "temp", ResourceType.Wood, 500));
    }
    //TODO
    /*public Resource Gather()
    {
        if (Resources.Count > 1)
        {
            Flash();
            /*int random = Random.Range(0, Resources.Count);
            Resource itemToGive = Resources[random];
            Resources.Remove(itemToGive);
            return itemToGive;
        }
        else
        {
            Debug.Log("Resource source delpeted");
            Destroy(this.gameObject);
            return null;
        }
    }*/
}
