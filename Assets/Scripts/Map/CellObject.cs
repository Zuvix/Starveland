using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CellObject : MonoBehaviour
{
    [SerializeField]
    float flashTime=0.2f;

    Color originalColor;
    public SpriteRenderer sr;
    protected Vector3 basicScale;
    protected Quaternion basicRotation;
    public string objectName;
    public string tip;

    public GameObject popup;

    public MapCell CurrentCell { get; private set; }

    public bool IsBlocking = false;
    public bool IsSelectable = false;

    virtual protected void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        basicScale = transform.localScale;
        basicRotation = transform.rotation;
        originalColor = sr.color;
    }
    public virtual bool EnterCell(MapCell MapCell)
    {
        return MapCell.SetCellObject(this);
    }
    public virtual bool CanEnterCell(MapCell MapCell)
    {
        return MapCell.CanBeEnteredByObject(this.IsBlocking);
    }
    public virtual void SetCurrentCell(MapCell Cell)
    {
        this.CurrentCell = Cell;
    }
    public virtual void Flip(string side)
    {
        if (side.Equals("right"))
        {
            sr.flipX = false;
        }
        if (side.Equals("left"))
        {
            sr.flipX = true;
        }
    }

    public void Flash()
    {
        sr.color = Color.black;
        Invoke("ResetColor", flashTime);
    }
    public void Flash(Color color)
    {
        sr.color = color;
        Invoke("ResetColor", flashTime);
    }
    private void ResetColor()
    {
        sr.color = originalColor;
    }
    public void CreatePopup(Sprite icon, int value)
    {
        GameObject g= Instantiate(popup,this.transform);
        g.GetComponentInChildren<ItemPopup>()?.CreatePopup(icon, value);
    }

    public void CreatePopup(Sprite icon, string text)
    {
        GameObject g = Instantiate(popup, this.transform);
        g.GetComponentInChildren<ItemPopup>()?.CreatePopup(icon, text);
    }

}
