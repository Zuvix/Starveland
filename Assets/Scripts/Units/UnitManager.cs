using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{
    public Queue<Tuple<SkillType, ActivityState, int>> ActionQueue;
    public List<UnitPlayer> IdleUnits;
    public Dictionary<string, SkillType> GetSkillDictionary;

    public UnitManager()
    {
        this.ActionQueue = new Queue<Tuple<SkillType, ActivityState, int>>();
        this.IdleUnits = new List<UnitPlayer>();
        this.GetSkillDictionary = new Dictionary<string, SkillType> //todo add another skills
        {
            {"Forest", SkillType.woodcutting },
            {"Animal", SkillType.hunting },
            {"Animal_Dead", SkillType.hunting }
        };
    }

    // main scheduling algorithm TODO
    public void ActionSchedulingLoop()
    {
        foreach (var action in ActionQueue.ToList())
        {
            if (IdleUnits.Count > 0)
            {
                //todo discard units that cant do the action

                UnitPlayer bestUnit = IdleUnits[0];

                foreach (UnitPlayer unit in IdleUnits)
                {
                    if (unit.Skills[action.Item1].CurrentExperience > bestUnit.Skills[action.Item1].CurrentExperience)
                    {
                        bestUnit = unit;
                    }
                }

                IdleUnits.Remove(bestUnit);

                bestUnit.GetComponent<UnitPlayer>().SetActivity(action.Item2.SetCommands(bestUnit, bestUnit.Skills[action.Item1]));
                /*bestUnit.GetComponent<Unit>().SetActivity(
                        new ActivityStateGather(
                        MapControl.Instance.map.Grid[action.CurrentCell.x][action.CurrentCell.y], bestUnit,
                        bestUnit.Skills[skillType]));*/

                Debug.Log("Pocet idle unitov: " + IdleUnits.Count());
                ActionQueue.Dequeue();
                Debug.Log("Velkost queue: " + ActionQueue.Count);
            }
            else
            {
                break;
            }             
        }

    }

    public bool AddActionToQueue(CellObject CellObject)
    {
        Tuple<SkillType, ActivityState, int> newAction = null;
        
        //todo rozlisovat medzi roznymi activity states, vsetko nebude gather
        if (CellObject is ResourceSource)
        {
            newAction = new Tuple<SkillType, ActivityState, int>(this.GetSkillDictionary[CellObject.tag],
                new ActivityStateGather(CellObject.CurrentCell), CellObject.GetInstanceID());
        }
        else if (CellObject is UnitAnimal)
        {
            newAction = new Tuple<SkillType, ActivityState, int>(this.GetSkillDictionary[CellObject.tag],
                new ActivityStateHunt((UnitAnimal)CellObject), CellObject.GetInstanceID());
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

}

