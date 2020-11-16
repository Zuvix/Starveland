using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UnitMovementConflictManager
{
    private int RemainingMovementTries;
    private int RemainingNewPathFindTries;

    private static readonly int MovementTriesMaxCap = 3;
    private static readonly int MovementTriesMinCap = 2;
    private static readonly int NewPathFindTriesMaxCap = 4;
    private static readonly int NewPathFindTriesMinCap = 2;

    private static readonly System.Random RandomNumberGenerator = new System.Random();

    public UnitMovementConflictManager()
    {
        this.RefreshRemainingRetryCounts();
    }
    public IEnumerator UnableToMoveRoutine(Unit Unit, ActivityState NewActivity = null)
    {
        if (this.RemainingMovementTries <= 0)
        {
            if (this.RemainingNewPathFindTries <= 0)
            {
                Unit.SetActivity(NewActivity ?? new ActivityStateIdle());
            }
            else
            {
                this.RemainingNewPathFindTries--;
                ((UnitCommandMove)Unit.CurrentCommand).Targets.Clear();
                ((UnitCommandMove)Unit.CurrentCommand).Targets.AddRange(PathFinding.Instance.FindPath(Unit.CurrentCell, Unit.CurrentCommand.Target, PathFinding.EXCLUDE_LAST));
            }
        }
        else
        {
            this.RemainingMovementTries--;
            yield return Unit.StartCoroutine(Unit.WaitToRetryMove());
        }
    }
    public void RefreshRemainingRetryCounts()
    {
        this.RefreshRemainingMovementRetryCounts();
        this.RemainingNewPathFindTries = RandomNumberGenerator.Next(NewPathFindTriesMinCap, NewPathFindTriesMaxCap + 1);
    }
    private void RefreshRemainingMovementRetryCounts()
    {
        this.RemainingMovementTries = RandomNumberGenerator.Next(MovementTriesMinCap, MovementTriesMaxCap + 1);
    }
}