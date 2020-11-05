using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

public abstract class UnitCommand
{
        
    public MapCell Target { get; }
    public UnitCommand(MapCell Target)
    {
        this.Target = Target;
    }
    public abstract IEnumerator PerformAction(Unit Unit, Skill Skill);
    public virtual bool IsDone(Unit Unit, Skill Skill)
    {
        return false;
    }
    public virtual bool CanBePerformed(Unit Unit)
    {
        return true;
    }
}