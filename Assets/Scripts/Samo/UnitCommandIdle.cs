using System;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

public class UnitCommandIdle : UnitCommand
{
    public UnitCommandIdle() : base(null)
    {
    }
    public override bool IsDone(Unit Unit, Skill Skill)
    {
        return false;
    }
    public override IEnumerator PerformAction(Unit Unit, Skill Skill)
    {
        yield return Unit.StartCoroutine(Unit.BeIdle());
    }
}
