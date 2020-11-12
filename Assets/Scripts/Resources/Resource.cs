using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource
{
    public string name;
    public string iconId;
    public ResourceType Type;
    public int Amount;
    public Resource()
    {
        this.Type = ResourceType.None;
        this.Amount = 0;
        this.name = "";
        this.iconId = "";
    }
    public Resource(ResourceType Type, int Amount)
    {
        this.Type = Type;
        this.Amount = Amount;
        this.name = "";
        this.iconId = "";
    }
    public Resource(string name, string iconId, ResourceType Type, int Amount)
    {
        this.name = name;
        this.iconId = iconId;
        this.Type = Type;
        this.Amount = Amount;
    }
    public void AddDestructive(Resource Resource)
    {
        // If this is trivial resource, fully change it to the added resource
        if (this.Type == ResourceType.None)
        {
            this.Type = Resource.Type;
            this.Amount = Resource.Amount;
        }
        // If we are adding trivial resource, we are basically not doing anything
        else if (Resource.Type == ResourceType.None)
        {
        }
        // If we are adding same resource type, this is our happy day scenario
        else if (Resource.Type == this.Type)
        {
            this.Amount += Resource.Amount;
        }
        // We are adding different resource types, which means a problem
        else
        {
            //TODO
            throw new Exception("Tried to add different resource types");
        }
        // Let's neutralize the added Resource
        Resource.Type = ResourceType.None;
        Resource.Amount = 0;
    }
    public Resource Deplete()
    {
        return this.Subtract(this.Amount);
    }
    public Resource Subtract(int Amount)
    {
        if (Amount < 0)
        {
            //TODO
            throw new Exception("Tried to subtract negative resource amount");
        }

        Resource Result = null;
        if (this.Type == ResourceType.None)
        {
            Result = new Resource();
        }
        else
        {
            int SubtractAmount = Amount > this.Amount ? this.Amount : Amount;
            this.Amount -= SubtractAmount;
            Result = new Resource(this.Type, SubtractAmount);
            if (this.Amount <= 0)
            {
                this.Type = ResourceType.None;
                this.Amount = 0;
            }
        }
        return Result;
    }
    public bool IsDepleted()
    {
        return this.Amount <= 0;
    }
}
