using System;
using System.Collections;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UnitCommandDrop : UnitCommand
{
    public UnitCommandDrop(MapCell Target) : base(Target)
    {
    }

    public override bool IsDone(Unit Unit, Skill Skill)
    {
        return Unit.CarriedResource.IsDepleted();
    }

    public override IEnumerator PerformAction(Unit Unit, Skill skill)
    {
        // TODO This might be moved to Mišo's Skill classes, Miso: nevidim dovod preco by to muselo byt v skille
        Resource DroppedResource = Unit.CarriedResource.Deplete();
        // TODO Add DroppedResource to storage, i.e. global inventory

        // TODO Animation here
        //Console.WriteLine("I'm dropping wood");
        yield return Unit.StartCoroutine(Unit.StoreResource(this.Target.CurrentObject.GetComponent<BuildingStorage>()));
    }
}
