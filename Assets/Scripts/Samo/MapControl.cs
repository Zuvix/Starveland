using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class MapControl : Singleton<MonoBehaviour> {

    public Map map;
    private float mouseMoveTimer;
    private float mouseMoveTimerMax = .01f;

    public GameObject forest;
    public GameObject player;

    private void Start() {
        //Example of the world
        map = new Map(32, 18, 10f, new Vector3(0, 0));
        GameObject test=CreateGameObject(0, 0, player);
        test.GetComponent<Unit>().StartCoroutine(test.GetComponent<Unit>().MoveUnitToNextPosition(map.cells[0][0], map.cells[5][5]));
    }

    private void Update() {
        HandleClickToModifymap();
    }

    private void HandleClickToModifymap() {
        if (Input.GetMouseButtonDown(0)) {
            CreateGameObject(UtilsClass.GetMouseWorldPosition(), forest);
        }
    }

    private void HandleMouseMove() {
        mouseMoveTimer -= Time.deltaTime;
        if (mouseMoveTimer < 0f) {
            mouseMoveTimer += mouseMoveTimerMax;
            GameObject mapValue = map.GetValue(UtilsClass.GetMouseWorldPosition());
            if(mapValue!=null)
                Debug.Log(mapValue.name);
        }
    }

    private GameObject CreateGameObject(int x, int y, GameObject toBeCreatedGO)
    {
        if (map.IsInBounds(x,y))
        {
            if (map.GetValue(x, y) == null)
            {
                GameObject g = Instantiate(toBeCreatedGO);
                map.CenterObject(x, y, g);
                map.SetValue(x, y, forest);
                return g;
            }
            else
            {
                Debug.Log("Space x: " + x + " y: " + y + " is allready occupied!");
                return null;
            }
        }
        return null;
    }

    private GameObject CreateGameObject(Vector3 worldPosition, GameObject toBeCreatedGO)
    {
        int x, y;
        map.GetXY(worldPosition, out x, out y);
        return CreateGameObject(x, y, toBeCreatedGO);
    }
}
