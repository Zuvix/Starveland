using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ResourceSourceGeneric : CellObject
{
    override protected void Awake()
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
        Resource Result = this.RetrieveResource(amount, out bool isDepleted);
        //Debug.LogWarning(Result.itemInfo.name);

        if (Result == null)
        {
            Debug.LogWarning("Result=null v Gather resource");
        }
        depleted = isDepleted;
        return Result;
    }
    protected abstract Resource RetrieveResource(int amount, out bool isDepleted);
}
