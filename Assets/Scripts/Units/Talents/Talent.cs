using UnityEngine;

public abstract class Talent
{
    public string Name;
    public string Description;
    public Sprite icon;
    public EffectList TalentEffects;
    public bool Ultimate;

    public Talent(string Name, string Description, Sprite icon)
    {
        this.Name = Name;
        this.Description = Description;
        this.icon = icon;
    }

    public abstract bool Apply(Unit Unit, Skill Skill);

    public abstract bool Remove(Unit Unit, Skill Skill);

    public abstract Talent CreateNewInstanceOfSelf();

    public abstract string Display();
}
