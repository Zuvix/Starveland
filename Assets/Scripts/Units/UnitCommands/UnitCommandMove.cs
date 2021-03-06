﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class UnitCommandMove : UnitCommand
{
    public List<MapCell> Targets { get; private set; }
    public List<MapCell> UsedTargets { get; private set; }
    public UnitCommandMove(MapCell Target, List<MapCell> Path) : base(Target)
    {
        this.Targets = Path;
        this.UsedTargets = new List<MapCell>();
    }
    public override bool IsDone(Unit Unit)
    {
        bool Result = false;
        if (this.Targets != null && this.Targets.Count == 0)
        {
            Result = true;
        }
        return Result;
    }
    public override bool CanBePerformed(Unit Unit)
    {
        bool Result = true;
        if (this.Targets == null || !this.Targets.First().CanBeEnteredByUnit())
        {
            Result = false;
        }
        return Result;
    }
    public override IEnumerator PerformAction(Unit Unit)
    {
        // TODO - Animation here
        if (this.Targets != null && this.Targets.Count > 0)
        {
            int MovementSpeedModifier = 100;

            if (Unit is UnitPlayer)
            {
                foreach (var skill in ((UnitPlayer)Unit).Skills)
                {
                    MovementSpeedModifier += skill.Value.GetMovementSpeedModifier((UnitPlayer)Unit);
                }               
            }
            yield return Unit.StartCoroutine(Unit.MoveUnitToNextPosition(this.Targets.First(), Unit.MovementSpeed * ((float)(MovementSpeedModifier)/100f)));


            // Remove the currently performed action from the command list.
            // Keep this at the bottom of the method unless you know better.
            this.UsedTargets.Add(this.Targets.First());
            this.Targets.RemoveAt(0);

            Unit.MovementConflictManager.RefreshRemainingRetryCounts();
        }
    }
    public void RestoreTargets()
    {
        this.UsedTargets.AddRange(this.Targets);
        this.Targets.Clear();
        this.Targets.AddRange(this.UsedTargets);
        this.UsedTargets.Clear();
    }
    public void DeNullifyTargets()
    {
        if(this.Targets == null)
        {
            this.Targets = new List<MapCell>();
            this.UsedTargets = new List<MapCell>();
        }
    }
}
