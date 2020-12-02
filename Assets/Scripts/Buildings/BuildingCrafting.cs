using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class BuildingCrafting : Building
{
    public List<CraftingRecipe> AvailableRecipes;
    public readonly List<CraftingRecipe> CraftingQueue = new List<CraftingRecipe>();
    public readonly List<int> ItemQuantities = new List<int>();
    public readonly UnityEvent<int> OnQueueUpdate = new UnityEvent<int>();
    private float CurrentProgress;
    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < AvailableRecipes.Count; i++)
        {
            ItemQuantities.Add(0);
        }
    }
    public override void RightClickAction()
    {
        PanelControl.Instance.BuildingMenuPanel.GetComponent<BuildingSpecificPanel>().Display(this);
    }
    public void EnqueueRecipe(int Index)
    {
        this.CraftingQueue.Add(AvailableRecipes[Index]);
        this.ItemQuantities[Index]++;
        OnQueueUpdate.Invoke(Index);
    }
    public void DequeueRecipe(int Index)
    {
        if (ItemQuantities[Index] > 0)
        {
            for (int i = CraftingQueue.Count - 1; i >= 0; i++)
            {
                if (CraftingQueue[i] == AvailableRecipes[Index])
                {
                    CraftingQueue.RemoveAt(i);
                    this.ItemQuantities[Index]--;
                    break;
                }
            }
            OnQueueUpdate.Invoke(Index);
        }
    }
}