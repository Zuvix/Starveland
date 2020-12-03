using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class UnitManager : Singleton<UnitManager>
{
    public List<Tuple<SkillType, ActivityState, CellObject, GameObject>> ActionQueue;
    public List<UnitPlayer> IdleUnits;
    public Dictionary<string, SkillType> GetSkillDictionary;
    public UnityEvent onActionQueueChanged;
    private readonly GameObject frame;

    public UnitManager()
    {
        this.frame = GameConfigManager.Instance.GameConfig.QueueFrame;
        this.ActionQueue = new List<Tuple<SkillType, ActivityState, CellObject, GameObject>>();
        this.IdleUnits = new List<UnitPlayer>();
        this.onActionQueueChanged = new UnityEvent();
        this.GetSkillDictionary = new Dictionary<string, SkillType> //todo add another skills
        {
            {"Forest", SkillType.Woodcutting },
            {"Animal", SkillType.Hunting },
            {"Animal_Dead", SkillType.Hunting }
        };
    }

    // main scheduling algorithm TODO
    public void ActionSchedulingLoop()
    {
        foreach (var action in ActionQueue.ToList())
        {
            if (IdleUnits.Count > 0)
            {
                //discard units that cant do the action
                List<UnitPlayer> IdleUnitsFiltered = new List<UnitPlayer>();
                foreach (UnitPlayer unit in IdleUnits)
                {
                    if (unit.Skills[action.Item1].Allowed)
                    {
                        IdleUnitsFiltered.Add(unit);
                    }
                }

                if (IdleUnitsFiltered.Count > 0)
                {
                    UnitPlayer bestUnit = IdleUnitsFiltered[0];
                    foreach (UnitPlayer unit in IdleUnitsFiltered)
                    {
                        if (unit.Skills[action.Item1].CurrentExperience > bestUnit.Skills[action.Item1].CurrentExperience)
                        {
                            bestUnit = unit;
                        }
                    }

                    IdleUnits.Remove(bestUnit);

                    bestUnit.GetComponent<UnitPlayer>().SetActivity(action.Item2.SetCommands(bestUnit, bestUnit.Skills[action.Item1]));

                    Debug.Log("Pocet idle unitov: " + IdleUnits.Count());
                    Destroy(ActionQueue.ElementAt(0).Item4.gameObject);
                    ActionQueue.RemoveAt(0);
                    onActionQueueChanged.Invoke();

                    IdleUnitsFiltered.Clear();
                }
            }
            else
            {
                break;
            }             
        }

    }

    public bool AddActionToQueue(CellObject CellObject)
    {
        if (this.ActionQueue.Count >= GameConfigManager.Instance.GameConfig.MaxQueueActions)
        {
            this.ActionSchedulingLoop();
            return false;
        }

        // add new action to the queue only if there is not same one already
        bool exists = false;
        foreach (var action in ActionQueue)
        {
            if (action.Item3.GetInstanceID() == CellObject.GetInstanceID())
            {
                exists = true;
                break;
            }
        }

        if (!exists)    
        {
            Tuple<SkillType, ActivityState, CellObject, GameObject> newAction = null;

            //todo rozlisovat medzi roznymi activity states, vsetko nebude gather
            /*if (CellObject is ResourceSource)
            {
                newAction = new Tuple<SkillType, ActivityState, CellObject, GameObject>(this.GetSkillDictionary[CellObject.tag],
                    new ActivityStateGather(CellObject.CurrentCell), CellObject, Instantiate(this.frame));
            }
            else if (CellObject is UnitAnimal)
            {
                newAction = new Tuple<SkillType, ActivityState, CellObject, GameObject>(this.GetSkillDictionary[CellObject.tag],
                    new ActivityStateHunt((UnitAnimal)CellObject), CellObject, Instantiate(this.frame));
            }*/

            ActivityState NewActivityState = CellObject.CreateActivityState();
            if (NewActivityState != null)
            {
                newAction = new Tuple<SkillType, ActivityState, CellObject, GameObject> (
                    this.GetSkillDictionary[CellObject.tag],
                    NewActivityState,
                    CellObject,
                    Instantiate(this.frame)
                );
            }

            if (newAction != null)
            {
                Debug.Log("Adding to queue");
                ActionQueue.Add(newAction);

                onActionQueueChanged.Invoke();

                // call the main scheduling loop 
                this.ActionSchedulingLoop();
                return true;             
            }
            else
            {
                throw new Exception("Unknown CellObject type tried to be added to the queue!");
            }
        }
        else
        {
            return false;
        }

    }

    public bool AddUnitToIdleList(UnitPlayer Unit)
    {
        if (!IdleUnits.Contains(Unit))
        {
            this.IdleUnits.Add(Unit);
        }
        this.ActionSchedulingLoop();
        return true;
    }

    public bool RemoveFromQueue(CellObject objectToRemove)
    {
        foreach (var action in this.ActionQueue.ToList())
        {
            if (action.Item3 == objectToRemove)
            {
                Destroy(action.Item4);
                this.ActionQueue.Remove(action);
                onActionQueueChanged.Invoke();
                break;
            }
        }
        return true;
    }

    private void Update()
    {
        foreach (var action in this.ActionQueue)
        {
            action.Item4.transform.position = action.Item3.transform.position;
        }
    }

}

