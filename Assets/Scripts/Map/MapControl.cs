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
    public GameObject carrot_field;

    public GameObject building_storage;

    public GameObject player;
    public GameObject animal;
    public GameObject animal_dead;
    public GameObject tombstone;

    private void Start() {
        //Example of the world
        StorageList = new List<MapCell>();
        map = new Map(32, 18, 10f, new Vector3(0, 0));
        GameObject testUnit1 = CreateGameObject(0, 0, player);
        GameObject testUnit2 = CreateGameObject(0, 1, player);
        GameObject testUnit3 = CreateGameObject(0, 2, player);
        CreateGameObject(5, 5, building_storage);

        List<(int, int)> ForestCoords = new List<(int, int)>(new (int, int)[]
        { 
            (9, 4), (10, 4), (11, 4), (9, 5), (15, 15), (15,14), (14,15), (11,11), (10,10), (10,11), (11,10), (12,13)
        });
        foreach ((int, int) Coord in ForestCoords)
        {
            ResourceSourceFactory.Instance.ProduceResourceSource(Coord.Item1, Coord.Item2, "Wood");
        }

        //animal test
        GameObject testAnimal1 = CreateGameObject(14, 14, animal);
        GameObject testAnimal2 = CreateGameObject(13, 13, animal);
        GameObject testAnimal3 = CreateGameObject(15, 12, animal);
        GameObject testAnimal4 = CreateGameObject(16, 14, animal);

        //Additional resource
        GameObject carrot_field_1 = ResourceSourceFactory.Instance.ProduceResourceSource(7, 5, "Carrot");



        //testUnit1.GetComponent<Unit>().SetActivity(new ActivityStateWoodcutting(map.Grid[11][4], testUnit1.GetComponent<Unit>(), testUnit1.GetComponent<Unit>().SkillWoodcutting));
        //testUnit1.GetComponent<Unit>().SetActivity(new ActivityStateIdle());
        // testUnit2.GetComponent<Unit>().SetActivity(new ActivityStateIdle());

    }

    private void Update() {
        HandleClickToModifymap();
    }

    private void HandleClickToModifymap()
    {
        if (GlobalGameState.Instance.InGameInputAllowed)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //CreateGameObject(UtilsClass.GetMouseWorldPosition(), forest);

                //Debug.Log(map.GetValue(UtilsClass.GetMouseWorldPosition()));
                //TODO asi prerobit inak, overit ci neni null a ci je kliknuty objekt gatherovatelny... potom spravit cez UI button gather
                //UnitManager.Instance.AddActionToQueue(new ActivityStateWoodcutting());
            }
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

    public GameObject CreateGameObject(int x, int y, GameObject toBeCreatedGO)
    {
        if (map.IsInBounds(x,y))
        {
            if (map.GetValue(x, y) == null)
            {
                //Debug.LogError("GameObject is going to be instantiated in MapControl");
                GameObject g = Instantiate(toBeCreatedGO);
                //Debug.LogError("GameObject instantiated in MapControl");
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
