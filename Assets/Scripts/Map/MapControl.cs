using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class MapControl : Singleton<MapControl> {

    public GameObject GreenBackground;

    public Map map;
    public List<MapCell> StorageList;

    private float mouseMoveTimer;
    private float mouseMoveTimerMax = .01f;

    public GameObject forest;
    public GameObject carrot_field;

    public GameObject Grass1;

    public GameObject building_storage;

    public GameObject player;
    public GameObject animal;
    public GameObject tombstone;

    public GameObject snake;
    public GameObject spider;
    public GameObject wildboar;
    public GameObject mouse;

    private void Start() {
        //Example of the world
        StorageList = new List<MapCell>();
        map = new Map(32, 18, 10f, new Vector3(0, 0));
        GameObject testUnit1 = CreateGameObject(0, 0, player);
        GameObject testUnit2 = CreateGameObject(0, 1, player);
        GameObject testUnit3 = CreateGameObject(0, 2, player);
        GameObject Storage = CreateGameObject(5, 5, building_storage);
        Storage.GetComponent<BuildingStorage>().Enter(testUnit1.GetComponent<UnitPlayer>());
        Storage.GetComponent<BuildingStorage>().Enter(testUnit2.GetComponent<UnitPlayer>());
        Storage.GetComponent<BuildingStorage>().Enter(testUnit3.GetComponent<UnitPlayer>());

        List<(int, int)> ForestCoords = new List<(int, int)>(new (int, int)[]
        { 
            (9, 4), (10, 4), (11, 4), (9, 5), (15, 15), (15,14), (14,15), (11,11), (10,10), (10,11), (11,10), (12,13)
        });
        foreach ((int, int) Coord in ForestCoords)
        {
            CellObjectFactory.Instance.ProduceResourceSource(Coord.Item1, Coord.Item2, RSObjects.Forest);
        }
        CellObjectFactory.Instance.ProduceResourceSource(10, 0, RSObjects.Stone);
        CellObjectFactory.Instance.ProduceResourceSource(0, 10, RSObjects.Stone);
        CellObjectFactory.Instance.ProduceResourceSource(0, 11, RSObjects.Stone);
        CellObjectFactory.Instance.ProduceResourceSource(0, 9, RSObjects.Stone);
        CellObjectFactory.Instance.ProduceResourceSource(9, 3, RSObjects.Stone);
        CellObjectFactory.Instance.ProduceResourceSource(12, 10, RSObjects.Stone);
        //animal test
        GameObject testAnimal1 = CreateGameObject(14, 14, animal);
        GameObject testAnimal2 = CreateGameObject(13, 13, animal);
        GameObject testAnimal3 = CreateGameObject(15, 12, animal);
        GameObject testAnimal4 = CreateGameObject(16, 14, animal);
        GameObject snake1 = CreateGameObject(27,1, snake);
        GameObject snake2 = CreateGameObject(27, 3, snake);
        GameObject spider1 = CreateGameObject(20, 10, spider);
        GameObject spider2 = CreateGameObject(20, 11, spider);
        GameObject boar3 = CreateGameObject(19, 11, wildboar);
        GameObject boar1 = CreateGameObject(2, 2, wildboar);
        GameObject boar2 = CreateGameObject(2, 1, wildboar);
        GameObject mouse1 = CreateGameObject(3, 2, mouse);
        GameObject mouse2 = CreateGameObject(4, 1, mouse);
        GameObject mouse3 = CreateGameObject(2, 3, mouse);
        GameObject mouse4 = CreateGameObject(5, 4, mouse);

        List<(int, int)> GrassCoords = new List<(int, int)>(new (int, int)[]
        {
            (3, 3), (3, 4), (3, 5), (3, 6), (4, 3), (4, 4), (4, 5), (4, 6), (5, 3), (5, 4), (5, 6)
        });
        foreach ((int, int) Coord in GrassCoords)
        {
            CreateGameObject(Coord.Item1, Coord.Item2, Grass1);
        }
        GameObject testAnimal5 = CreateGameObject(4, 4, animal);

        //testUnit1.GetComponent<Unit>().SetActivity(new ActivityStateWoodcutting(map.Grid[11][4], testUnit1.GetComponent<Unit>(), testUnit1.GetComponent<Unit>().SkillWoodcutting));
        //testUnit1.GetComponent<Unit>().SetActivity(new ActivityStateIdle());
        // testUnit2.GetComponent<Unit>().SetActivity(new ActivityStateIdle());
        System.Random random = new System.Random();
        List<(int, int)> BushCoords = new List<(int, int)>(new (int, int)[]
        {
            (9, 1), (9, 2), (10, 1), (10, 2), (11, 0), (11, 1), (11, 2),
        });
        foreach ((int, int) Coord in BushCoords)
        {

            CellObjectFactory.Instance.ProduceResourceSource(Coord.Item1, Coord.Item2, RSObjects.Stone);//BushTypeList[random.Next(BushTypeList.Count)]);
        }

        CellObjectFactory.Instance.ProduceCellObject(7, 7, CellObjects.Sapling);
        CellObjectFactory.Instance.ProduceCellObject(7, 8, CellObjects.Bush_Berry_Purple);
    }

    public GameObject CreateGameObject(int x, int y, GameObject toBeCreatedGO)
    {
        if (map.IsInBounds(x,y))
        {
            CellObject CellObjectComponent = toBeCreatedGO.GetComponent<CellObject>();
            if (CellObjectComponent.CanEnterCell(map.Grid[x][y]))
            {
                GameObject g = Instantiate(toBeCreatedGO);
                //Debug.LogError("GameObject instantiated in MapControl");
                map.CenterObject(x, y, g);
                map.SetValue(x, y, g);
                return g;
            }
            else
            {
                Debug.LogWarning("Space x: " + x + " y: " + y + " is already occupied!");
                return null;
            }
        }
        Debug.LogWarning($"Instantiation failed in MapControl");
        return null;
    }

    private GameObject CreateGameObject(Vector3 worldPosition, GameObject toBeCreatedGO)
    {
        map.GetXY(worldPosition, out int x, out int y);
        return CreateGameObject(x, y, toBeCreatedGO);
    }
}
