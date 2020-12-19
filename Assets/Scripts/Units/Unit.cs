using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Unit : CellObject
{
    [HideInInspector]
    public string CurrentAction { get; set; }

    [SerializeField]
    float xScalingFactor = 2f;
    [SerializeField]
    float yScalingFactor = 2f;
    [SerializeField]
    float rotationFactor = 5f;
    [SerializeField]
    float scalingTime = 0.2f;
    [SerializeField]
    float rotationTime = 0.5f;
    [SerializeField]
    float rotationSpeed = 1f;
    private static readonly float roatatingIntesity = 0.02f;
    private static readonly float scalingIntesity = 0.015f;
    protected bool animateMovement = false;

    public static readonly List<UnitPlayer> PlayerUnitPool = new List<UnitPlayer>();
    public static readonly List<Unit> UnitPool = new List<Unit>();

    public UnitCommand CurrentCommand { get; private set; }

    private ActivityState RealCurrentActivity;
    public ActivityState CurrentActivity
    {
        get
        {
            return RealCurrentActivity;
        }
        private set
        {
            RealCurrentActivity = value;
            OnActivityStateChanged.Invoke(RealCurrentActivity);
        }
    }
    public UnityEvent<ActivityState> OnActivityStateChanged = new UnityEvent<ActivityState>();
    public ActivityState NextActivity { get; private set; }
    public UnitMovementConflictManager MovementConflictManager;
    [HideInInspector]
    public Resource CarriedResource = new Resource(null,0);

    // Used for movement collisions
    private static readonly System.Random WaitTimeGenerator = new System.Random();
    private static readonly float MinWaitTime = 0.1f;
    private static readonly float MaxWaitTime = 0.3f;
    private static readonly float WaitTimeRange = MaxWaitTime - MinWaitTime;

    [Header("Combat stuff")]
    public Sprite ReceiveDamageIcon;
    public float MovementSpeed = 20f;
    [HideInInspector]
    public int Health;
    public int MaxHealth = 100;
    public int BaseDamage = 10;
    public int Accuracy = 90;
    public int Dodge = 10;
    public int CritChance = 25;
    public int Defence = 0;
    public int TimesCriticalMultiplier = 2;
    public int TargetDistance2AbortAttackOn = 4;
    [HideInInspector]
    public UnityEvent<int> onDamageRecieved = new UnityEvent<int>();

    public Building CurrentBuilding { get; protected set; } = null;
    public readonly UnityEvent OnBuildingEntered = new UnityEvent();

    private static readonly string ScreenTextMiss = "Miss";
    protected override void Awake()
    {
        base.Awake();
        this.NextActivity = null;
        Unit.UnitPool.Add(this);
    }
    protected virtual void Start()
    {
        this.MovementConflictManager = new UnitMovementConflictManager();
        this.ChangeActivity();
        StartCoroutine(ControlUnit());
        StartCoroutine(ScalingAnimation());
        StartCoroutine(RotatingAnimation());
    }
    void OnMouseDown()
    {
        MouseEvents.Instance.SimulateClickOnObject(this.gameObject);
    }
    /*DO NOT CALL THIS. THIS IS SUPPOSED TO BE CALLED FROM Building::Enter(). USE THAT METHOD TO MAKE UNIT ENTER BUILDING*/
    public void EnterBuilding(Building Building)
    {
        OnBuildingEntered.Invoke();
        if(this.CurrentCell != null)
        {
            this.CurrentCell.EraseUnit();
            this.CurrentCell = null;
        }
        this.CurrentBuilding = Building;
    }
    public void LeaveBuilding(MapCell MapCell)
    {
        MapCell.SetUnit(this);
        this.CurrentBuilding = null;
    }
    public void LeaveBuildingDead()
    {
        this.CurrentBuilding = null;
    }
    public bool IsInBuilding()
    {
        return CurrentBuilding != null;
    }
    public override bool EnterCell(MapCell MapCell)
    {
        return MapCell.SetUnit(this);
    }
    public override bool CanEnterCell(MapCell MapCell)
    {
        return MapCell.CanBeEnteredByUnit();
    }
    public void SetCommand(UnitCommand Command)
    {
        this.CurrentCommand = Command;
        if (this.CurrentCommand != null)
        {
            this.MovementConflictManager.RefreshRemainingRetryCounts();
        }
    }
    public virtual void SetActivity(ActivityState Activity)
    {
        this.NextActivity = Activity;
    }
    public virtual bool InventoryFull()
    {
        return true;
    }
    public virtual bool InventoryFull(Skill Skill)
    {
        return true;
    }
    public virtual bool InventoryEmpty()
    {
        return false;
    }
    IEnumerator RotatingAnimation()
    {
        float dR = rotationSpeed * roatatingIntesity;
        float timeRotated = 0f;
        while (true)
        {
            if (animateMovement)
            {
                while (timeRotated < rotationTime)
                {
                    transform.Rotate(Vector3.forward * dR);
                    yield return new WaitForSeconds(roatatingIntesity);
                    timeRotated += roatatingIntesity;
                }
                timeRotated = -rotationTime;
                while (timeRotated < rotationTime)
                {
                    transform.Rotate(Vector3.back * dR);
                    yield return new WaitForSeconds(roatatingIntesity);
                    timeRotated += roatatingIntesity;
                }
                timeRotated = 0;
                while (timeRotated < rotationTime)
                {
                    transform.Rotate(Vector3.forward * dR);
                    yield return new WaitForSeconds(roatatingIntesity);
                    timeRotated += roatatingIntesity;
                }
            }
            timeRotated = 0;
            transform.rotation = basicRotation;
            yield return new WaitForFixedUpdate();
        }

    }
    IEnumerator ScalingAnimation()
    {
        float dX = xScalingFactor * scalingIntesity / scalingTime;
        float dY = yScalingFactor * scalingIntesity / scalingTime;
        while (true)
        {
            if (animateMovement)
            {
                while (transform.localScale.x > basicScale.x - xScalingFactor)
                {
                    transform.localScale = new Vector3(transform.localScale.x - dX, transform.localScale.y - dY);
                    yield return new WaitForSeconds(scalingIntesity);
                }
                while (transform.localScale.x < basicScale.x)
                {
                    transform.localScale = new Vector3(transform.localScale.x + dX, transform.localScale.y + dY);
                    yield return new WaitForSeconds(scalingIntesity);
                }
            }
            transform.localScale = basicScale;
            yield return new WaitForFixedUpdate();
        }
    }
    public bool ChangeActivity()
    {
        bool Result = false; 
        if (this.NextActivity != null)
        {
            if (NextActivity is ActivityStateUnderAttack && !CurrentActivity.IsInterruptibleByAttack())
            {
                return Result;
            }
            this.CurrentActivity = this.NextActivity;
            this.NextActivity = null;
            this.CurrentActivity.InitializeCommand(this);
            Result = true;
        }
        return Result;
    }
    public virtual IEnumerator ControlUnit()
    {
        while (true)
        {
            yield return StartCoroutine(this.CurrentActivity.PerformAction(this));
            yield return new WaitForFixedUpdate();
        }
    }
    public virtual IEnumerator MoveUnitToNextPosition(MapCell TargetCell, float MovementSpeed)
    {
        this.CurrentAction = "Moving";
        animateMovement = true;
        this.CurrentCell.EraseUnit();
        TargetCell.SetUnit(this);

        // Calculate distance and movement vector
        float distance;
        distance = Vector3.Distance(transform.position, TargetCell.position);
        Vector3 movementVector = TargetCell.position - transform.position;
        movementVector = movementVector.normalized;

        // Flip unit towards position
        Flip(TargetCell);

        //initiate ingame movement process
        while (distance > 0.5f)
        {
            transform.position += movementVector * MovementSpeed * Time.deltaTime;
            yield return new WaitForFixedUpdate();
            distance = Vector3.Distance(transform.position, TargetCell.position);
        }
        //turn off animations
        animateMovement = false;

        //center units position to cells position
        transform.position = TargetCell.position;
    }
    public IEnumerator GatherResource(ResourceSourceGeneric target, float GatheringTime)
    {
        this.CurrentAction = "Gathering";
        Flip(target.CurrentCell);
        yield return new WaitForSeconds(GatheringTime);
        yield return new WaitForFixedUpdate();
    }

    public virtual IEnumerator Fight(Unit UnitTarget, float AttackTime = 2f)
    {
        this.CurrentAction = "In combat!";
        Flip(UnitTarget.CurrentCell);
        yield return new WaitForSeconds(AttackTime);
        yield return new WaitForSeconds(0.2f);
    }
    protected void Flip(MapCell MapCell)
    {
        if (MapCell.position.x > transform.position.x)
        {
            Flip(FlipDirection.Right);
        }
        else if (MapCell.position.x < transform.position.x)
        {
            Flip(FlipDirection.Left);
        }
    }
    public virtual IEnumerator StoreResource(BuildingStorage target)
    {
        return null;
    }
    public IEnumerator EnterBuildingAnimation(Building target)
    {
        this.CurrentAction = "Entering Building";
        yield return new WaitForSeconds(1.0f);
        target.Flash();
        yield return new WaitForSeconds(0.2f);
    }

    public IEnumerator BeIdle()
    {
        this.CurrentAction = "Idling";
        yield return new WaitForSeconds(1.0f);
        yield return new WaitForSeconds(0.2f);
    }
    public IEnumerator WaitToRetryMove()
    {
        yield return new WaitForSeconds((float)(MinWaitTime + WaitTimeGenerator.NextDouble() * WaitTimeRange));
    }
    public void Attack(Unit Unit, Unit TargetUnit, bool pierce = false)
    {
        int chanceToHit = Unit.Accuracy - TargetUnit.Dodge;

        if (UnityEngine.Random.Range(1, 100) <= chanceToHit)
        {
            bool CriticalHit = UnityEngine.Random.Range(1, 100) <= Unit.CritChance;
            int CausedDamage = Unit.BaseDamage * (CriticalHit ? this.TimesCriticalMultiplier : 1);
            {
                TargetUnit.DealDamage(CausedDamage, Unit, CriticalHit, pierce);
            }
        }
        else
        {
            Unit.CreatePopup(ScreenTextMiss);
        }
    }
    protected void DealDamage(int Amount, Unit AttackingUnit, bool crit, bool pierce = false)
    {
        DealDamageStateRoutine(AttackingUnit);

        if (!pierce)
        {
            Amount -= this.Defence;
            Amount = Math.Max(0, Amount);
        }

        this.Health -= Amount;
        DisplayReceivedDamage(Amount, crit);
        this.onDamageRecieved.Invoke(Amount);

        if (this.Health <= 0) //handle death
        {
            Die();
        }
        else
        {
            this.Flash(Color.red);
        }
    }
    public abstract void DealDamageStateRoutine(Unit AttackingUnit);
    public void Die()
    {
        int x = -1, y = -1;
        if (!this.IsInBuilding())
        {
            x = this.CurrentCell.x;
            y = this.CurrentCell.y;
        }
        UnitManager.Instance.RemoveFromQueue(this);
        if (!this.IsInBuilding())
        {
            this.CurrentCell.EraseUnit();
        }
        Destroy(this.gameObject);

        if (!this.IsInBuilding())
        {
            SpawnOnDeath(x, y);
        }
        ActionOnDeath();
        Unit.UnitPool.Remove(this);
    }
    public void ForcedDie()
    {
        UnitManager.Instance.RemoveFromQueue(this);
        Destroy(this.gameObject);
        ActionOnDeath();
        Unit.UnitPool.Remove(this);
    }
    public virtual void SpawnOnDeath(int x, int y) { }
    public virtual void ActionOnDeath() { }
    public void DisplayReceivedDamage(int Amount, bool crit)
    {
        if (crit)
        {
            CreatePopup(ReceiveDamageIcon, -Amount, Color.red);
        }
        else
        {
            CreatePopup(ReceiveDamageIcon, -Amount);
        }
    }
    public static SkillType ResourceType2SkillType(Item itemInfo)
    {
        SkillType Result;
        switch (itemInfo.name)
        {
            case "Wood":
                Result = SkillType.Foraging;
                break;
            case "Stone":
            case "Iron":
                Result = SkillType.Mining;
                break;
            default:
                Result = SkillType.none;
                break;
        }
        return Result;
    }
    public abstract void SetDefaultActivity();
    public void ToggleVisibility(bool Visible)
    {
        Vector4 NewColor = this.gameObject.GetComponent<SpriteRenderer>().color;
        NewColor.w = Visible ? 1 : 0;
        this.gameObject.GetComponent<SpriteRenderer>().color = NewColor;
    }
    public virtual void SetSprite(Sprite sprite = null) {}
}