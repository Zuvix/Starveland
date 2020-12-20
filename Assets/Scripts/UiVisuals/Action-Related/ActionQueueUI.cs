using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueueUI : MonoBehaviour
{
    public GameObject ActionUI;
    public GameObject ActionUILayout;
    List<ActionShow> actionList;
    private int maxActions;

    private void Awake()
    {
        this.maxActions = GameConfigManager.Instance.GameConfig.MaxQueueActions;
        actionList = new List<ActionShow>();
        for (int i = 0; i < maxActions; i++)
        {
            GameObject itm = Instantiate(ActionUI, this.ActionUILayout.transform);
            actionList.Add(itm.GetComponent<ActionShow>());
            itm.SetActive(false);
        }
    }

    private void Start()
    {
        UnitManager.Instance.onActionQueueChanged.AddListener(this.AssignActions);
    }

    public void AssignActions()
    {
        if (PanelControl.Instance.GetActivePanelID() == 1)
        {
            int i = 0;
            foreach (ActionShow panel in actionList)
            {
                panel.gameObject.SetActive(false);
            }
            foreach (var action in UnitManager.Instance.ActionQueue)
            {
                actionList[i].ShowItem(action.Item3);
                i++;
            }
        }
    }
}
