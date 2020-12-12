using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public enum RSObjects
{
    Forest,
    DeadAnimal,
    Stone,
    Bush_Berry_Purple,
    Diamond,
    Mushroom,
    ToxicMushroom,
    Iron,
    Coal,
    Gold,
    HardLog,
    DeadAnimalArcheologist
}

public enum CellObjects
{
    Sapling,
    Bush_Berry_Purple
}

public enum BGObjects
{
    Grass,
    Grass1,
    Gravel, 
    Lekno,
    Rumble,
}
public enum AnimalObjects
{
    Mouse,
    Boar,
    Snake,
    Spider
}


class CellObjectFactory : Singleton<CellObjectFactory>
{
    // Resource Sources
    [Header("Resource sources")]
    public GameObject forest;
    public GameObject deadAnimal;
    public GameObject stone;
    public GameObject bush;
    public GameObject bush_berry_purple;
    public GameObject diamond;
    public GameObject mushroom;
    public GameObject toxicFungi;
    public GameObject iron;
    public GameObject coal;
    public GameObject gold;
    public GameObject hardLog;
    public GameObject deadAnimalArcheologist;

    // Cell Objects
    [Header ("Cell objects")]
    public GameObject sapling;
    public GameObject berryless_bush;

    [Header("Background objects")]
    public GameObject[] water;
    public GameObject grass;
    public GameObject grass1;
    public GameObject lekno;
    public GameObject gravel;
    public GameObject rumble;

    [Header("Animals")]
    public GameObject boar;
    public GameObject snake;
    public GameObject mouse;
    public GameObject spider;

    public GameObject ProduceResourceSource(int x, int y, RSObjects type, List<Resource> additionalResources=null)
    {
        GameObject Result = null;
        GameObject selectedPrefab = null;
        ResourceSource createdResourceSource = null;
        switch (type)
        {
            case RSObjects.Forest:
            {
                selectedPrefab = forest;
                break;
            }
            case RSObjects.DeadAnimal:
            {
                selectedPrefab = deadAnimal;
                break;
            }
            case RSObjects.Stone:
            {
                selectedPrefab = stone;
                break;
            }
            case RSObjects.Bush_Berry_Purple:
            {
                selectedPrefab = bush_berry_purple;
                break;
            }
            case RSObjects.Diamond:
            {
                selectedPrefab = diamond;
                break;
            }
            case RSObjects.Coal:
                {
                    selectedPrefab = coal;
                    break;
                }
            case RSObjects.Iron:
                {
                    selectedPrefab = iron;
                    break;
                }
            case RSObjects.Gold:
                {
                    selectedPrefab = gold;
                    break;
                }
            case RSObjects.Mushroom:
                {
                    selectedPrefab = mushroom;
                    break;
                }
            case RSObjects.ToxicMushroom:
                {
                    selectedPrefab = toxicFungi;
                    break;
                }
            case RSObjects.HardLog:
                {
                    selectedPrefab = hardLog;
                    break;
                }
            case RSObjects.DeadAnimalArcheologist:
            {
                selectedPrefab = deadAnimalArcheologist;
                break;
            }
            default:
                break;
        }
        Result = MapControl.Instance.CreateGameObject(x, y, selectedPrefab);
        if (Result != null)
        {
            createdResourceSource = Result.GetComponent<ResourceSource>();
            createdResourceSource?.GenerateResources();
        }
        if (additionalResources != null)
        {
            foreach(Resource newResource in additionalResources)
            {
                createdResourceSource.AddResource(newResource);
            }
        }
        if (createdResourceSource!=null)
        {
            if (createdResourceSource.resource==null)
            {
                Destroy(Result);
                Debug.LogWarning("Destroyed ResourceSource because it had no resources.");
                return null;
            }
        }
        return Result;
    }
    public GameObject ProduceCellObject(int x, int y, CellObjects type)
    {
        GameObject selectedPrefab = null;
        GameObject Result = null;
        switch (type)
        {
            case CellObjects.Sapling:
            {
                selectedPrefab = sapling;
                break;
            }
            case CellObjects.Bush_Berry_Purple:
            {
                selectedPrefab = berryless_bush;
                break;
            }

            default:
            {
                break;
            }
        }
        if (selectedPrefab != null)
        {
            Result = MapControl.Instance.CreateGameObject(x, y, selectedPrefab);
        }

        return Result;
    }

    public GameObject ProduceBGlObject(int x, int y, BGObjects type)
    {
        GameObject selectedPrefab = null;
        GameObject Result = null;
        if (MapControl.Instance.map.Grid[x][y].CanBeEnteredByUnit())
            switch (type)
            {
                case BGObjects.Grass:
                    {
                        selectedPrefab = grass;
                        break;
                    }
                case BGObjects.Grass1:
                    {
                        selectedPrefab = grass1;
                        break;
                    }

                case BGObjects.Gravel:
                    {
                        selectedPrefab = gravel;
                        break;
                    }
                case BGObjects.Lekno:
                    {
                        selectedPrefab = lekno;
                        break;
                    }
                case BGObjects.Rumble:
                    {
                        selectedPrefab = rumble;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        else
        {
            Debug.Log("Dont need to create a BGOBJECT under unit at x:" + x + " y:" + y);
        }
        if (selectedPrefab != null)
        {
            Result = MapControl.Instance.CreateGameObject(x, y, selectedPrefab);
        }

        return Result;
    }
    public GameObject ProduceAnimal(int x, int y, AnimalObjects type)
    {
        GameObject selectedPrefab = null;
        GameObject Result = null;
        switch (type)
        {
            case AnimalObjects.Boar:
                {
                    selectedPrefab = boar;
                    break;
                }
            case AnimalObjects.Snake:
                {
                    selectedPrefab = snake;
                    break;
                }

            case AnimalObjects.Mouse:
                {
                    selectedPrefab = mouse;
                    break;
                }
            case AnimalObjects.Spider:
                {
                    selectedPrefab = spider;
                    break;
                }
            default:
                {
                    break;
                }
        }
        if (selectedPrefab != null)
        {
            Result = MapControl.Instance.CreateGameObject(x, y, selectedPrefab);
        }

        return Result;
    }
}

