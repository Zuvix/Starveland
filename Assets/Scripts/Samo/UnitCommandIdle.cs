using System;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

public class UnitCommandIdle : UnitCommand
{
    private bool isIdling = false;
    public UnitCommandIdle() : base(null)
    {
    }
    public override bool IsDone(Unit Unit)
    {
        //return false;
        return isIdling;
    }
    public override IEnumerator PerformAction(Unit Unit)
    {
        isIdling = true;
        yield return Unit.StartCoroutine(Unit.BeIdle());
        isIdling = false;
    }
}
