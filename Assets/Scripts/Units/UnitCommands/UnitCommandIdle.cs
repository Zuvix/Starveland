using System;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UnitCommandIdle : UnitCommand
{
    public UnitCommandIdle() : base(null)
    {
    }
    public override bool IsDone(Unit Unit)
    {
        return false;
    }
    public override IEnumerator PerformAction(Unit Unit)
    {
        yield return Unit.StartCoroutine(Unit.BeIdle());
    }
}
