using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ActivityStateWander : ActivityState
{
    private UnitCommandMove MoveCommand;
    private UnitCommandIdle IdleCommand;

    public ActivityStateWander() : base()
    {
        this.IdleCommand = new UnitCommandIdle();
    }

    public override void InitializeCommand(Unit Unit)
    {
        base.InitializeCommand(Unit);
        Unit.SetCommand(this.IdleCommand);
    }

    public override IEnumerator PerformAction(Unit Unit)
    {
        return null;
    }
}

