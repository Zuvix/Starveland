using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{
    public Queue<Tuple<SkillType, ActivityState>> ActionQueue;
    public List<Unit> IdleUnits;
    public Dictionary<string, SkillType> GetSkillDictionary;

    public UnitManager()
    {
        this.ActionQueue = new Queue<Tuple<SkillType, ActivityState>>();
        this.IdleUnits = new List<Unit>();
        this.GetSkillDictionary = new Dictionary<string, SkillType> //todo add another skills
        {
            {"Forest", SkillType.woodcutting },
            {"Animal", SkillType.hunting }
        };
    }

    // main scheduling algorithm TODO
    public void ActionSchedulingLoop()
    {
        foreach (Tuple<SkillType, ActivityState> action in ActionQueue.ToList())
        {
            if (IdleUnits.Count > 0)
            {
                //todo discard units that cant do the action

                Unit bestUnit = IdleUnits[0];
                foreach (Unit unit in IdleUnits)
                {
                    if (unit.Skills[action.Item1].CurrentExperience > bestUnit.Skills[action.Item1].CurrentExperience)
                    {
                        bestUnit = unit;
                    }
                }

                IdleUnits.Remove(bestUnit);

                bestUnit.GetComponent<Unit>().SetActivity(action.Item2.SetCommands(bestUnit, bestUnit.Skills[action.Item1]));
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
        Tuple<SkillType, ActivityState> newAction = null;

        //todo rozlisovat medzi roznymi activity states, vsetko nebude gather
        if (CellObject is ResourceSource)
        {
            newAction = new Tuple<SkillType, ActivityState>(this.GetSkillDictionary[CellObject.tag],
                new ActivityStateGather(CellObject.CurrentCell));
        }

        if (newAction != null)
        {
            // add new action to the queue only if there is not same one already
            if (!ActionQueue.Contains(newAction))
            {
                Debug.Log("Adding to queue");
                ActionQueue.Enqueue(newAction);
            }
            // call the main scheduling loop 
            this.ActionSchedulingLoop();

            return true;
        }
        else
        {
            throw new Exception("Unknown CellObject type!");
        }

    }

    public bool AddUnitToIdleList(Unit Unit)
    {
        if (!IdleUnits.Contains(Unit))
        {
            Debug.Log("Pridavam sa do idle listu");
            this.IdleUnits.Add(Unit);
        }
        this.ActionSchedulingLoop();
        return true;
    }

}

