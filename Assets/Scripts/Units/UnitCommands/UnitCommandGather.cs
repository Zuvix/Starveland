using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UnitCommandGather : UnitCommand
{
    public Skill Skill;
    private ResourceSource originalCellObject;
    public UnitCommandGather(MapCell Target, Skill Skill, ResourceSource CellObject) : base(Target)
    {
        this.Skill = Skill;
        this.originalCellObject = CellObject;
    }

    public override bool IsDone(Unit Unit)
    {
        //return Unit.CarriedResource.Amount >= Unit.CarryingCapacity;
        if (this.Target.CurrentObject != (CellObject)this.originalCellObject || Unit.InventoryFull())
        {
            return true;
        }
        else
        {
            return false;
        }
    } 

    public override IEnumerator PerformAction(Unit Unit)
    {
        // TODO Perhaps this will be in Mišo's Skill class
        // Constant 1 should be the amount of resource harvested with single hit
        //Unit.CarriedResource.AddDestructive(((ResourceSource)Target.CurrentObject).Resource.Subtract(1));


        // TODO Animation might be here

        //Console.WriteLine("I'm cutting wood {0}/{1}", Unit.CarriedResource.Amount, Skill.CarryingCapacity);
        yield return Unit.StartCoroutine(Unit.GatherResource((ResourceSource)this.Target.CurrentObject, Skill.GetGatheringSpeed((ResourceSource)this.Target.CurrentObject)));

        if (this.Target.CurrentObject != null && this.Target.CurrentObject is ResourceSource)
        {
            ResourceSource TargetResourceSource = (ResourceSource)this.Target.CurrentObject;
            if (Unit.InventoryEmpty() || TargetResourceSource.resource.itemInfo == Unit.CarriedResource.itemInfo)
            {
                TargetResourceSource.Flash();
                Resource GatheredResource;
                Skill.DoAction(Unit, TargetResourceSource, out GatheredResource);
                Unit.CreatePopup(GatheredResource.itemInfo.icon, GatheredResource.Amount);
            }
        }
    }
    public override bool CanBePerformed(Unit Unit)
    {
        bool Result = false;
        ResourceSource TargetResourceSource = (ResourceSource)this.Target.CurrentObject;
        if (TargetResourceSource == null)
        {
            Result = false;
        }
        else if (TargetResourceSource is ResourceSource)
        {
            if (TargetResourceSource.resource.IsDepleted())
            {
                Result = false;
            }
            else if (Unit.InventoryEmpty())
            {
                Result = true;
            }
            else if (TargetResourceSource.resource.itemInfo == Unit.CarriedResource.itemInfo)
            {
                Result = true;
            }
        }
        return Result;
    }
}
