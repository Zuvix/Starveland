using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BuildingStorage : Building
{
    public override void SetCurrentCell(MapCell Cell)
    {
        base.SetCurrentCell(Cell);
        MapControl.Instance.StorageList.Add(this.CurrentCell);
    }
    public void OnDestroy()
    {
        if (MapControl.Instance != null)
        {
            MapControl.Instance.StorageList.Remove(this.CurrentCell);
        }
    }
}
