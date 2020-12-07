using System;
using System.Collections;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UnitCommandEnterBuilding : UnitCommand
{
    public UnitCommandEnterBuilding(MapCell Target) : base(Target)
    {
    }

    public override bool IsDone(Unit Unit)
    {
        return Unit.IsInBuilding();
    }

    public override IEnumerator PerformAction(Unit Unit)
    {
        yield return Unit.StartCoroutine(Unit.EnterBuildingAnimation((Building)this.Target.CurrentObject));
        ((Building)this.Target.CurrentObject).Enter(Unit);
        this.Target.CurrentObject.CreatePopup(Unit.GetComponent<SpriteRenderer>().sprite, 1);
    }
}
