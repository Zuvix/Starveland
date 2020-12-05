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
    public GameObject popupNoIcon;
    public readonly float MultiPopupDelay = 0.5f;
    public MapCell CurrentCell { get; private set; }

    public bool IsBlocking = false;
    public bool IsSelectable = false;
    public bool IsPossibleToAddToActionQueue = false;

    private SpriteRenderer SpriteRendererComponent;

    public GameObject Replacement;

    virtual protected void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        basicScale = transform.localScale;
        basicRotation = transform.rotation;
        originalColor = sr.color;
    }
    private void Start()
    {
        this.SpriteRendererComponent = this.gameObject.GetComponent<SpriteRenderer>();
    }
    public void MakeTransparent(float Value)
    {
        if (SpriteRendererComponent != null)
        {
            Color NewColour = this.SpriteRendererComponent.color;
            NewColour.a = Value;
            this.SpriteRendererComponent.color = NewColour;
        }
    }
    public void MakeOpaque()
    {
        MakeTransparent(1.0f);
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
    public virtual void RightClickAction() {}
    public void AddToActionQueueSimple()
    {
        UnitManager.Instance.AddActionToQueue(this);
    }
    public virtual ActivityState CreateActivityState()
    {
        return null;
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
    public void CreatePopup(string value)
    {
        GameObject g = Instantiate(popupNoIcon, this.transform);
        g.GetComponentInChildren<ItemPopupNoIcon>()?.CreatePopup(value);
    }
    public void CreatePopup(string value, Color color)
    {
        GameObject g = Instantiate(popupNoIcon, this.transform);
        g.GetComponentInChildren<ItemPopupNoIcon>()?.CreatePopup(value, color);
    }
    public void CreatePopup(Sprite icon, int value)
    {
        GameObject g= Instantiate(popup,this.transform);
        g.GetComponentInChildren<ItemPopup>()?.CreatePopup(icon, value);
    }
    public void CreatePopup(Sprite icon, int value, Color color)
    {
        GameObject g = Instantiate(popup, this.transform);
        g.GetComponentInChildren<ItemPopup>()?.CreatePopup(icon, value, color);
    }
    public void CreatePopup(Sprite icon, string text, Color color)
    {
        GameObject g = Instantiate(popup, this.transform);
        g.GetComponentInChildren<ItemPopup>()?.CreatePopup(icon, text, color);
    }
    public void CreatePopup(Sprite icon, string text)
    {
        GameObject g = Instantiate(popup, this.transform);
        g.GetComponentInChildren<ItemPopup>()?.CreatePopup(icon, text);
    }
    public void CreatePopups(List<(Sprite, int)> Entries)
    {
        StartCoroutine(MultiPopupCoroutine(Entries));
    }
    public void CreatePopups(List<(Sprite, string)> Entries)
    {
        StartCoroutine(MultiPopupCoroutine(Entries));
    }
    private IEnumerator MultiPopupCoroutine(List<(Sprite, int)> Entries)
    {
        for(int i = 0; i < Entries.Count; i++)
        {
            if (i > 0)
            {
                yield return new WaitForSeconds(MultiPopupDelay);
            }
            this.CreatePopup(Entries[i].Item1, Entries[i].Item2);
        }
    }
    private IEnumerator MultiPopupCoroutine(List<(Sprite, string)> Entries)
    {
        for (int i = 0; i < Entries.Count; i++)
        {
            if (i > 0)
            {
                yield return new WaitForSeconds(3f);
            }
            this.CreatePopup(Entries[i].Item1, Entries[i].Item2);
        }
    }

}
