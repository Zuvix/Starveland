using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPlayer : Unit
{
    [HideInInspector]
    public Dictionary<SkillType, Skill> Skills;
    [Header("Player unit specific")]
    [Min(1)]
    public int CarryingCapacity = 3;
    public Sprite defaultSprite;

    public override void SetActivity(ActivityState Activity)
    {
        base.SetActivity(Activity);
        if (Activity is ActivityStateIdle)
        {
            UnitManager.Instance.AddUnitToIdleList(this);
        }
        else
        {
            UnitManager.Instance.IdleUnits.Remove(this);
        }
    }
    protected override void Awake()
    {
        this.Skills = new Dictionary<SkillType, Skill> 
        {
            { SkillType.Foraging, new SkillForaging() },
            { SkillType.Hunting, new SkillHunting() },
            { SkillType.Mining, new SkillMining() } 
        };
        this.Health = this.MaxHealth;
        Unit.PlayerUnitPool.Add(this);
        PlayerPrefs.SetInt("MaxPlayers", PlayerPrefs.GetInt("MaxPlayers")+1);
        base.Awake();
    }
    protected override void Start()
    {       
        objectName = NameGenerator.GetRandomName();
        this.SetActivity(new ActivityStateIdle());
        base.Start();
    }

    /*public override bool InventoryFull()
    {
         if (this.CarriedResource.IsDepleted())
         {
             return false;
         }

         SkillType CurrentResourceSkill = Unit.ResourceType2SkillType(this.CarriedResource.itemInfo);
         return this.InventoryFull(this.Skills[CurrentResourceSkill]);
    }*/

    /*public override bool InventoryFull(Skill Skill)
    {
        return this.CarriedResource.Amount >= this.CarryingCapacity;
    }*/

    public override bool InventoryFull()
    {
        if (this.CarriedResource.IsDepleted())
        {
            return false;
        }
        return this.CarriedResource.Amount >= this.CarryingCapacity;
    }

    public override bool InventoryEmpty()
    {
        if (this.CarriedResource.IsDepleted())
        {
            return true;
        }
        return false;
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

    /* public override void DealDamage(int Amount, Unit AttackingUnit)
     {
         if (!(this.CurrentActivity is ActivityStateUnderAttack) && !(this.CurrentActivity is ActivityStateHunt))
         {
             this.SetActivity(new ActivityStateUnderAttack(AttackingUnit, this));
         }
         this.Health -= Amount;
         DisplayReceivedDamage(Amount);
         if (this.Health <= 0) //handle death
         {
             int x = this.CurrentCell.x;
             int y = this.CurrentCell.y;
             this.CurrentCell.SetCellObject(null);
             Destroy(this.gameObject);
             MapControl.Instance.CreateGameObject(x, y, MapControl.Instance.tombstone);
         }
     }*/
    public override void DealDamageStateRoutine(Unit AttackingUnit)
    {
        if (!(this.CurrentActivity is ActivityStateUnderAttack) && !(this.CurrentActivity is ActivityStateHunt) && !DayCycleManager.Instance.TimeOut)
        {
            this.SetActivity(new ActivityStateUnderAttack(AttackingUnit, this, this.Skills[SkillType.Hunting]));
        }
    }
    public override void SpawnOnDeath(int x, int y)
    {
        MapControl.Instance.CreateGameObject(x, y, MapControl.Instance.tombstone);
    }
    public override void ActionOnDeath()
    {
        Unit.PlayerUnitPool.Remove(this);
        UnitManager.Instance.IdleUnits.Remove(this);
        if (DayCycleManager.Instance.GameIsWaitingForPlayerUnits2GoEat())
        {
            DayCycleManager.Instance.IndicateEndDayRoutineEnd();
        }
        if (IsInBuilding())
        {
            this.CurrentBuilding.LeaveDead(this);
        }
        GameOver.Instance.IndicatePlayerUnitDeath();
    }

    public override void SetDefaultActivity()
    {
        SetActivity(new ActivityStateIdle());
    }

    public override void SetSprite(Sprite sprite = null)
    {
        if (sprite != null)
        {
            this.sr.sprite = sprite;
        }
        else
        {
            this.sr.sprite = this.defaultSprite;
        }
    }
}
