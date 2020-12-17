using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

class GameOver : Singleton<GameOver>
{
    [HideInInspector]
    public bool GameIsOver { get; private set; } = false;
    public static readonly string INTERSCENE_VARIABLE_NAME_PLAYER_COUNT = "PlayersSurvived";
    public static readonly string INTERSCENE_VARIABLE_NAME_GAME_OVER_RESULT = "GameOver";
    public void IndicatePlayerUnitDeath()
    {
        if (UnitManager.Instance.PlayerUnitPool.Count <= 0)
        {
            InitiateNegativeGameOver();
        }
    }
    public void InitiateNegativeGameOver()
    {
        ClearOnGameEnd();

        PlayerPrefs.SetInt(INTERSCENE_VARIABLE_NAME_PLAYER_COUNT, 0);
        PlayerPrefs.SetInt(INTERSCENE_VARIABLE_NAME_GAME_OVER_RESULT, 0);
        SceneManager.LoadScene(GUIReference.Instance.SCENE_INDEX_GAMEOVER);
    }
    public void InitiatePositiveGameOver()
    {
        ClearOnGameEnd();

        PlayerPrefs.SetInt(INTERSCENE_VARIABLE_NAME_PLAYER_COUNT, UnitManager.Instance.PlayerUnitPool.Count);
        PlayerPrefs.SetInt(INTERSCENE_VARIABLE_NAME_GAME_OVER_RESULT, 1);
        SceneManager.LoadScene(GUIReference.Instance.SCENE_INDEX_GAMEOVER);
    }
    private void ClearOnGameEnd()
    {
        GameIsOver = true;
        Destroy(DayCycleManager.Instance);
        FeedingManager.Instance.FeedingPanel.SetActive(false);
        GlobalGameState.Instance.InGameInputAllowed = false;
    }
}
