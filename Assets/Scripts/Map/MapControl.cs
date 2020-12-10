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
    //Pre Misa na talent
    public RSObjects[] Foragables = { RSObjects.Bush_Berry_Purple };
    CellObjectFactory COF;
    public int treeMaxCount = 75;
    int treeCount = 0;
    public int hardLogMaxCount = 5;
    public int mushroomMaxCount = 15;
    public int maxBerryCount = 8;
    public int toxicMaxFungiCount = 12;
    public int rockMaxCount = 40;
    public int ironMaxCount = 12;
    public int coalMaxCount = 8;



    [Header("Animals")]
    public int mouseMaxCount= 6;
    public int boarMaxCount = 4;
    public int snakeMaxCount = 3;
    public int spiderMaxCount = 5;

    private void Start() {
        COF = CellObjectFactory.Instance;
        GenerateWorld();
        //Ak chces daco navyse spawnut tak skus tu
    }
    public void GenerateWorld()
    {
        
        map = new Map(35, 22, 10f, new Vector3(0, 0));
        Building storage=CreateGameObject(16, 12, building_storage).GetComponent<Building>();
        GameObject unit=CreateGameObject(16, 13, player);
        //storage.Enter(player.GetComponent<Unit>());
        unit=CreateGameObject(16, 11, player);
        //storage.Enter(player.GetComponent<Unit>());
        unit=CreateGameObject(15, 12, player);
        //storage.Enter(player.GetComponent<Unit>());
        unit=CreateGameObject(17, 12, player);
        //storage.Enter(player.GetComponent<Unit>());


        //Gravel around water
        SpawnGravelAroundMap();
        //Create Lake
        CreateLake(map.GetWidth() - 9, map.GetHeight() - 7,3,3);
        //Forest
        SpawnForest2(12, 18, 2, 2);

        //SpawnLeftovers
        //SpawnLeftoverStuff();
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
    public void SpawnForest2(int forestxSize, int forestySize, int startx, int starty)
    {
        if (startx <0 && starty <0) return;
        if (forestxSize < 0 && forestySize < 0) return;
        if (startx + forestxSize > map.GetWidth()-1) return;
        if (starty + forestySize > map.GetHeight()-1) return;
        List<sur> usedSpots = new List<sur>();
        
        //Spawn trees
        SpawnTrees(startx, starty, forestxSize, forestySize, out usedSpots);
        //Normal mushrooms
        SpawnMushrooms(RSObjects.Mushroom, mushroomMaxCount, usedSpots);
        //Toxic mushrooms
        SpawnMushrooms(RSObjects.ToxicMushroom, toxicMaxFungiCount, usedSpots);
        //Animals
        SpawnAnimalAnywhereInArea(AnimalObjects.Mouse, mouseMaxCount, startx, starty, forestxSize, forestySize);
        //Spawn the grass
        SpawnGrassInArea(startx, starty, forestxSize, forestySize);

    }
    public int GetRandomIndex(int max)
    {
        int randomIndex = Random.Range(0, max);
        return randomIndex;
    }
    private void SpawnLeftoverStuff()
    {
        //Spawn trees
        while (treeCount <= treeMaxCount)
        {
            int rx;
            int ry;
            rx = Random.Range(1, map.GetWidth() - 2);
            ry = Random.Range(1, map.GetHeight() - 2);
            if (map.Grid[rx][ry].CanBeEnteredByObject(true))
            {
                treeCount++;
                COF.ProduceResourceSource(rx, ry, RSObjects.Forest);
            }
        }
    }
    private void SpawnWaterAroundIsland()
    {  
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
    }
    private void CreateLake(int startx, int starty, int lakexSize, int lakeySize)
    {
        int x = startx;
        int y = starty;
        if (x != -1 && y != -1)
        {
            CreateGameObject(x, y, COF.water[4]);
            CreateGameObject(x + lakexSize, y, COF.water[5]);
            CreateGameObject(x + lakexSize, y + lakeySize, COF.water[6]);
            CreateGameObject(x, y + lakeySize, COF.water[7]);
            for (int i = x + 1; i < x + lakexSize; i++)
            {
                CreateGameObject(i, y, COF.water[0]);
                CreateGameObject(i, y + lakeySize, COF.water[2]);
            }
            for (int i = y + 1; i < y + lakeySize; i++)
            {
                CreateGameObject(x, i, COF.water[3]);
                CreateGameObject(x + lakexSize, i, COF.water[1]);
            }
            for (int i = x + 1; i < x + lakexSize; i++)
            {
                for (int d = y + 1; d < y + lakeySize; d++)
                {

                    CreateGameObject(i, d, COF.water[8]);

                    /*int r = Random.Range(0, 3);
                    if (r >= 0)
                    {
                        COF.ProduceBGlObject(i, d, BGObjects.lekno);
                    }*/

                }
            }
        }
    }
    private void SpawnTrees(int startx, int starty, int forestxSize, int forestySize, out List<sur> usedSpots)
    {
        usedSpots = new List<sur>();
        int maxFields = forestxSize * forestySize;
        List<char> spots = new List<char>();
        for (int i = 0; i < Mathf.FloorToInt(treeMaxCount * 0.8f); i++)
        {
            spots.Add('f');
            treeCount++;
        }
        for (int i = 0; i < hardLogMaxCount; i++)
        {
            spots.Add('h');
        }
        for (int i = treeCount + hardLogMaxCount; i < maxFields; i++)
        {
            spots.Add('x');
        }
        for (int i = startx; i < forestxSize + startx; i++)
        {
            for (int d = starty; d < forestySize + starty; d++)
            {
                int randomIndex = Random.Range(0, spots.Count);
                if (spots[randomIndex] == 'f' || spots[randomIndex] == 'h')
                {
                    if (!MapControl.Instance.map.Grid[i - 1][d].CanBeEnteredByUnit() || !MapControl.Instance.map.Grid[i][d - 1].CanBeEnteredByUnit() || !MapControl.Instance.map.Grid[i - 1][d - 1].CanBeEnteredByUnit() || !MapControl.Instance.map.Grid[i + 1][d - 1].CanBeEnteredByUnit())
                    {
                        randomIndex = GetRandomIndex(spots.Count);
                        if (spots[randomIndex] == 'f')
                        {
                            COF.ProduceResourceSource(i, d, RSObjects.Forest);
                            usedSpots.Add(new sur(i, d));
                        }

                        if (spots[randomIndex] == 'h')
                        {
                            COF.ProduceResourceSource(i, d, RSObjects.HardLog);
                            usedSpots.Add(new sur(i, d));
                        }

                    }
                    else
                    {
                        if (spots[randomIndex] == 'f')
                        {
                            COF.ProduceResourceSource(i, d, RSObjects.Forest);
                            usedSpots.Add(new sur(i, d));
                        }

                        if (spots[randomIndex] == 'h')
                        {
                            COF.ProduceResourceSource(i, d, RSObjects.HardLog);
                            usedSpots.Add(new sur(i, d));
                        }
                    }
                }
                else
                {
                    if (MapControl.Instance.map.Grid[i - 1][d].CanBeEnteredByUnit() && MapControl.Instance.map.Grid[i][d - 1].CanBeEnteredByUnit() && MapControl.Instance.map.Grid[i - 1][d - 1].CanBeEnteredByUnit() && MapControl.Instance.map.Grid[i + 1][d - 1].CanBeEnteredByUnit())
                    {
                        randomIndex = GetRandomIndex(spots.Count);
                        if (spots[randomIndex] == 'f')
                        {
                            COF.ProduceResourceSource(i, d, RSObjects.Forest);
                            usedSpots.Add(new sur(i, d));
                        }
                        if (spots[randomIndex] == 'h')
                        {
                            COF.ProduceResourceSource(i, d, RSObjects.HardLog);
                            usedSpots.Add(new sur(i, d));
                        }
                    }
                }
                spots.RemoveAt(randomIndex);

            }
        }
    }
    public void SpawnAnimalAnywhereInArea(AnimalObjects animal, int maxCount, int startx, int starty, int width, int height)
    {
        int count = 0;
        while (count <= maxCount)
        {
            int rx;
            int ry;
            rx = Random.Range(startx,width);
            ry = Random.Range(starty, height);
            if (map.Grid[rx][ry].CanBeEnteredByObject(false))
            {
                MapCell cell=map.Grid[rx][ry].GetRandomUnitEnterableNeighbour();
                if (cell != null)
                {
                    CellObjectFactory.Instance.ProduceAnimal(cell.x, cell.y, animal);
                }
            }
            count++;
        }
    }
    public void SpawnMushrooms(RSObjects mushroomObject, int maxCount, List<sur> usedSpots)
    {
        int mushCount = 0;
        while (mushCount < maxCount)
        {
            int randIndex = Random.Range(0, usedSpots.Count);
            sur treeSpot = usedSpots[randIndex];
            int randomDir = Random.Range(0, 4);
            int x = usedSpots[randIndex].x;
            int y = usedSpots[randIndex].y;
            if (randomDir == 0)
            {
                x += -1;
                y += 0;
            }
            if (randomDir == 1)
            {
                x += +1;
                y += 0;
            }
            if (randomDir == 2)
            {
                x += 0;
                y += 1;
            }
            if (randomDir == 3)
            {
                x += 0;
                y += -1;
            }
            if (MapControl.Instance.map.Grid[x][y].CanBeEnteredByUnit() && MapControl.Instance.map.Grid[x][y].CanBeEnteredByObject(false))
            {
                COF.ProduceResourceSource(x, y, mushroomObject);
                mushCount++;
            }

        }
    }
    public void SpawnGrassInArea(int startx, int starty, int width, int height)
    {
        for (int i = startx - 1; i < width + startx + 1; i++)
        {
            for (int d = starty - 1; d < height + starty + 1; d++)
            {
                int r = Random.Range(0, 100);
                if (r <= 50)
                {
                    COF.ProduceBGlObject(i, d, BGObjects.Grass);
                }
                if (r > 50 && r <= 80)
                {
                    COF.ProduceBGlObject(i, d, BGObjects.Grass1);
                }
            }
        }
    }
    private void SpawnGravelAroundMap()
    {
        for (int i = 0; i < map.GetWidth(); i++)
        {
            int r = Random.Range(0, 3);
            if (r <= 1)
                COF.ProduceBGlObject(i, 0, BGObjects.Gravel);
            r = Random.Range(0, 3);
            if (r <= 1)
                COF.ProduceBGlObject(i, map.GetHeight() - 1, BGObjects.Gravel);
        }
        for (int i = 0; i < map.GetHeight(); i++)
        {
            int r = Random.Range(0, 3);
            if (r <= 1)
            {
                COF.ProduceBGlObject(0, i, BGObjects.Gravel);
            }
            r = Random.Range(0, 3);
            if (r <= 1)
            {
                COF.ProduceBGlObject(map.GetWidth() - 1, i, BGObjects.Gravel);
            }
        }
    }
    public void SpawnOre()
    {

    }
}
