using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UnitCommandGather : UnitCommand
{
    public Skill Skill;
    public UnitCommandGather(MapCell Target, Skill Skill) : base(Target)
    {
        this.Skill = Skill;
    }

    public override bool IsDone(Unit Unit) 
    {
        //return Unit.CarriedResource.Amount >= Unit.CarryingCapacity;
        return Unit.InventoryFull(Skill);
    }

    public override IEnumerator PerformAction(Unit Unit)
    {
        // TODO Perhaps this will be in Mišo's Skill class
        // Constant 1 should be the amount of resource harvested with single hit
        //Unit.CarriedResource.AddDestructive(((ResourceSource)Target.CurrentObject).Resource.Subtract(1));
        

        // TODO Animation might be here

        //Console.WriteLine("I'm cutting wood {0}/{1}", Unit.CarriedResource.Amount, Skill.CarryingCapacity);
        yield return Unit.StartCoroutine(Unit.GatherResource(this.Target.GetCellObject().GetComponent<ResourceSource>(), Skill.GatheringSpeed));

        if (Target.CurrentObject != null && Target.CurrentObject is ResourceSource)
        {
            Resource GatheredResource;
            Skill.DoAction(Unit, (ResourceSource)Target.CurrentObject, out GatheredResource);
            Unit.CreatePopup(GatheredResource.itemInfo.icon, GatheredResource.Amount);
        }
    }
    public override bool CanBePerformed(Unit Unit)
    {
        bool Result = false;
        if (this.Target.CurrentObject == null)
        {
            Result = false;
        }
        else if (this.Target.CurrentObject is ResourceSource)
        {
            Result = !((ResourceSource)this.Target.CurrentObject).Resources[0].IsDepleted();
        }
        return Result;
    }
}
