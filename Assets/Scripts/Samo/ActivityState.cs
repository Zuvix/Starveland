using UnityEngine;
using System.Collections;
public abstract class ActivityState
{
    public abstract IEnumerator PerformAction(Unit Unit);
    public virtual void InitializeCommand(Unit Unit)
    {
        Unit.SetCommand(null);
    }
    public virtual ActivityState SetCommands(Unit Unit, Skill Skill)
    {
        return null;
    }
}
