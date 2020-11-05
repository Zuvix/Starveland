using UnityEngine;
using System.Collections;
public abstract class ActivityState
{
    public abstract IEnumerator PerformAction(Unit Unit);
    public virtual void InitializeCommand(Unit Unit)
    {
        Unit.CurrentCommand = null;
    }
}
