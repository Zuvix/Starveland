using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPopup : MonoBehaviour
{
    private float upSpeed = 0.02f;
    private float timeCounded = 0;
    private float timeToLive = .85f;
    private Image img;
    private TMP_Text valueTxt;
    private void Awake()
    {
        img = GetComponentInChildren<Image>();
        valueTxt = GetComponentInChildren<TMP_Text>();
    }
    public void CreatePopup(Sprite icon, int value)
    {
        img.sprite = icon;
        /*if (value == 0)
        {
            valueTxt.text = (Math.Abs(value) == 1 && value >= 0 ? "" : Math.Abs(value).ToString());
        }
        else if (value < 0)
        {
            valueTxt.text = "-" + (Math.Abs(value) == 1 && value >= 0 ? "" : Math.Abs(value).ToString());
        }
        else if (value > 0)
        {
            valueTxt.text = "+" + (Math.Abs(value) == 1 && value >= 0 ? "" : Math.Abs(value).ToString());
        }
        //valueTxt.text = (value >= 0 ? "+" : "-") + (Math.Abs(value) == 1 && value >= 0 ? "" : Math.Abs(value).ToString());
        timeToLive = .85f;*/


        if (value != 0)
        {
            valueTxt.text =
                (value > 0 ? "+" : "-")
                +
                (Math.Abs(value) == 1 && value > 0 ? "" : Math.Abs(value).ToString());
            img.sprite = icon;
            Vector4 NewColor = img.color;
            NewColor.w = 1;
            img.color = NewColor;
        }
        else
        {
            valueTxt.text = "Nothing";
            valueTxt.GetComponent<RectTransform>().position = img.GetComponent<RectTransform>().position;
            Vector4 NewColor = img.color;
            NewColor.w = 0;
            img.color = NewColor;
        }
        timeToLive = .85f;
    }

    public void CreatePopup(Sprite icon, int value, Color color)
    {
        valueTxt.color = color;
        CreatePopup(icon, value);
    }

    public void CreatePopup(Sprite icon, string text)
    {
        img.sprite = icon;
        valueTxt.text = text;
        timeToLive = 3.0f;
    }
    public void CreatePopup(Sprite icon, string text, Color color)
    {
        valueTxt.color = color;
        CreatePopup(icon, text);
    }
    public void CreatePopup(Sprite icon)
    {
        img.sprite = icon;
        valueTxt.text = "+";
        timeToLive = .85f;
        // Vector3 startPos = this.transform.position;

    }
    private void Update()
    {
        if(timeCounded < timeToLive)
        {
            transform.position += new Vector3(0, upSpeed, 0);
            timeCounded += Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
