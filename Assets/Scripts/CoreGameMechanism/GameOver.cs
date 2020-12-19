using UnityEngine;
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

        PlayerPrefs.SetInt(InterSceneVariables.PlayerUnitsSurvivedCount, 0);
        PlayerPrefs.SetInt(InterSceneVariables.GameFinishState, GameFinishState.Loss);
        SceneManager.LoadScene(2);
    }
    public void InitiatePositiveGameOver()
    {
        GameIsOver = true;
        Destroy(DayCycleManager.Instance);
        FeedingManager.Instance.FeedingPanel.SetActive(false);
        GlobalGameState.Instance.InGameInputAllowed = false;

        PlayerPrefs.SetInt(InterSceneVariables.PlayerUnitsSurvivedCount, Unit.PlayerUnitPool.Count);
        PlayerPrefs.SetInt(InterSceneVariables.GameFinishState, GameFinishState.Victory);
        SceneManager.LoadScene(2);
    }
}