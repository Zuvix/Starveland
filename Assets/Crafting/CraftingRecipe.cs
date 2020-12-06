using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class CraftingRecipe : ScriptableObject
{
    public List<Resource> Input;
    public bool IsUnlocked = true;
    public float CraftingDuration;

    private string Description;
    [SerializeField]
    private bool DescriptionInitialized = false;

    public abstract (Sprite, int) ProduceOutput(BuildingCrafting ProducingBuilding);
    public abstract string OutputName();
    public string OutputDescription()
    {
        Debug.LogWarning(DescriptionInitialized);
        if (!DescriptionInitialized)
        {
            Debug.LogWarning("Creating Description");
            Description = CreateOutputDescription();
            DescriptionInitialized = true;
        }

        Debug.LogWarning(Description);
        return Description;
    }
    protected abstract string CreateOutputDescription();
    public abstract Sprite OutputIcon();
}
