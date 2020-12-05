using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class MapControl : Singleton<MapControl> {

    public GameObject GreenBackground;

    public Map map;
    public List<MapCell> StorageList;
    public GameObject building_storage;
    public GameObject player;
    public GameObject tombstone;
    //Pre Misa na talent
    public RSObjects[] Foragables= {RSObjects.Bush_Berry_Purple};
    CellObjectFactory COF;
    private void Start() {
        COF = CellObjectFactory.Instance;
        GenerateWorld();
        //Ak chces daco navyse spawnut tak skus tu
    }
    public void GenerateWorld()
    {
        StorageList = new List<MapCell>();
        map = new Map(35, 22, 10f, new Vector3(0, 0));
        CreateGameObject(1, 1, player);
        CreateGameObject(1, 2, player);
        CreateGameObject(1, 3, player);
        CreateGameObject(4, 1, player);
        CreateGameObject(15, 12, building_storage);

        //Water 
        /*
        CreateGameObject(0, 0, COF.water[4]);
        CreateGameObject(map.GetWidth()-1, 0, COF.water[5]);
        CreateGameObject(map.GetWidth() - 1, map.GetHeight() - 1, COF.water[6]);
        CreateGameObject(0, map.GetHeight()-1, COF.water[7]);
        for (int i = 1; i < map.GetWidth()-1; i++)
        {
            CreateGameObject(i, 0, COF.water[0]);
            CreateGameObject(i, map.GetHeight()-1, COF.water[2]);
        }
        for(int i = 1; i < map.GetHeight()-1; i++)
        {
            CreateGameObject(0, i, COF.water[3]);
            CreateGameObject(map.GetWidth()-1, i, COF.water[1]);
        }
        */

        //Gravel around water
        for (int i = 0; i < map.GetWidth(); i++)
        {
            int x = Random.Range(0, 3);
            if (x <= 1)
                COF.ProduceBGlObject(i, 0, BGObjects.Gravel);
            x = Random.Range(0, 3);
            if (x <= 1)
                COF.ProduceBGlObject(i, map.GetHeight()-1, BGObjects.Gravel);
        }
        for (int i = 0; i < map.GetHeight(); i++)
        {
            int x = Random.Range(0, 3);
            if (x <= 1)
            {
                COF.ProduceBGlObject(0, i, BGObjects.Gravel);
            }
            x = Random.Range(0, 3);
            if (x <= 1)
            {
                COF.ProduceBGlObject(map.GetWidth() - 1, i, BGObjects.Gravel);
            }
        }
        //Grass
        for (int i=1; i<map.GetWidth()-1;i++)
        {
            for(int d = 1; d < map.GetHeight() - 1; d++)
            {
                int x = Random.Range(0, 4);
                if (x <= 1)
                {
                    COF.ProduceBGlObject(i, d, BGObjects.Grass);
                }
                if (x ==2)
                {
                    COF.ProduceBGlObject(i, d, BGObjects.Grass1);
                }
            }
        }
        //Trees



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
