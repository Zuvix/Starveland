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

    protected override void Awake()
    {
        this.MovementSpeed = GameConfigManager.Instance.GameConfig.MovementSpeedAnimal;
        this.MaxHealth = GameConfigManager.Instance.GameConfig.MaxHealthAnimal;
        this.Health = this.MaxHealth;
        this.BaseDamage = GameConfigManager.Instance.GameConfig.BaseDamageAnimal;
        this.WanderingRadius = GameConfigManager.Instance.GameConfig.WanderingRadius;
        base.Awake();
    }
    protected override void Start()
    {
        objectName = "Unicorn";
        this.spawnX = this.CurrentCell.x;
        this.spawnY = this.CurrentCell.y;
        this.SetActivity(new ActivityStateWander(this.WanderingRadius, this.CurrentCell));

        base.Start();
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
        UnitTarget.Flash();
        yield return new WaitForSeconds(0.2f);
    }

    public override void DealDamage(int Amount, Unit AttackingUnit)
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
            Destroy(this.gameObject);
            ResourceSourceFactory.Instance.ProduceResourceSource(x, y, "DeadAnimal");
        }
    }

    private void OnMouseOver()
    {
        if (GlobalGameState.Instance.InGameInputAllowed)
        {
            if (Input.GetMouseButtonDown(1))
            {
                UnitManager.Instance.AddActionToQueue(this);
            }
        }
    }

}

