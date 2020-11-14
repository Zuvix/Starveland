using UnityEngine;
class FeedingManager : Singleton<FeedingManager>
{
    public GameObject FeedingPanel;

    private void Awake()
    {
        FeedingPanel.SetActive(false);
    }
    public void InitiateDayEnd()
    {
        FeedingPanel.SetActive(true);
    }
    public void InitiateDayStart(string _)
    {
        FeedingPanel.SetActive(false);
        DayCycleManager.Instance.StartDay();
    }
}
