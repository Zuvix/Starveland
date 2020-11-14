using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : CellObject
{
    [HideInInspector]
    public string CurrentAction { get; set; }
    [HideInInspector]
    public float MovementSpeed { get; set; }
    [HideInInspector]
    public int Health { get; set; }
    [HideInInspector]
    public int MaxHealth { get; set; }

    public UnitCommand CurrentCommand { get; private set; }
    protected ActivityState CurrentActivity;
    public UnitMovementConflictManager MovementConflictManager;
    public Resource CarriedResource = new Resource(null,0);

    // Used for movement collisions
    private static readonly System.Random WaitTimeGenerator = new System.Random();
    private static readonly float MinWaitTime = 0.1f;
    private static readonly float MaxWaitTime = 0.3f;
    private static readonly float WaitTimeRange = MaxWaitTime - MinWaitTime;

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
        this.CurrentActivity = Activity;
        Activity.InitializeCommand(this);
    }

    public virtual bool InventoryFull()
    {
        return true;
    }
    public virtual bool InventoryFull(Skill Skill)
    {
        return true;
    }
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        this.MovementConflictManager = new UnitMovementConflictManager();
        this.SetActivity(new ActivityStateIdle());

        StartCoroutine("ControlUnit");
    }
    public virtual IEnumerator ControlUnit()
    {
        while (true)
        {
            yield return StartCoroutine(this.CurrentActivity.PerformAction(this));
            yield return new WaitForFixedUpdate();
        }
    }
    public virtual IEnumerator MoveUnitToNextPosition(MapCell TargetCell)
    {
        this.CurrentAction = "Moving";
        // TODO - Will have to be more sophisticated
        //set cell to be used by unit, free the old cell
        MapCell PreviousCell = this.CurrentCell;
        this.CurrentCell.SetCellObject(null);
        TargetCell.SetCellObject(this.gameObject);

        //calculate distance and movement vector
        float distance;
        distance = Vector3.Distance(transform.position, TargetCell.position);
        Vector3 movementVector = TargetCell.position - transform.position;
        movementVector = movementVector.normalized;

        //turn on animations
        ScalingAnim(true);
        RotationAnim(true);

        //Flip unit towards position
        if (TargetCell.position.x > transform.position.x)
        {
            Flip("right");
        }
        else if (TargetCell.position.x < transform.position.x)
        {
            Flip("left");
        }

        //initiate ingame movement process
        while (distance > 0.5f)
        {
            transform.position += movementVector * MovementSpeed * Time.deltaTime;
            yield return new WaitForFixedUpdate();
            distance = Vector3.Distance(transform.position, TargetCell.position);
        }
        //turn off animations
        ScalingAnim(false);
        RotationAnim(false);

        //center units position to cells position
        transform.position = TargetCell.position;
    }
    public IEnumerator GatherResource(ResourceSource target, float GatheringTime)
    {
        /* if (itemInHand == null)
         {*/
        this.CurrentAction = "Gathering";
        // Debug.Log("Preparing the axe");
        yield return new WaitForSeconds(GatheringTime);
        // Debug.Log("Gathering object");
        //itemInHand = target.Gather();
        if (target != null)
        {
            target.Flash();
        }

        yield return new WaitForSeconds(0.2f);
        /* }
         else
         {
             Debug.Log("my hand is full");
         }*/
    }

    public virtual IEnumerator StoreResource(BuildingStorage target)
    {
        return null;
    }

    public IEnumerator BeIdle()
    {
        this.CurrentAction = "Idling";
        /*if (itemInHand != null)
        {
            Debug.Log("Storing resource with name:" + itemInHand.name);
            Resource storedResource=itemInHand;
            itemInHand = null;
            return storedResource;
        }*/
        //Debug.Log("About to do idle fun");
        yield return new WaitForSeconds(1.0f);
        // Debug.Log("I'm idling");
        //itemInHand = target.Gather();
        yield return new WaitForSeconds(0.2f);
    }
    public IEnumerator WaitToRetryMove()
    {
        yield return new WaitForSeconds((float)(MinWaitTime + WaitTimeGenerator.NextDouble() * WaitTimeRange));
    }
    public static SkillType ResourceType2SkillType(Item itemInfo)
    {
        SkillType Result;
        switch (itemInfo.name)
        {
            case "Wood":
                Result = SkillType.woodcutting;
                break;
            case "Stone":
            case "Iron":
                Result = SkillType.mining;
                break;
            default:
                Result = SkillType.none;
                break;
        }
        return Result;
    }
}