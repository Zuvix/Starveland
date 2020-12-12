using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest : MonoBehaviour
{
    Map map;
    public RSObjects[] Foragables = { RSObjects.Bush_Berry_Purple };
    CellObjectFactory COF;
    public int treeMaxCount = 40;
    int treeCount = 0;
    [Header("Resource Sources")]
    public int hardLogMaxCount = 5;
    public int mushroomMaxCount = 6;
    public int toxicMaxFungiCount = 4;
    [Header("Animals")]
    public int mouseMaxCount = 6;
    private void Awake()
    {
        map = MapControl.Instance.map;
        COF = CellObjectFactory.Instance;
    }
    public void SpawnForest2(int width, int forestySize, int startx, int starty)
    {
        map = MapControl.Instance.map;
        if (startx < 0 && starty < 0) return;
        if (width < 0 && forestySize < 0) return;
        if (startx + width > map.GetWidth() - 1) return;
        if (starty + forestySize > map.GetHeight() - 1) return;
        List<sur> usedSpots = new List<sur>();

        //Spawn trees
        SpawnTrees(startx, starty, width, forestySize, out usedSpots);
        //Normal mushrooms
        SpawnMushrooms(RSObjects.Mushroom, mushroomMaxCount, usedSpots);
        //Toxic mushrooms
        SpawnMushrooms(RSObjects.ToxicMushroom, toxicMaxFungiCount, usedSpots);
        //Animals
        MapControl.Instance.SpawnAnimalAnywhereInArea(AnimalObjects.Mouse, mouseMaxCount, startx, starty, width, forestySize);
        //Spawn the grass
        SpawnGrassInArea(startx, starty, width + 1, forestySize);

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
    public int GetRandomIndex(int max)
    {
        int randomIndex = Random.Range(0, max);
        return randomIndex;
    }
}
