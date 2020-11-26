using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionShow : MonoBehaviour, IPointerClickHandler
{
    public Image icon;
    private CellObject target;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            StartCoroutine(this.FlashThreeTimes());
        }
    }

    public void ShowItem(CellObject co)
    {
        this.target = co;
        icon.sprite = co.sr.sprite;
        this.gameObject.SetActive(true);
    }

    private IEnumerator FlashThreeTimes()
    {
        target.Flash();
        yield return new WaitForSeconds(0.2f);
        target.Flash();
        yield return new WaitForSeconds(0.2f);
        target.Flash();
    }
}
