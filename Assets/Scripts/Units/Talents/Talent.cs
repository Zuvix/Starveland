using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Talent
{
    public string Name;
    public string Description;
    public Sprite icon;
    public int Effect;
    public bool Ultimate;

    public Talent(string Name, string Description, Sprite icon, bool Ultimate)
    {
        this.Name = Name;
        this.Description = Description;
        this.icon = icon;
        this.Ultimate = Ultimate;
    }

    public abstract bool Apply(Unit Unit, Skill Skill);

    public abstract bool Remove(Unit Unit, Skill Skill);

    public abstract Talent CreateNewInstanceOfSelf();

    public virtual string Display()
    {
        return $"{this.Description} +{this.Effect}%"; ;
    }

}
