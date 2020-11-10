using System;
using System.Collections;
using UnityEngine;
class ActivityStateIdle : ActivityState
{
    public override void InitializeCommand(Unit Unit)
    {
        base.InitializeCommand(Unit);
        Unit.CurrentCommand = new UnitCommandIdle();
    }

    public override IEnumerator PerformAction(Unit Unit)
    {
        yield return Unit.StartCoroutine(Unit.CurrentCommand.PerformAction(Unit));

        //TODO add to idle list, nemoze to tu byt kvoli tomu ze je to corutina
        //UnitManager.Instance.AddUnitToIdleList(Unit);
    }
}
