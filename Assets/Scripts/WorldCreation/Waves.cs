using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    public float speed;
    private void Start()
    {
        speed = speed * Random.Range(0.5f, 1.5f);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }
    private void OnBecameInvisible()
    {
        transform.position = new Vector3(498, transform.position.y, 0);
    }
}
