using UnityEngine;

public class TalentArcheologist : Talent
{
    public TalentArcheologist(string Name, string Description, Sprite icon) : base(Name, Description, icon)
    {
        this.TalentType = TalentType.Archeologist;
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
        return new TalentArcheologist(this.Name, this.Description, this.icon);
    }

    public override string Display()
    {
        return $"{this.Description} ";
    }

    public override void Execute(int x, int y)
    {
        CellObjectFactory.Instance.ProduceResourceSource(x, y, RSObjects.DeadAnimal);
    }
}