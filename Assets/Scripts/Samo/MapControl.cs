using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class MapControl : Singleton<MapControl> {

    public Map map;
    public List<MapCell> StorageList;

    private float mouseMoveTimer;
    private float mouseMoveTimerMax = .01f;

    public GameObject forest;
    public GameObject player;
    public GameObject building_storage;

    private void Start() {
        //Example of the world
        StorageList = new List<MapCell>();
        map = new Map(33, 21, 10f, new Vector3(0, 0));
        GameObject testUnit = CreateGameObject(0, 0, player);
        CreateGameObject(5, 5, building_storage);
        GameObject testForest = CreateGameObject(9, 4, forest);

        testUnit.GetComponent<Unit>().SetActivity(new ActivityStateWoodcutting(map.Grid[9][4], testUnit.GetComponent<Unit>()));
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
                map.SetValue(x, y, g);
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
