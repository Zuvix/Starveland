using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public float CurrentProgress = 0;
    public GameObject EmptyBar;
    public GameObject FullBar;
    void Start()
    {
        ChangeFullBarWidth();
    }
    void Update()
    {
      /*  if (CurrentProgress < 1)
        {
            CurrentProgress += Time.deltaTime * 0.05f;
            CurrentProgress = Mathf.Min(CurrentProgress, 1);
            ChangeFullBarWidth();
            Debug.Log("Changing width " + CurrentProgress);
        }*/
    }
    void ChangeFullBarWidth()
    {
        Vector2 FullWidth = EmptyBar.GetComponent<RectTransform>().sizeDelta;
        FullWidth.x *= CurrentProgress;
        FullBar.GetComponent<RectTransform>().sizeDelta = FullWidth;
    }
}
