using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UnitAnimal : Unit
{
    public int AggroRadius = 1;
    [Header("Other")]
    [Tooltip("Defines how far a unit can move from its spawn position during wandering.")]
    [Min(0)]
    public int WanderingRadius = 2;
    [Tooltip("Defines the chance of a unit moving to random position if he's wandering around and idling.")]
    [Range(0, 100)]
    public int ChanceToMoveDuringWandering = 10;
    private int spawnX;
    private int spawnY;
    public List<ResourcePack> inventory;
    [Tooltip("Defines difficulty for player to kill this Animal.")]
    public string difficulty;
    public int MaxTravelDistance = 5;

    public MapCell SpawnCell { get; private set; } = null;

    protected override void Awake()
    {
        this.Health = this.MaxHealth;
        base.Awake();
        this.IsPossibleToAddToActionQueue = true;
    }
    protected override void Start()
    {
        this.spawnX = this.CurrentCell.x;
        this.spawnY = this.CurrentCell.y;
        SetDefaultActivity();
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
        if (UnitTarget.CurrentCell.position.x > transform.position.x)
        {
            Flip("right");
        }
        else if (UnitTarget.CurrentCell.position.x < transform.position.x)
        {
            Flip("left");
        }
        yield return new WaitForSeconds(AttackTime);
        //UnitTarget.DealDamage(this.BaseDamage, this, false);
        if (UnitTarget != null)
        {
            this.Attack(this, UnitTarget);
        }
        //UnitTarget.Flash(Color.red);
        yield return new WaitForSeconds(0.2f);
    }
    public override void DealDamageStateRoutine(Unit AttackingUnit)
    {
        if (!(this.CurrentActivity is ActivityStateUnderAttack))
        {
            this.SetActivity(new ActivityStateUnderAttack(AttackingUnit, this));
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
                drops.Add(toAddResource);
            }

        }
        if (drops.Count > 0)
        {
            List<MapCell> SpawnCells = this.CurrentCell.GetRandomUnitEnterableNeighbour(drops.Count - 1);
            SpawnCells.Add(this.CurrentCell);
            int i = 0;
            foreach (MapCell SpawnCell in SpawnCells)
            {
                SpawnCell.EraseCellObject();
                CellObjectFactory.Instance.ProduceResourceSource(SpawnCell.x, SpawnCell.y, ItemManager.Instance.resourceToResourceSource[drops[i].itemInfo], new List<Resource>() { drops[i] });
                i++;
            }
            if (i < drops.Count)
            {
                Debug.LogError($"Failed to spawn {drops.Count - 1} items from {this.gameObject}");
            }
        }
    }
    public override void SetDefaultActivity()
    {
        this.SetActivity(new ActivityStateWander(this.WanderingRadius, this.CurrentCell, this.ChanceToMoveDuringWandering, this.AggroRadius));
    }
    public override void SetCurrentCell(MapCell Cell)
    {
        base.SetCurrentCell(Cell);
        if (SpawnCell == null)
        {
            SpawnCell = Cell;
        }
        if (PathFinding.Instance.BlockDistance(SpawnCell, CurrentCell) > MaxTravelDistance)
        {
            SetActivity(new ActivityStateMoveToSpawnPosition(SpawnCell));
        }
    }
}

