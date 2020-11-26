using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class UnitManager : Singleton<UnitManager>
{
    public Queue<Tuple<SkillType, ActivityState, int, CellObject>> ActionQueue;
    public List<UnitPlayer> IdleUnits;
    public Dictionary<string, SkillType> GetSkillDictionary;
    public UnityEvent onActionQueueChanged;

    public UnitManager()
    {
        this.ActionQueue = new Queue<Tuple<SkillType, ActivityState, int, CellObject>>();
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
                    ActionQueue.Dequeue();
                    onActionQueueChanged.Invoke();
                    //Debug.Log("Velkost queue: " + ActionQueue.Count);

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
        Tuple<SkillType, ActivityState, int, CellObject> newAction = null;
        
        //todo rozlisovat medzi roznymi activity states, vsetko nebude gather
        if (CellObject is ResourceSource)
        {
            newAction = new Tuple<SkillType, ActivityState, int, CellObject>(this.GetSkillDictionary[CellObject.tag],
                new ActivityStateGather(CellObject.CurrentCell), CellObject.GetInstanceID(), CellObject);
        }
        else if (CellObject is UnitAnimal)
        {
            newAction = new Tuple<SkillType, ActivityState, int, CellObject>(this.GetSkillDictionary[CellObject.tag],
                new ActivityStateHunt((UnitAnimal)CellObject), CellObject.GetInstanceID(), CellObject);
        }

        if (newAction != null)
        {
            // add new action to the queue only if there is not same one already
            bool exists = false;
            foreach (var action in ActionQueue) 
            {
                if (action.Item3 == CellObject.GetInstanceID())
                {
                    exists = true;
                    break;
                }
            }
            if (!exists)
            {
                Debug.Log("Adding to queue");
                ActionQueue.Enqueue(newAction);

                onActionQueueChanged.Invoke();

                // call the main scheduling loop 
                this.ActionSchedulingLoop();
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            throw new Exception("Unknown CellObject type!");
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

    public bool RemoveFromQueue(CellObject co)
    {
        //this.ActionQueue.
        return true;
    }

}

