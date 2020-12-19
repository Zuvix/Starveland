public class IncubatedResourceSource : CellObject
{
    public int DaysToGrow;
    public RSObjects IncubatedResourceSourceType;
    void Start()
    {
        DaytimeCounter.Instance.OnDayOver.AddListener(Grow);
    }
    private void Grow()
    {
        this.DaysToGrow--;
        if (DaysToGrow <= 0)
        {
            int x = this.CurrentCell.x;
            int y = this.CurrentCell.y;
            this.CurrentCell.EraseCellObject();
            CellObjectFactory.Instance.ProduceResourceSource(x, y, IncubatedResourceSourceType);
        }
    }
}
