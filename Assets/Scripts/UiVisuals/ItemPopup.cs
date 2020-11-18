using System.Collections;
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
        valueTxt.text = "+"+value;
        if (value == 1)
        {
            valueTxt.text = "+";
        }
        // Vector3 startPos = this.transform.position;

    }
    public void CreatePopup(Sprite icon)
    {
        img.sprite = icon;
        valueTxt.text = "+";
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
