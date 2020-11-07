using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{
    public Queue<Tuple<string, int, int>> ActionQueue; //tag, x, y
    public List<Unit> IdleUnits;

    public UnitManager()
    {
        this.ActionQueue = new Queue<Tuple<string, int, int>>();
        this.IdleUnits = new List<Unit>();
    }

    // main scheduling algorithm TODO
    public void ActionSchedulingLoop()
    {
        if (IdleUnits.Count > 0)
        {
            foreach (Tuple<string, int, int> action in ActionQueue.ToList())
            {
                //todo discard units that cant do the action

                //woodcutting
                if (action.Item1.Equals("Forest"))
                {

                    Unit bestWoodcuttingUnit = IdleUnits[0];
                    foreach (Unit unit in IdleUnits)
                    {
                        if (unit.SkillWoodcutting.CurrentExperience > bestWoodcuttingUnit.SkillWoodcutting.CurrentExperience)
                        {
                            bestWoodcuttingUnit = unit;
                        }
                    }

                    IdleUnits.Remove(bestWoodcuttingUnit);
                    
                    bestWoodcuttingUnit.GetComponent<Unit>().SetActivity(
                            new ActivityStateGather(
                            MapControl.Instance.map.Grid[action.Item2][action.Item3], bestWoodcuttingUnit,
                            bestWoodcuttingUnit.SkillWoodcutting));

                    Debug.Log("Pocet idle unitov: " + IdleUnits.Count());
                    ActionQueue.Dequeue();
                    Debug.Log("Velkost queue: " + ActionQueue.Count);

                }

            }

        }

    }

    public bool AddActionToQueue(string tag, int x, int y)
    {
        // add new action to the queue only if there is not same one already
        Tuple<string, int, int> NewAction = new Tuple<string, int, int>(tag, x, y);
        if (!ActionQueue.Contains(NewAction)) {
            Debug.Log("Adding to queue");
            ActionQueue.Enqueue(NewAction);
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

