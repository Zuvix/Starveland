using UnityEngine;

public class ResourceSource : ResourceSourceGeneric
{
    [HideInInspector]
    public Resource resource = null;
    protected override Resource RetrieveResource(int amount, out bool isDepleted)
    {
        Resource Result = this.resource.Subtract(amount);
        isDepleted = false;
        if (this.resource.IsDepleted())
        {
            isDepleted = true;
            Debug.Log("Destroying Resource Source");
            UnitManager.Instance.RemoveFromQueue(this);
            //this.CurrentCell.SetCellObject(null);
            this.CurrentCell.EraseCellObject();
            Destroy(this.gameObject);
        }
        return Result;
    }
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