public abstract class Building : CellObject
{
    protected override void Awake()
    {
        this.IsBlocking = true;
        this.IsSelectable = true;
        base.Awake();
    }
}
