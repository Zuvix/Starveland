using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ActionShow : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    public Image xcross;
    public GameObject frame;
    public TMP_Text coordinates;
    private CellObject target;

    private void Awake()
    {
        this.xcross.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // click on icon
        if (eventData.pointerEnter == this.icon.gameObject)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                StartCoroutine(this.FlashThreeTimes(Color.black));
            }
            /*else if (eventData.button == PointerEventData.InputButton.Right)
            {
                UnitManager.Instance.RemoveFromQueue(this.target);
            }*/
        }
        // click on red cross
        else if (eventData.pointerEnter == this.xcross.gameObject)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                UnitManager.Instance.RemoveFromQueue(this.target);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.xcross.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.xcross.gameObject.SetActive(false);
    }

    private void Update()
    {
        frame.transform.position = target.transform.position; 
        this.coordinates.text = $"{this.target.CurrentCell.x},{this.target.CurrentCell.y}";
    }

    public void ShowItem(CellObject co)
    {
        this.target = co;
        icon.sprite = co.sr.sprite;
        this.gameObject.SetActive(true);
    }

    private IEnumerator FlashThreeTimes(Color color)
    {
        target.Flash(color);
        yield return new WaitForSeconds(0.2f);
        yield return new WaitForFixedUpdate();
        target.Flash(color);
        yield return new WaitForSeconds(0.2f);
        yield return new WaitForFixedUpdate();
        target.Flash(color);
    }
}
