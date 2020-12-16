using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class BuildingCrafting : Building
{
    private static List<BuildingCrafting> CraftingBuildingPool = new List<BuildingCrafting>();
    public List<CraftingRecipe> AvailableRecipes;
    public readonly List<int> CraftingQueue = new List<int>();
    public readonly List<int> ItemQuantities = new List<int>();
    public readonly UnityEvent<int, List<Resource>> OnQueueUpdate = new UnityEvent<int, List<Resource>>();
    public readonly UnityEvent<string> OnCraftStart = new UnityEvent<string>();
    public readonly UnityEvent<float> OnCraftUpdate = new UnityEvent<float>();
    public readonly UnityEvent OnCraftEnd = new UnityEvent();
    private float CurrentProgress;
    private ProgressBar ProgressBar;

    public int CurrentRecipeIndex { get; private set; }  = -1;
    private static bool ProgressBarAllowed;
    private static bool WorkHalted = false;
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
        ProgressBar = Instantiate(PrefabPallette.Instance.CellObjectSliderPrefab, PrefabPallette.Instance.Canvas.transform).GetComponent<ProgressBar>();
        // Move Progress Bar to position of building and adjust it a little
        Vector3 ProgressBarPosition = this.gameObject.transform.position;
        ProgressBarPosition.x -= 1;
        ProgressBarPosition.y += this.gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        PositionConversion.MoveUIObjectToWorldObjectPosition(ProgressBarPosition, ProgressBar.gameObject, PrefabPallette.Instance.Canvas, PrefabPallette.Instance.Camera);
        ProgressBar.gameObject.SetActive(false);

        if (WorkHalted)
        {
            WorkHalted = false;
        }

        CraftingBuildingPool.Add(this);
    }
    void Update()
    {
        if (WorkHalted)
        {
            return;
        }

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
                (Sprite, int) PopupInfo = AvailableRecipes[CurrentRecipeIndex].ProduceOutput(this);

                ProgressBar.gameObject.SetActive(false);
                if(ProgressBarAllowed)
                {
                    this.CreatePopup(PopupInfo.Item1, PopupInfo.Item2);
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
        OnCraftStart.Invoke(AvailableRecipes[CurrentRecipeIndex].OutputName());
    }
    public void CancelQueuedRecipe(int Index)
    {
        if (ItemQuantities[Index] > 0)
        {
            for (int i = CraftingQueue.Count - 1; i >= 0; i--)
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
    public static void ToggleProgressBarVisibility(bool newValue)
    {
        ProgressBarAllowed = newValue;
        foreach (BuildingCrafting Building in CraftingBuildingPool)
        {
            if ((newValue && Building.CurrentRecipeIndex != -1) || !newValue)
            {
                Building.ProgressBar.gameObject.SetActive(newValue);
            }
        }
        /*if ((newValue && CurrentRecipeIndex != -1) || !newValue)
        {
            this.ProgressBar.gameObject.SetActive(newValue);
        }*/
    }

    public static void HaltWork()
    {
        ToggleWorkHalting(true);
    }
    public static void RestoreWork(int _)
    {
        ToggleWorkHalting(false);
    }
    private static void ToggleWorkHalting(bool value)
    {
        WorkHalted = value;
    }
}