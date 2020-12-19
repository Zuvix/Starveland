using System;
using UnityEngine;
[Serializable]
public class Resource
{
    public Item itemInfo;
    public int Amount;
    public Resource(Item item, int Amount)
    {
        this.Amount = Amount;
        itemInfo = item;
    }
    public Resource Duplicate()
    {
        return new Resource(this.itemInfo, this.Amount);
    }
    public void AddDestructive(Resource Resource)
    {
        if (this.itemInfo == null)
        {
            this.itemInfo = Resource.itemInfo;
            this.Amount = Resource.Amount;
        }
        else if (Resource.itemInfo.Equals(this.itemInfo))
        {
            this.Amount += Resource.Amount;
        }
        else
        {
            Debug.LogError($"Tried to add different resource types: {Resource.itemInfo.name} and {this.itemInfo}");
        }
    }
    public Resource Deplete()
    {
        return this.Subtract(this.Amount);
    }
    public Resource Subtract(int Amount)
    {
        Resource Result = null;
        if (Amount < 0)
        {
            Debug.LogError("Tried to subtract from negative resource amount");
        }
        else if(Amount>=this.Amount)
        {
            Result= new Resource(this.itemInfo,this.Amount);
            this.Amount = 0;
        }
        else if (Amount<this.Amount)
        {
            this.Amount -= Amount;
            Result = new Resource(this.itemInfo, Amount);
        }
        if (this.Amount <= 0)
        {
            this.itemInfo = null;
            this.Amount = 0;
        }
        return Result;
    }
    public bool IsDepleted()
    {
        return this.Amount <= 0;
    }
}
