using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TalentDualWielder : Talent
{
    public int slowedGatheringSpeed;
    public int ExtraTargets;

    public TalentDualWielder(string Name, string Description, EffectList Effects, Sprite icon) : base(Name, Description, icon)
    {
        this.TalentType = TalentType.DualWielder;
        this.TalentEffects = Effects;
        this.slowedGatheringSpeed = this.TalentEffects.Effects[0].effectValue;
        this.ExtraTargets = this.TalentEffects.Effects[1].effectValue;
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        Skill.GatheringTime *= (1f + ((float)this.slowedGatheringSpeed) / 100f);
        return true;
    }

    public override bool Remove(Unit Unit, Skill Skill)
    {
        Skill.GatheringTime /= (1f + ((float)this.slowedGatheringSpeed) / 100f);
        return true;
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentDualWielder(this.Name, this.Description, this.TalentEffects, this.icon);
    }

    public override string Display()
    {
        return $"{this.slowedGatheringSpeed}% {this.Description} +{this.ExtraTargets}";
    }

    public override void Execute(Unit Unit, ResourceSource Target, Resource Resource, Func<int, int, bool> Depleted)
    {
        if (Target != null)
        {
            int maximumTargets = this.ExtraTargets;
            List<MapCell> neighbourFields = Target.CurrentCell.GetClosestNeighbours();
            neighbourFields.AddRange(Target.CurrentCell.GetClosestDiagonalNeighbours());
            for (int i = 1; i < neighbourFields.Count; i++)
            {
                if (neighbourFields[i].GetTopSelectableObject() is ResourceSource NeighbouringResourceSource)
                {
                    if (Target.resource.itemInfo != null && NeighbouringResourceSource.resource.itemInfo != null)
                    {
                        if (NeighbouringResourceSource.resource.itemInfo.name.Equals(Target.resource.itemInfo.name) && maximumTargets-- > 0 && !Unit.InventoryFull())
                        {
                            int nx = NeighbouringResourceSource.CurrentCell.x;
                            int ny = NeighbouringResourceSource.CurrentCell.y;
                            Resource NeightbouringResource = NeighbouringResourceSource.GatherResource(1, out bool isDepletedNeighbouring);
                            NeighbouringResourceSource.Flash();
                            if (isDepletedNeighbouring && !Target.tag.Equals("Diamond"))
                            {
                                Depleted(nx, ny);
                            }
                            Resource.Amount++;
                            Unit.CarriedResource.AddDestructive(NeightbouringResource);
                        }
                    }
                }
            }
        }
    }
}