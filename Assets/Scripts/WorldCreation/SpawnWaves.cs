using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWaves : MonoBehaviour
{
    public int waveCOunt = 50;
    public float maxX = 500;
    public float maxY = 500;
    public GameObject wave;
    void Start()
    {
        for(int i = 0; i < waveCOunt; i++)
        {
            GameObject newWave=Instantiate(wave);
            newWave.transform.position = new Vector3(Random.Range(0, maxX), Random.Range(0, maxY), 0);
        }
    }
}
