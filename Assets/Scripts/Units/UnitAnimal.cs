using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UnitAnimal : Unit
{
    private int WanderingRadius;
    private int spawnX;
    private int spawnY;
    public List<ResourcePack> inventory;

    protected override void Awake()
    {
        this.MovementSpeed = GameConfigManager.Instance.GameConfig.MovementSpeedAnimal;
        this.MaxHealth = GameConfigManager.Instance.GameConfig.MaxHealthAnimal;
        this.Health = this.MaxHealth;
        this.BaseDamage = GameConfigManager.Instance.GameConfig.BaseDamageAnimal;
        this.WanderingRadius = GameConfigManager.Instance.GameConfig.WanderingRadius;
        base.Awake();

        this.IsPossibleToAddToActionQueue = true;
    }
    protected override void Start()
    {
        this.spawnX = this.CurrentCell.x;
        this.spawnY = this.CurrentCell.y;
        Wander();

        base.Start();
    }
    public override void RightClickAction()
    {
        AddToActionQueueSimple();
    }
    public override ActivityState CreateActivityState()
    {
        return new ActivityStateHunt(this);
    }
    public override void Flip(string side)
    {
        if (side.Equals("right"))
        {
            sr.flipX = true;
        }
        if (side.Equals("left"))
        {
            sr.flipX = false;
        }
    }

    public override IEnumerator Fight(Unit UnitTarget, float AttackTime = 1.0f)
    {
        this.CurrentAction = "In combat!";
        yield return new WaitForSeconds(AttackTime);
        UnitTarget.DealDamage(this.BaseDamage, this);
        UnitTarget.Flash(Color.red);
        yield return new WaitForSeconds(0.2f);
    }

    /*public override void DealDamage(int Amount, Unit AttackingUnit)
    {
        if (!(this.CurrentActivity is ActivityStateUnderAttack))
        {
            this.SetActivity(new ActivityStateUnderAttack(AttackingUnit, this, this.spawnX, this.spawnY, this.WanderingRadius));
        }
        this.Health -= Amount;
        DisplayReceivedDamage(Amount);
        if (this.Health <= 0) //handle death
        {
            int x = this.CurrentCell.x;
            int y = this.CurrentCell.y;
            this.CurrentCell.SetCellObject(null);
            Die();
        }
    }*/
    public override void DealDamageStateRoutine(int Amount, Unit AttackingUnit)
    {
        if (!(this.CurrentActivity is ActivityStateUnderAttack))
        {
            this.SetActivity(new ActivityStateUnderAttack(AttackingUnit, this, this.spawnX, this.spawnY, this.WanderingRadius));
        }
    }
    public override void SpawnOnDeath(int x, int y)
    {
        List<Resource> drops = new List<Resource>();
        foreach (ResourcePack rp in inventory)
        {
            Resource toAddResource = rp.UnpackPack();
            if (toAddResource != null)
            {
                drops.Add(rp.resource);
            }

        }
        CellObjectFactory.Instance.ProduceResourceSource(x, y, RSObjects.DeadAnimal, drops);
    }
    public void Wander()
    {
        this.SetActivity(new ActivityStateWander(this.WanderingRadius, this.CurrentCell));
    }
}

