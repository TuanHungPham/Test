using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using TMPro;
using System;

public class FBFriendSystem : MonoBehaviour
{
    #region public var
    #endregion

    #region private var
    [SerializeField] private int index;
    #endregion

    private void Awake()
    {
        LoadComponents();
    }

    private void Reset()
    {
        LoadComponents();
    }

    private void LoadComponents()
    {
    }

    public void GetUserFriend()
    {
        if (!FB.IsLoggedIn) return;

        FB.API("/me/friends", HttpMethod.GET, GetFriendOnFacebook);
    }

    private void GetFriendOnFacebook(IGraphResult result)
    {
        var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
        var listOfFriend = (List<object>)dictionary["data"];

        DisplayFriendInformation(listOfFriend);
    }

    private void DisplayFriendInformation(List<object> listOfFriend)
    {
        foreach (UI ui in FriendListUI.Instance.uiList)
        {
            int index = FriendListUI.Instance.uiList.IndexOf(ui);

            if (index > listOfFriend.Count - 1) return;

            string id = ((Dictionary<string, object>)listOfFriend[index])["id"].ToString();
            string query = id + "/picture";

            FB.API(query, HttpMethod.GET,
            result =>
            {
                Texture2D avatar = result.Texture;
                ui.SetImageUIData(avatar);
            }
            );

            string userName = ((Dictionary<string, object>)listOfFriend[index])["name"].ToString();
            ui.SetNameUIData(userName);
        }
    }
}
