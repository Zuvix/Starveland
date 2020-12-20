using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPopupNoIcon : MonoBehaviour
{
    private float upSpeed = 0.02f;
    private float timeCounded = 0;
    private float timeToLive = .85f;
    private TMP_Text valueTxt;
    private void Awake()
    {
        valueTxt = GetComponentInChildren<TMP_Text>();
    }
    public void CreatePopup(int value)
    {
        valueTxt.text = (value >= 0 ? "+" : "-") + (Math.Abs(value) == 1 && value >= 0 ? "" : Math.Abs(value).ToString());
        timeToLive = .85f;
    }

    public void CreatePopup(int value, Color color)
    {
        valueTxt.color = color;
        valueTxt.text = (value >= 0 ? "+" : "-") + (Math.Abs(value) == 1 && value >= 0 ? "" : Math.Abs(value).ToString());
        timeToLive = .85f;
    }

    public void CreatePopup(string text)
    {
        valueTxt.text = text;
        timeToLive = 1.5f;
    }
    public void CreatePopup(string text, Color color)
    {
        valueTxt.color = color;
        valueTxt.text = text;
        timeToLive = 1.5f;
    }
    public void CreatePopup()
    {
        valueTxt.text = "+";
        timeToLive = .85f;
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
