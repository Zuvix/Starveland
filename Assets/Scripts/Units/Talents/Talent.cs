using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Talent
{
    public string Name { get; }
    public Sprite icon;
    public int Effect;

    public Talent(string Name, Sprite icon)
    {
        this.Name = Name;
        this.icon = icon;
    }

    public virtual void Display()
    {
        throw new NotImplementedException();
    }

}
