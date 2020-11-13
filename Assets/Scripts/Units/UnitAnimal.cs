using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UnitAnimal : Unit
{
    private readonly int WanderingRadius = 3;
    private int SpawnX;
    private int SpawnY;

    protected override void Awake()
    {
        this.MovementSpeed = 5.0f;
        this.MaxHealth = 100;
        this.Health = this.MaxHealth;
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        this.SpawnX = this.CurrentCell.x;
        this.SpawnY = this.CurrentCell.y;
        StartCoroutine("RandomBehaviour");
        objectName = "Unicorn";
    }

    protected IEnumerator RandomBehaviour()
    {
        while (true)
        {
            if (this.CurrentActivity is ActivityStateIdle)
            {

                float t = Random.Range(5, 10);
                yield return new WaitForSeconds(t);

                int rx = Random.Range(SpawnX - this.WanderingRadius, this.SpawnX + this.WanderingRadius);
                int ry = Random.Range(this.SpawnY - this.WanderingRadius, this.SpawnY + this.WanderingRadius);

                Debug.LogWarning($"{rx}, {ry}");

                SetActivity(new ActivityStateGather(MapControl.Instance.map.Grid[rx][ry]).SetCommands(this, null));

                yield return new WaitForFixedUpdate();
            }
            else
            {
                yield return new WaitForSeconds(1);
            }
        }
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

