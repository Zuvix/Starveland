﻿using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UnitCommandGather : UnitCommand
{
    public Skill Skill;
    public UnitCommandGather(MapCell Target, Skill Skill) : base(Target)
    {
        this.Skill = Skill;
    }

    public override bool IsDone(Unit Unit) 
    {
        //return Unit.CarriedResource.Amount >= Unit.CarryingCapacity;
        return Unit.CarriedResource.Amount >= Skill.CarryingCapacity;
    }

    public override IEnumerator PerformAction(Unit Unit)
    {
        // TODO Perhaps this will be in Mišo's Skill class
        // Constant 1 should be the amount of resource harvested with single hit
        //Unit.CarriedResource.AddDestructive(((ResourceSource)Target.CurrentObject).Resource.Subtract(1));
        

        // TODO Animation might be here
        //Console.WriteLine("I'm cutting wood {0}/{1}", Unit.CarriedResource.Amount, Skill.CarryingCapacity);
        yield return Unit.StartCoroutine(Unit.GatherResource(this.Target.GetCellObject().GetComponent<ResourceSource>(), Skill.gatheringSpeed));

        Skill.DoAction(Unit, (ResourceSource)Target.CurrentObject);
    }
}
