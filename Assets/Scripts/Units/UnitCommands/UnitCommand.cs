using System.Collections;

public abstract class UnitCommand
{
    public MapCell Target { get; }
    public UnitCommand(MapCell Target)
    {
        this.Target = Target;
    }
    public abstract IEnumerator PerformAction(Unit Unit);
    public virtual bool IsDone(Unit Unit)
    {
        return false;
    }
    public virtual bool CanBePerformed(Unit Unit)
    {
        return true;
    }
}