using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class UnitManager : Singleton<UnitManager>
{
    public List<Tuple<SkillType, ActivityState, CellObject>> ActionQueue;
    public List<UnitPlayer> IdleUnits;
    public Dictionary<string, SkillType> GetSkillDictionary;
    public UnityEvent onActionQueueChanged;

    public UnitManager()
    {
        this.ActionQueue = new List<Tuple<SkillType, ActivityState, CellObject>>();
        this.IdleUnits = new List<UnitPlayer>();
        this.onActionQueueChanged = new UnityEvent();
        this.GetSkillDictionary = new Dictionary<string, SkillType> //todo add another skills
        {
            {"Forest", SkillType.Foraging },
            {"Animal", SkillType.Hunting },
            {"Animal_Dead", SkillType.Hunting },
            {"Stone", SkillType.Mining },
            {"Diamond", SkillType.Mining }
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
                        if (unit.Skills[action.Item1].Level > bestUnit.Skills[action.Item1].Level)
                        {
                                bestUnit = unit;
                        }
                    }

                    IdleUnits.Remove(bestUnit);

                    bestUnit.GetComponent<UnitPlayer>().SetActivity(action.Item2.SetCommands(bestUnit, bestUnit.Skills[action.Item1]));

                    Debug.Log("Pocet idle unitov: " + IdleUnits.Count());
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
            Tuple<SkillType, ActivityState, CellObject> newAction = null;

            ActivityState NewActivityState = CellObject.CreateActivityState();
            if (NewActivityState != null)
            {
                newAction = new Tuple<SkillType, ActivityState, CellObject> (
                    this.GetSkillDictionary[CellObject.tag],
                    NewActivityState,
                    CellObject
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
                this.ActionQueue.Remove(action);
                onActionQueueChanged.Invoke();
                break;
            }
        }
        return true;
    }

}

