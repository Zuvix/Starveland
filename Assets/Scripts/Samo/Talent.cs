using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Talent
{
    public string Name { get; }

    public Talent(string Name)
    {
        this.Name = Name;
    }

    public virtual void Display()
    {
        throw new NotImplementedException();
    }

}
