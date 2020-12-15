using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

class GameOver : Singleton<GameOver>
{
    public bool GameIsOver { get; private set; }
    public void Awake()
    {
        GameIsOver = false;
    }
    public void IndicatePlayerUnitDeath()
    {
        if (Unit.PlayerUnitPool.Count <= 0)
        {
            InitiateNegativeGameOver();
        }
    }
    public void InitiateNegativeGameOver()
    {
        GameIsOver = true;
        Destroy(DayCycleManager.Instance);
        FeedingManager.Instance.FeedingPanel.SetActive(false);
        GlobalGameState.Instance.InGameInputAllowed = false;
        /*foreach (Unit Unit in Unit.UnitPool)
        {
            Destroy(Unit);
        }
        Unit.UnitPool.Clear();*/

        Debug.LogError("All player units are dead. Game is over!");

        PlayerPrefs.SetInt("PlayersSurvived", 0);
        PlayerPrefs.SetInt("GameOver", 0);
        SceneManager.LoadScene(2);
    }
    public void InitiatePositiveGameOver()
    {
        GameIsOver = true;
        Destroy(DayCycleManager.Instance);
        FeedingManager.Instance.FeedingPanel.SetActive(false);
        GlobalGameState.Instance.InGameInputAllowed = false;

        Debug.LogError("Ship is built. Game won!");

        PlayerPrefs.SetInt("PlayersSurvived", Unit.PlayerUnitPool.Count);
        PlayerPrefs.SetInt("GameOver", 1);
        SceneManager.LoadScene(2);
    }
}
