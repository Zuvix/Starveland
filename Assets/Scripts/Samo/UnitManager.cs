using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{
    public Queue<CellObject> ActionQueue;
    public List<Unit> IdleUnits;
    public Dictionary<string, SkillType> GetSkillDictionary;

    public UnitManager()
    {
        this.ActionQueue = new Queue<CellObject>();
        this.IdleUnits = new List<Unit>();
        this.GetSkillDictionary = new Dictionary<string, SkillType> //todo add another skills
        {
            {"Forest", SkillType.woodcutting }
        };
    }

    // main scheduling algorithm TODO
    public void ActionSchedulingLoop()
    {
        if (IdleUnits.Count > 0)
        {
            foreach (CellObject action in ActionQueue.ToList())
            {
                //todo discard units that cant do the action

                SkillType skillType = GetSkillDictionary[action.tag];
            
                Unit bestUnit = IdleUnits[0];
                foreach (Unit unit in IdleUnits)
                {
                    if (unit.Skills[skillType].CurrentExperience > bestUnit.Skills[skillType].CurrentExperience)
                    {
                        bestUnit = unit;
                    }
                }

                IdleUnits.Remove(bestUnit);
                    
                bestUnit.GetComponent<Unit>().SetActivity(
                        new ActivityStateGather(
                        MapControl.Instance.map.Grid[action.CurrentCell.x][action.CurrentCell.y], bestUnit,
                        bestUnit.Skills[skillType]));

                Debug.Log("Pocet idle unitov: " + IdleUnits.Count());
                ActionQueue.Dequeue();
                Debug.Log("Velkost queue: " + ActionQueue.Count);
             
            }

        }

    }

    public bool AddActionToQueue(CellObject CellObject)
    {
        // add new action to the queue only if there is not same one already
        if (!ActionQueue.Contains(CellObject)) {
            Debug.Log("Adding to queue");
            ActionQueue.Enqueue(CellObject);
        }
        // call the main scheduling loop 
        this.ActionSchedulingLoop();
        return true;
    }

    public bool AddUnitToIdleList(Unit Unit)
    {
        if (!IdleUnits.Contains(Unit))
        {
            Debug.Log("Pridavam sa do listu");
            this.IdleUnits.Add(Unit);
        }
        this.ActionSchedulingLoop();
        return true;
    }

}

