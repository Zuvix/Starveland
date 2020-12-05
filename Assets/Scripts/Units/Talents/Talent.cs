using System;
using UnityEngine;

public abstract class Talent
{
    public string Name;
    public string Description;
    public Sprite icon;
    public EffectList TalentEffects;
    public TalentType TalentType;

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

    public virtual void Execute() { }
    public virtual void Execute(int x, int y) { }
    public virtual void Execute(int x, int y, Resource Resource) { }
    public virtual void Execute(int x, int y, Resource Resource, ResourceSource Target) { }
    public virtual float Execute(float value) { return value;  }
    public virtual int Execute(int value) { return value;  }
    public virtual void Execute(Unit Unit, ResourceSource Target, Resource Resource, Func<int, int, bool> Depleted) { }
    public virtual float Execute(ResourceSource resourceSource, float value) { return value; }
    public virtual Resource Execute(Resource Resource) { return Resource; }
    public virtual int Execute(Item item) { return 0; }
    public virtual Resource Execute(ResourceSource Target, out bool isDepleted) 
    {
        isDepleted = false;
        return null; 
    }
    public virtual void Execute(Unit Unit, Unit Target) { }
}

public enum TalentType
{
    Carnivore,
    Archeologist,
    MiningBerserk,
    HeavyLifter,
    DualWielder,
    Lumberjack,
    Gatherer,
    ForestFruits,
    FriendOfTheForest,
    CriticalHarvest,
    MotherOfNature,
    ThroatSeeker,
    LethalBlow,
    WindDancer,
    DeadEye,
    DivineBlessing,
    UnwaveringStance,
    Opportunist
}