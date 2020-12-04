using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        if (Resource.itemInfo.type.Equals("Resource"))
        {
            List<MapCell> neighbouringFields = Target.CurrentCell.GetNeighbours();
            MapCell randomCell = neighbouringFields[UnityEngine.Random.Range(0, neighbouringFields.Count)];
            while (CellObjectFactory.Instance.ProduceResourceSource(randomCell.x, randomCell.y, RSObjects.Bush_Berry_Purple) == null)
            {
                randomCell = neighbouringFields[UnityEngine.Random.Range(0, neighbouringFields.Count)];
            }
        }
    }
}