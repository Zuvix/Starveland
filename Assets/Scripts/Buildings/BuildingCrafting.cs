using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class BuildingCrafting : Building
{
    public List<CraftingRecipe> AvailableRecipes;
    public readonly List<int> CraftingQueue = new List<int>();
    public readonly List<int> ItemQuantities = new List<int>();
    public readonly UnityEvent<int, List<Resource>> OnQueueUpdate = new UnityEvent<int, List<Resource>>();
    public readonly UnityEvent<string> OnCraftStart = new UnityEvent<string>();
    public readonly UnityEvent<float> OnCraftUpdate = new UnityEvent<float>();
    public readonly UnityEvent OnCraftEnd = new UnityEvent();
    private float CurrentProgress;
    private ProgressBar ProgressBar;
    private int CurrentRecipeIndex = -1;
    private static bool ProgressBarAllowed;
    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < AvailableRecipes.Count; i++)
        {
            ItemQuantities.Add(0);
        }
    }
    void Start()
    {
        ProgressBar = Instantiate(PrefabPallette.Instance.CellObjectSliderPrefab).GetComponent<ProgressBar>();
        ProgressBar.gameObject.transform.SetParent(PrefabPallette.Instance.Canvas.transform);
        // Move Progress Bar to position of building and adjust it a little
        Vector3 ProgressBarPosition = this.gameObject.transform.position;
        ProgressBarPosition.x -= 1;
        ProgressBarPosition.y += this.gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        PositionConversion.MoveUIObjectToWorldObjectPosition(ProgressBarPosition, ProgressBar.gameObject, PrefabPallette.Instance.Canvas, PrefabPallette.Instance.Camera);
        ProgressBar.gameObject.SetActive(false);
    }
    void Update()
    {
        if (CurrentRecipeIndex != -1)
        {
            CurrentProgress += Time.deltaTime / AvailableRecipes[CurrentRecipeIndex].CraftingDuration;
            if (ProgressBarAllowed)
            {
                ProgressBar.CurrentProgress = CurrentProgress;
            }
            ProgressBar.gameObject.SetActive(ProgressBarAllowed);
            OnCraftUpdate.Invoke(CurrentProgress);
            if (CurrentProgress >= 1.0f)
            {
                GlobalInventory.Instance.AddItem(AvailableRecipes[CurrentRecipeIndex].Output.Duplicate());
                
                ProgressBar.gameObject.SetActive(false);
                if(ProgressBarAllowed)
                {
                    this.CreatePopup(AvailableRecipes[CurrentRecipeIndex].Output.itemInfo.icon, AvailableRecipes[CurrentRecipeIndex].Output.Amount);
                }
                CurrentRecipeIndex = -1;
                OnCraftEnd.Invoke();
            }
        }
        else if (CraftingQueue.Count > 0)
        {
            
            CurrentProgress = 0f;
            if (ProgressBarAllowed)
            {
                ProgressBar.gameObject.SetActive(true);
                ProgressBar.CurrentProgress = CurrentProgress;
            }
            DequeueRecipe();
        }
    }
    public override void RightClickAction()
    {
        PanelControl.Instance.BuildingMenuPanel.GetComponent<BuildingSpecificPanel>().Display(this);
        this.ProgressBar.gameObject.SetActive(false);
    }
    public void EnqueueRecipe(int Index)
    {
        if (GlobalInventory.Instance.AttemptRemoveItems(AvailableRecipes[Index].Input))
        {
            this.CraftingQueue.Add(Index);
            this.ItemQuantities[Index]++;
            OnQueueUpdate.Invoke(Index, AvailableRecipes[Index].Input);

            this.CreatePopups(
                AvailableRecipes[Index].Input.Select(res => (res.itemInfo.icon, -res.Amount)).ToList()
            );
        }
    }
    private void DequeueRecipe()
    {
        CurrentRecipeIndex = CraftingQueue[0];
        CraftingQueue.RemoveAt(0);
        ItemQuantities[CurrentRecipeIndex]--;
        OnQueueUpdate.Invoke(CurrentRecipeIndex, AvailableRecipes[CurrentRecipeIndex].Input);
        OnCraftStart.Invoke(AvailableRecipes[CurrentRecipeIndex].Output.itemInfo.name);
    }
    public void CancelQueuedRecipe(int Index)
    {
        if (ItemQuantities[Index] > 0)
        {
            for (int i = CraftingQueue.Count - 1; i >= 0; i++)
            {
                if (CraftingQueue[i] == Index)
                {
                    CraftingQueue.RemoveAt(i);
                    this.ItemQuantities[Index]--;
                    GlobalInventory.Instance.AddItems(AvailableRecipes[Index].Input.Select(x => x.Duplicate()).ToList());

                    OnQueueUpdate.Invoke(Index, AvailableRecipes[Index].Input);
                    break;
                }
            }
        }
    }
    public void ToggleProgressBarVisibility(bool newValue)
    {
        ProgressBarAllowed = newValue;
        if ((newValue && CurrentRecipeIndex != -1) || !newValue)
        {
            this.ProgressBar.gameObject.SetActive(newValue);
        }
    }
}