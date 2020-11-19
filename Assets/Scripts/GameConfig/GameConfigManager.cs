using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class GameConfigManager : MonoBehaviour
{ 
    public GameConfig GameConfig;
    public static GameConfigManager Instance { get; private set; }

    public void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
    }
}*/

public class GameConfigManager : Singleton<GameConfigManager>
{
    public GameConfig GameConfig; 
}

/*public class GameConfigManager : MonoBehaviour
{
    public GameConfig GameConfig;
    private static GameConfigManager inst = null;
    public static GameConfigManager Instance
    {
        get
        {
            if (inst == null)
            {
                inst = this;
            }
            return inst;
        }
    }
}*/