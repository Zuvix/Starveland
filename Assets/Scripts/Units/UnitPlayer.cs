using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitPlayer : Unit
{
    [HideInInspector]
    public Dictionary<SkillType, Skill> Skills;
    [Header("Player unit specific")]
    [Min(1)]
    public int CarryingCapacity = 3;
    public Sprite defaultSprite;
    public UnityEvent onPlayerDeath = new UnityEvent();
    public UnityEvent<Sprite> onSpriteChange = new UnityEvent<Sprite>();

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
        PlayerPrefs.SetInt(InterSceneVariables.MaxPlayerCount, PlayerPrefs.GetInt(InterSceneVariables.MaxPlayerCount) + 1);
        base.Awake();
    }
    protected override void Start()
    {       
        objectName = NameGenerator.GetRandomName();
        this.SetActivity(new ActivityStateIdle());
        base.Start();
    }
    public override bool InventoryFull()
    {
        if (this.CarriedResource.IsDepleted())
        {
            return false;
        }
        else if (this.CurrentCommand is UnitCommandGather && this.CurrentCommand.Target.CurrentObject is ResourceSourceFishing)
        {
            return true;
        }
        return this.CarriedResource.Amount >= this.CarryingCapacity;
    }
    public override bool InventoryEmpty()
    {
        return this.CarriedResource.IsDepleted();
    }
    public override IEnumerator StoreResource(BuildingStorage target)
    {
        this.CurrentAction = "Dropping resources";
        yield return new WaitForSeconds(1.0f);
        if (target != null)
        {
            target.Flash();
        }
        yield return new WaitForSeconds(0.2f);
    }
    public override void DealDamageStateRoutine(Unit AttackingUnit)
    {
        if (!(this.CurrentActivity is ActivityStateUnderAttack) && !(this.CurrentActivity is ActivityStateHunt) && !DayCycleManager.Instance.TimeOut)
        {
            this.SetActivity(new ActivityStateUnderAttack(AttackingUnit, this, this.Skills[SkillType.Hunting]));
        }
    }
    public override void SpawnOnDeath(int x, int y)
    {
        MapControl.Instance.map.Grid[x][y].EraseCellObject();
        MapControl.Instance.CreateGameObject(x, y, MapControl.Instance.tombstone);
    }
    public override void ActionOnDeath()
    {
        Unit.PlayerUnitPool.Remove(this);
        UnitManager.Instance.IdleUnits.Remove(this);
        onPlayerDeath.Invoke();
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
        Sprite NewSprite = sprite == null ? this.defaultSprite : sprite;
        this.sr.sprite = NewSprite;
        onSpriteChange.Invoke(NewSprite);
    }
}