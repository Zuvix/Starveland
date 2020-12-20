using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntingField : MonoBehaviour
{
    Map map;
    CellObjectFactory COF;
    public int maxBerryCount = 5;


    [Header("Animals")]
    public int boarMaxCount = 4;
    public int snakeMaxCount = 3;
    private void Awake()
    {
        COF = CellObjectFactory.Instance;
    }
    public void SpawnHuntingZone(int width, int height, int startx, int starty)
    {
        map = MapControl.Instance.map;
        //Lake
        CreateLake(map.GetWidth() - 9, map.GetHeight() - 7, 3, 3);
        //Berries
        sur[] berryPositions = new sur[5];
        berryPositions[0] = new sur(map.GetWidth() - 3, map.GetHeight() - 4);
        berryPositions[1] = new sur(map.GetWidth() - 3, map.GetHeight() - 7);
        berryPositions[2] = new sur(18, 13);
        berryPositions[3] = new sur(21, 16);
        berryPositions[4] = new sur(19, 18);
        SpawnBerries(berryPositions);
        //Snake
        COF.ProduceAnimal(map.GetWidth() - 3, map.GetHeight() - 5, AnimalObjects.Snake);
        //Boar
        MapControl.Instance.SpawnAnimalAnywhereInArea(AnimalObjects.Boar, boarMaxCount, startx + 4, starty, (width / 2) - 3, height);
        //Long Grass
        SpawnLongGrassInArea(startx, starty, width + 1, height);
    }
    public void SpawnBerries(sur[] positions)
    {
        foreach (sur s in positions)
        {
            COF.ProduceResourceSource(s.x, s.y, RSObjects.Bush_Berry_Purple);
        }
    }
    private void CreateLake(int startx, int starty, int lakexSize, int lakeySize)
    {
        int x = startx;
        int y = starty;
        if (x != -1 && y != -1)
        {
            GameObject go=MapControl.Instance.CreateGameObject(x, y, COF.water[4]);
            Debug.Log(go.transform.position);
            MapControl.Instance.CreateGameObject(x + lakexSize, y, COF.water[5]);
            MapControl.Instance.CreateGameObject(x + lakexSize, y + lakeySize, COF.water[6]);
            MapControl.Instance.CreateGameObject(x, y + lakeySize, COF.water[7]);
            for (int i = x + 1; i < x + lakexSize; i++)
            {
                MapControl.Instance.CreateGameObject(i, y, COF.water[0]);
                MapControl.Instance.CreateGameObject(i, y + lakeySize, COF.water[2]);
            }
            for (int i = y + 1; i < y + lakeySize; i++)
            {
                MapControl.Instance.CreateGameObject(x, i, COF.water[3]);
                MapControl.Instance.CreateGameObject(x + lakexSize, i, COF.water[1]);
            }
            for (int i = x + 1; i < x + lakexSize; i++)
            {
                for (int d = y + 1; d < y + lakeySize; d++)
                {

                    MapControl.Instance.CreateGameObject(i, d, COF.water[8]);
                }
            }
            for (int i = x - 1; i < x + lakexSize + 2; i++)
            {
                COF.ProduceBGlObject(i, y - 1, BGObjects.SeaStone);
                COF.ProduceBGlObject(i, lakeySize + y + 1, BGObjects.SeaStone);
            }
            for (int i = y - 1; i < y + lakeySize + 1; i++)
            {
                COF.ProduceBGlObject(x - 1, i, BGObjects.SeaStone);
                COF.ProduceBGlObject(lakexSize + x + 1, i, BGObjects.SeaStone);
            }
        }
    }
    public void SpawnLongGrassInArea(int startx, int starty, int width, int height)
    {
        for (int i = startx - 1; i < width + startx + 1; i++)
        {
            for (int d = starty - 1; d < height + starty + 1; d++)
            {
                int r = Random.Range(0, 100);
                if (r <= 85)
                {
                    COF.ProduceBGlObject(i, d, BGObjects.LongGrass);
                }
            }
        }
    }
}
