using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mines : MonoBehaviour
{
    [Header("Resource Sources")]
    public int rockMaxCount = 40;
    public int ironMaxCount = 6;
    public int coalMaxCount = 5;
    public int goldMaxCount = 3;

    [Header("Animals")]
    public int spiderMaxCount = 5;
    CellObjectFactory COF;
    Map map;
    private void Awake()
    {
        COF=CellObjectFactory.Instance;
        map = MapControl.Instance.map;
    }
    public void SpawnMines(int width, int height, int startx, int starty)
    {
        map = MapControl.Instance.map;
        RSObjects[] rocks;
        int rocksGold = Mathf.FloorToInt(rockMaxCount / 3);
        int rocksIron = Mathf.FloorToInt(rockMaxCount / 3);
        int rocksCoal = Mathf.FloorToInt(rockMaxCount / 3);

        //Spawn Gold
        MapCell goldCell = map.Grid[Mathf.FloorToInt(3 * width / 4 + startx + 1)][Mathf.FloorToInt(height / 4 + starty)];
        RSObjects[] gold = new RSObjects[goldMaxCount];
        FillArray(gold, RSObjects.Gold);
        rocks = new RSObjects[rocksGold];
        FillArray(rocks, RSObjects.Stone);
        SpawnOres(gold, rocks, goldCell);

        //Spawn iron
        MapCell ironCell = map.Grid[Mathf.FloorToInt(3 * width / 4 + startx - 4)][Mathf.FloorToInt(height / 4 + starty + 2)];
        RSObjects[] iron = new RSObjects[ironMaxCount];
        FillArray(iron, RSObjects.Iron);
        rocks = new RSObjects[rocksIron];
        FillArray(rocks, RSObjects.Stone);
        SpawnOres(iron, rocks, ironCell);

        //Spawn Coal
        MapCell coalCell = map.Grid[Mathf.FloorToInt(3 * width / 4 + startx + 1)][Mathf.FloorToInt(height / 4 + starty + 3)];
        RSObjects[] coal = new RSObjects[coalMaxCount];
        FillArray(coal, RSObjects.Coal);
        rocks = new RSObjects[rocksCoal];
        FillArray(rocks, RSObjects.Stone);
        SpawnOres(coal, rocks, coalCell);
        //Spawn rumble
        MapControl.Instance.SpawnRSAnywhereInArea(RSObjects.Stone, Random.Range(2, 3), startx + 4, starty + 1, 1, 1);
        MapControl.Instance.SpawnRSAnywhereInArea(RSObjects.Stone, Random.Range(2, 4), startx + 4, starty + 1, 1, 1);
        MapControl.Instance.SpawnRSAnywhereInArea(RSObjects.Stone, Random.Range(2, 4), startx + 5, starty + 3, 1, 1);
        SpawnRumble(startx - 1, starty, width + 3, height);
        MapControl.Instance.SpawnAnimalAnywhereInArea(AnimalObjects.Spider, spiderMaxCount, startx + 6, starty, width - 6, height - 5);
    }

    private RSObjects[] FillArray(RSObjects[] inputArray, RSObjects desiredResource)
    {
        for (int i = 0; i < inputArray.Length; i++)
        {
            inputArray[i] = desiredResource;
        }
        return inputArray;
    }
    public void SpawnOres(RSObjects[] treasure, RSObjects[] fodder, MapCell startCell)
    {
        var allProducts = new List<RSObjects>(treasure.Length + fodder.Length);
        allProducts.AddRange(treasure);
        allProducts.AddRange(fodder);
        RandomizeList<RSObjects>(allProducts, treasure.Length + fodder.Length / 3, 1);
        COF.ProduceResourceSource(startCell.x, startCell.y, treasure[0]);
        for (int i = 1; i < allProducts.Count; i++)
        {
            MapCell emptyCell = startCell.GetRandomUnitEnterableNeighbour();
            if (emptyCell != null)
            {
                COF.ProduceResourceSource(emptyCell.x, emptyCell.y, allProducts[i]);
            }
        }

    }
    public void SpawnRumble(int startx, int starty, int width, int height)
    {
        for (int i = startx; i < width + startx; i++)
        {
            for (int d = starty; d < height + starty; d++)
            {
                List<MapCell> neighbours = map.Grid[i][d].GetClosestNeighbours();
                foreach (MapCell mc in neighbours)
                {
                    if (!mc.CanBeEnteredByObject(true))
                    {
                        if (mc.CurrentObject is ResourceSource)
                        {
                            COF.ProduceBGlObject(i, d, BGObjects.Rumble);
                            break;
                        }

                    }
                }
            }
        }
    }
    private void RandomizeList<T>(List<T> inputArray, int max, int min)
    {
        for (int i = max - 1; i > min; i--)
        {
            int randomIndex = Random.Range(min, i + 1);

            T temp = inputArray[i];
            inputArray[i] = inputArray[randomIndex];
            inputArray[randomIndex] = temp;
        }
    }
}
