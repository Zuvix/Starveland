using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPlayer : Unit
{
    public Dictionary<SkillType, Skill> Skills = new Dictionary<SkillType, Skill> {
        { SkillType.woodcutting, new SkillWoodcutting() } };

    public override void SetActivity(ActivityState Activity)
    {
        base.SetActivity(Activity);
        if (Activity is ActivityStateIdle)
        {
            UnitManager.Instance.AddUnitToIdleList(this);
        }
    }
    protected override void Awake()
    {
        this.MovementSpeed = 20.0f;
        this.MaxHealth = 100;
        this.Health = this.MaxHealth;
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        objectName = NameGenerator.GetRandomName();
    }

    public override bool InventoryFull()
    {
         if (this.CarriedResource.IsDepleted())
         {
             return false;
         }

         SkillType CurrentResourceSkill = Unit.ResourceType2SkillType(this.CarriedResource.itemInfo);
         return this.InventoryFull(this.Skills[CurrentResourceSkill]);
    }

    public override bool InventoryFull(Skill Skill)
    {
        return this.CarriedResource.Amount >= Skill.CarryingCapacity;
    }

    public override IEnumerator StoreResource(BuildingStorage target)
    {
        /*if (itemInHand != null)
    {
    Debug.Log("Storing resource with name:" + itemInHand.name);
    Resource storedResource=itemInHand;
    itemInHand = null;
    return storedResource;
    }*/
        this.CurrentAction = "Dropping resources";
        //Debug.Log("About to drop");
        yield return new WaitForSeconds(1.0f);
        //Debug.Log("Dropping resources");
        //itemInHand = target.Gather();
        target.Flash();
        yield return new WaitForSeconds(0.2f);
    }


}
