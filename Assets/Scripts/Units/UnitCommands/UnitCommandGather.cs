using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UnitCommandGather : UnitCommand
{
    public Skill Skill;
    private ResourceSourceGeneric originalCellObject;
    public UnitCommandGather(MapCell Target, Skill Skill, ResourceSourceGeneric CellObject) : base(Target)
    {
        this.Skill = Skill;
        this.originalCellObject = CellObject;
    }
    public override bool IsDone(Unit Unit)
    {
        return this.Target.CurrentObject != (CellObject)this.originalCellObject || Unit.InventoryFull();
    } 
    public override IEnumerator PerformAction(Unit Unit)
    {
        yield return Unit.StartCoroutine(Unit.GatherResource((ResourceSourceGeneric)this.Target.CurrentObject, Skill.GetGatheringSpeed((ResourceSourceGeneric)this.Target.CurrentObject)));

        if (this.Target.CurrentObject != null && Target.CurrentObject is ResourceSourceGeneric)
        {
            ResourceSourceGeneric TargetResourceSource = (ResourceSourceGeneric)this.Target.CurrentObject;
            if (Unit.InventoryEmpty() || (TargetResourceSource is ResourceSource source && source.resource.itemInfo == Unit.CarriedResource.itemInfo))
            {
                TargetResourceSource.Flash();
                Skill.DoAction(Unit, TargetResourceSource, out Resource GatheredResource);
                if (GatheredResource != null)
                {
                    Unit.CreatePopup(GatheredResource.itemInfo.icon, GatheredResource.Amount);
                }
                else
                {
                    Unit.CreatePopup(PrefabPallette.Instance.VoidSprite, 0);
                }
            }
        }
    }
    public override bool CanBePerformed(Unit Unit)
    {

        ResourceSourceGeneric TargetResourceSource = (ResourceSourceGeneric)this.Target.CurrentObject;
        // The target must be Resource Source, it must not be empty.
        // Unit must have either empty inventory or the same type of resource in its inventory.
        // If the target Resource Source is something with not just one type of resource stored and unit has something in inventory,
        // this should not be performed either.
        bool Result =
                !(TargetResourceSource == null) &&
                !TargetResourceSource.IsDepleted() &&
                (
                    Unit.InventoryEmpty() ||
                    (
                        TargetResourceSource is ResourceSource source &&
                        source.resource.itemInfo == Unit.CarriedResource.itemInfo
                    )
                );
        return Result;
    }
}
