using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendListUI : MonoBehaviour
{
    private static FriendListUI instance;
    public static FriendListUI Instance { get => instance; }
    public List<UI> uiList = new List<UI>();


    private void Awake()
    {
        instance = this;

        for (int i = 0; i < transform.childCount; i++)
        {
            UI ui = transform.GetChild(i).GetComponent<UI>();
            if (uiList.Contains(ui)) continue;

            uiList.Add(ui);
        }
    }

    private void Reset()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            UI ui = transform.GetChild(i).GetComponent<UI>();
            if (uiList.Contains(ui)) continue;

            uiList.Add(ui);
        }
    }

    public void ClearList()
    {
        foreach (UI ui in uiList)
        {
            ui.ResetUI();
        }
    }
}
