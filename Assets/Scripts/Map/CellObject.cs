using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CellObject : MonoBehaviour
{
    [SerializeField]
    float xScalingFactor = 2f;
    [SerializeField]
    float yScalingFactor = 2f;
    [SerializeField]
    float rotationFactor = 5f;
    [SerializeField]
    float scalingTime = 0.2f;
    [SerializeField]
    float rotationTime = 0.5f;
    [SerializeField]
    float rotationSpeed=1f;
    [SerializeField]
    float flashTime=0.2f;

    Color originalColor;
    public SpriteRenderer sr;
    Vector3 basicScale;
    Quaternion basicRotation;
    public string objectName;
    public string tip;


    public MapCell CurrentCell { get; private set; }
    virtual protected void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        basicScale = transform.localScale;
        basicRotation = transform.rotation;
        originalColor = sr.color;
    }

    virtual protected void Start()
    {

    }
    // Update is called once per frame
    virtual protected void Update()
    {
        
    }
    public virtual void SetCurrentCell(MapCell Cell)
    {
        this.CurrentCell = Cell;
    }
    public void ScalingAnim(bool toStart)
    {
        if (toStart)
        {
            transform.localScale = basicScale;
            StopCoroutine("ScalingAnimation");
            StartCoroutine("ScalingAnimation");
        }
        else
        {
            StopCoroutine("ScalingAnimation");
            transform.localScale = basicScale;
        }
    }
    public void RotationAnim(bool toStart)
    {
        if (toStart)
        {
            transform.rotation = basicRotation;
            StopCoroutine("RotatingAnimation");
            StartCoroutine("RotatingAnimation");
        }
        else
        {
            StopCoroutine("RotatingAnimation");
            transform.rotation = basicRotation;
        }
    }
    public virtual void Flip(string side)
    {
        if (side.Equals("right"))
        {
            sr.flipX = false;
        }
        if(side.Equals("left"))
        {
            sr.flipX = true;
        }
    }
    IEnumerator RotatingAnimation()
    {
        float roatatingIntesity = 0.015f;
        float dR = rotationSpeed * roatatingIntesity;
        float timeRotated = 0f;
        while (true)
        {
            while (timeRotated<rotationTime)
            {
                transform.Rotate(Vector3.forward * dR);
                yield return new WaitForSeconds(roatatingIntesity);
                timeRotated += roatatingIntesity;
            }
            timeRotated = -rotationTime;
            while (timeRotated < rotationTime)
            {
                transform.Rotate(Vector3.back * dR);
                yield return new WaitForSeconds(roatatingIntesity);
                timeRotated += roatatingIntesity;
            }
            timeRotated = -rotationTime;
            yield return new WaitForFixedUpdate();
        }

    }
    IEnumerator ScalingAnimation()
    {
        float scalingIntesity = 0.015f;
        float dX = xScalingFactor * scalingIntesity / scalingTime;
        float dY = yScalingFactor * scalingIntesity / scalingTime;
        while (true)
        {
            while (transform.localScale.x > basicScale.x - xScalingFactor)
            {
                transform.localScale = new Vector3(transform.localScale.x - dX, transform.localScale.y - dY);
                yield return new WaitForSeconds(scalingIntesity);
            }
            while (transform.localScale.x < basicScale.x)
            {
                transform.localScale = new Vector3(transform.localScale.x + dX, transform.localScale.y + dY);
                yield return new WaitForSeconds(scalingIntesity);
            }
            transform.localScale = basicScale;
            yield return new WaitForFixedUpdate();
        }
    }

    public void Flash()
    {
        sr.color = Color.black;
        Invoke("ResetColor", flashTime);
    }
    private void ResetColor()
    {
        sr.color = originalColor;
    }

}
