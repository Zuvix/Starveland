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
    }

    public Resource GatherResource(int amount)
    {
        Resource Result = this.Resources[0].Subtract(amount);
        //Debug.LogWarning(Result.itemInfo.name);
        if (this.Resources[0].Amount <= 0)
        {
            Debug.Log("Destroying Resource Source");
            this.CurrentCell.SetCellObject(null);
            Destroy(this.gameObject);
        }
        if (Result == null)
        {
            Debug.LogWarning("Result=null v Gather resource");
        }
        return Result;
    }

    private void OnMouseOver()
     {
         if (Input.GetMouseButtonDown(1))
         {
             UnitManager.Instance.AddActionToQueue(this);
         }
     }
    /*private void OnMouseDown()
    {
        UnitManager.Instance.AddActionToQueue(this.tag, this.CurrentCell.x, this.CurrentCell.y);
    }*/

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
