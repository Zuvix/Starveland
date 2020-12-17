using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Areal : MonoBehaviour
{
    public int FishingSpotCount;

    Map map;
    CellObjectFactory COF;
    private void Awake()
    {
        COF = CellObjectFactory.Instance;
    }
    public void GenerateAreal()
    {
        map = MapControl.Instance.map;
        //SpawnWaterAroundIsland();
        ProduceSpecificRoad();
        SpawnGravelAroundMap();
        ProduceFishingSpots();
    }
    private void SpawnWaterAroundIsland()
    {
        MapControl.Instance.CreateGameObject(0, 0, COF.water[4]);
        MapControl.Instance.CreateGameObject(map.GetWidth() - 1, 0, COF.water[5]);
        MapControl.Instance.CreateGameObject(map.GetWidth() - 1, map.GetHeight() - 1, COF.water[6]);
        MapControl.Instance.CreateGameObject(0, map.GetHeight() - 1, COF.water[7]);
        for (int i = 1; i < map.GetWidth() - 1; i++)
        {
            MapControl.Instance.CreateGameObject(i, 0, COF.water[0]);
            MapControl.Instance.CreateGameObject(i, map.GetHeight() - 1, COF.water[2]);
        }
        for (int i = 1; i < map.GetHeight() - 1; i++)
        {
            MapControl.Instance.CreateGameObject(0, i, COF.water[3]);
            MapControl.Instance.CreateGameObject(map.GetWidth() - 1, i, COF.water[1]);
        }
    }
    private void SpawnGravelAroundMap()
    {
        for (int i = 0; i < map.GetWidth(); i++)
        {
            int r = Random.Range(0, 3);
            if (r <= 1)
                COF.ProduceBGlObject(i, 0, BGObjects.SeaStone);
            r = Random.Range(0, 3);
            if (r <= 1)
                COF.ProduceBGlObject(i, map.GetHeight() - 1, BGObjects.SeaStone);
        }
        for (int i = 0; i < map.GetHeight(); i++)
        {
            int r = Random.Range(0, 3);
            if (r <= 1)
            {
                COF.ProduceBGlObject(0, i, BGObjects.SeaStone);
            }
            r = Random.Range(0, 3);
            if (r <= 1)
            {
                COF.ProduceBGlObject(map.GetWidth() - 1, i, BGObjects.SeaStone);
            }
        }
    }
    private void ProduceSpecificRoad()
    {
        for (int i = 0; i < 11; i++)
        {
            COF.ProduceBGlObject(17, i, BGObjects.Gravel);
            if (i % 2 == 0)
            {
                COF.ProduceBGlObject(16, i, BGObjects.Gravel);
            }
            else
            {
                COF.ProduceBGlObject(18, i, BGObjects.Gravel);
            }
        }
        for (int i = 5; i < 11; i++)
        {
            if (i % 2 == 0)
            {
                COF.ProduceBGlObject(19, i, BGObjects.Gravel);
            }
            else
            {
                COF.ProduceBGlObject(20, i, BGObjects.Gravel);
            }
        }
        COF.ProduceBGlObject(18, 0, BGObjects.Gravel);
        COF.ProduceBGlObject(21, 8, BGObjects.Gravel);
        COF.ProduceBGlObject(21, 10, BGObjects.Gravel);
        COF.ProduceBGlObject(22, 9, BGObjects.Gravel);
        COF.ProduceBGlObject(23, 10, BGObjects.Gravel);
        COF.ProduceBGlObject(23, 8, BGObjects.Gravel);
        COF.ProduceBGlObject(24, 9, BGObjects.Gravel);
        COF.ProduceBGlObject(25, 10, BGObjects.Gravel);
        COF.ProduceBGlObject(26, 9, BGObjects.Gravel);
        COF.ProduceBGlObject(27, 10, BGObjects.Gravel);
    }
    private void ProduceFishingSpots()
    {
        if (FishingSpotCount < 1) { return; }
        PlaceFishingSpot(0, Random.Range(1, map.GetHeight() - 2));
        if (FishingSpotCount < 2) { return; }
        PlaceFishingSpot(map.GetWidth() - 1, Random.Range(1, map.GetHeight() - 2));
        if (FishingSpotCount < 3) { return; }
        PlaceFishingSpot(Random.Range(1, map.GetWidth() - 2), 0);
        if (FishingSpotCount < 4) { return; }
        PlaceFishingSpot(Random.Range(1, map.GetWidth() - 2), map.GetHeight() - 1);
        if (FishingSpotCount < 5) { return; }
    }
    private void PlaceFishingSpot(int x, int y)
    {
        MapControl.Instance.map.Grid[x][y].EraseCellObject();
        COF.ProduceResourceSource(x, y, RSObjects.FishingSport);
    }
}
