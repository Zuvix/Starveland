using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;


public struct sur
{
    public int x;
    public int y;
    public sur(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
public class MapControl : Singleton<MapControl> {

    public GameObject GreenBackground;

    public Map map;
    public List<MapCell> StorageList=new List<MapCell>();
    public GameObject building_storage;
    public GameObject player;
    public GameObject tombstone;


    private void Start() {
        GenerateWorld();
        //Ak chces daco navyse spawnut tak skus tu
    }
    public void GenerateWorld()
    {
        
        map = new Map(35, 22, 10f, new Vector3(0, 0));
        Building storage=CreateGameObject(18, 8, building_storage).GetComponent<Building>();
        GetComponent<Areal>().GenerateAreal();
        GameObject unit;
        unit = CreateGameObject(18, 5, player);
        //storage.Enter(player.GetComponent<Unit>());
        unit =CreateGameObject(16, 5, player);
        //storage.Enter(player.GetComponent<Unit>());
        unit=CreateGameObject(18, 3, player);
        //storage.Enter(player.GetComponent<Unit>());
        unit=CreateGameObject(16, 1, player);
        unit = CreateGameObject(17, 3, player);
        //Create Lake
        GetComponent<HuntingField>().SpawnHuntingZone(15,8,17,12);
        //Forest
        GetComponent<Forest>().SpawnForest2(12, 18, 2, 2);
        GetComponent<Mines>().SpawnMines(15, 10, 17, 1);
    }
    public GameObject CreateGameObject(int x, int y, GameObject toBeCreatedGO)
    {
        if (map.IsInBounds(x, y))
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
  
    
   
    public void SpawnAnimalAnywhereInArea(AnimalObjects animal, int maxCount, int startx, int starty, int width, int height)
    {
        int count = 0;
        while (count <= maxCount)
        {
            int rx;
            int ry;
            rx = Random.Range(startx, width + startx);
            ry = Random.Range(starty, height+starty);
            if (!map.Grid[rx][ry].CanBeEnteredByObject(true))
            {
                MapCell cell=map.Grid[rx][ry].GetRandomUnitEnterableNeighbour();
                if (cell != null)
                {
                    CellObjectFactory.Instance.ProduceAnimal(cell.x, cell.y, animal);
                }
            }
            else
            {
                CellObjectFactory.Instance.ProduceAnimal(rx, ry, animal);
            }
            count++;
        }
    }
    public void SpawnRSAnywhereInArea(RSObjects RStoSpawn,int maxCount,int startx, int starty, int width, int height)
    {
        int count = 0;
        while (count <= maxCount)
        {
            int rx;
            int ry;
            rx = Random.Range(startx, width+startx);
            ry = Random.Range(starty, height+starty);
            if (!map.Grid[rx][ry].CanBeEnteredByObject(true))
            {
                MapCell cell = map.Grid[rx][ry].GetRandomUnitEnterableNeighbour();
                if (cell != null)
                {
                    CellObjectFactory.Instance.ProduceResourceSource(cell.x, cell.y, RStoSpawn);
                }
            }
            count++;
        }
    }
    
}
