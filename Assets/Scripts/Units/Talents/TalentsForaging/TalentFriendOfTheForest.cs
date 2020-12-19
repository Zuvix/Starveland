using UnityEngine;

public class TalentFriendOfTheForest : Talent
{
    public int SpawnSaplingChance;

    public TalentFriendOfTheForest(string Name, string Description, EffectList Effects, Sprite icon) : base(Name, Description, icon)
    {
        this.TalentType = TalentType.FriendOfTheForest;
        this.TalentEffects = Effects;
        this.SpawnSaplingChance = this.TalentEffects.Effects[0].effectValue;
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
        return new TalentFriendOfTheForest(this.Name, this.Description, this.TalentEffects, this.icon);
    }

    public override string Display()
    {
        return $"{this.Description} {this.TalentEffects.Effects[0].effectValue}%";
    }

    public override void Execute(int x, int y, Resource Resource)
    {
        if (Resource.itemInfo.ItemType == ItemType.Material && Random.Range(1, 100) <= this.SpawnSaplingChance)
        {
            CellObjectFactory.Instance.ProduceCellObject(x, y, CellObjects.Sapling);
        }
    }
}