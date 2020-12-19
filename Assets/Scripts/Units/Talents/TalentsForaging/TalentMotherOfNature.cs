using System.Collections.Generic;
using UnityEngine;

public class TalentMotherOfNature : Talent
{
    public TalentMotherOfNature(string Name, string Description, Sprite icon) : base(Name, Description, icon)
    {
        this.TalentType = TalentType.MotherOfNature;
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        return true;
    }

    public override bool Remove(Unit Unit, Skill Skill)
    {
        return true;
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentMotherOfNature(this.Name, this.Description, this.icon);
    }

    public override string Display()
    {
        return this.Description;
    }

    public override void Execute(int x, int y, Resource Resource, ResourceSource Target)
    {
        if (Resource.itemInfo.ItemType == ItemType.Material)
        {
            List<MapCell> neighbouringFields = Target.CurrentCell.GetClosestNeighbours();
            neighbouringFields.AddRange(Target.CurrentCell.GetClosestDiagonalNeighbours());
            MapCell randomCell = neighbouringFields[Random.Range(0, neighbouringFields.Count)];
            while (CellObjectFactory.Instance.ProduceResourceSource(randomCell.x, randomCell.y, RSObjects.Bush_Berry_Purple) == null)
            {
                randomCell = neighbouringFields[Random.Range(0, neighbouringFields.Count)];
            }
        }
    }
}