using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSource : CellObject
{
    [HideInInspector]
    public Resource resource = null;

    override protected  void Awake()
    {
        base.Awake();
        this.IsPossibleToAddToActionQueue = true;
    }
    public override void RightClickAction()
    {
        AddToActionQueueSimple();
    }
    public override ActivityState CreateActivityState()
    {
        return new ActivityStateGather(this.CurrentCell);
    }
	
    public Resource GatherResource(int amount, out bool depleted)
    {
        bool isDepleted = false;
        Resource Result = this.resource.Subtract(amount);
        //Debug.LogWarning(Result.itemInfo.name);
        if (this.resource.Amount <= 0)
        {
            isDepleted = true;
            Debug.Log("Destroying Resource Source");
            UnitManager.Instance.RemoveFromQueue(this);
            //this.CurrentCell.SetCellObject(null);
            this.CurrentCell.EraseCellObject();
            Destroy(this.gameObject);
        }
        if (Result == null)
        {
            Debug.LogWarning("Result=null v Gather resource");
        }
        depleted = isDepleted;
        return Result;
    }

    public ResourcePack rp;

    public void GenerateResources()
    {   

        this.resource = rp.UnpackPack();
    }

    public bool AddResource(Resource toAddResource)
    {
        if (resource == null)
        {
            resource.AddDestructive(toAddResource);
            return true;
        }
        else if (!resource.itemInfo.name.Equals(toAddResource.itemInfo.name))
        {
            Debug.Log("Trying to add resources of different type");
            return false;
        }
        else
        {
            resource.AddDestructive(toAddResource);
            return true;
        }
    }
}
