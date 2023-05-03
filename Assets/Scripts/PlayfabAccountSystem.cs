using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;
using System;

public class PlayfabAccountSystem : MonoBehaviour
{
    private static PlayfabAccountSystem instance;

    public static PlayfabAccountSystem Instance { get => instance; set => instance = value; }

    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private Image userAvatar;
    [SerializeField] private GameObject playfabScoreSystem;

    private void Awake()
    {
        instance = this;

        playfabScoreSystem = transform.parent.Find("PlayfabScoreSystem").gameObject;
        usernameText = GameObject.Find("PlayfabUsername").GetComponent<TMP_Text>();
        userAvatar = GameObject.Find("PlayfabUserAvatar").GetComponent<Image>();
    }

    private void Reset()
    {
        playfabScoreSystem = transform.parent.Find("PlayfabScoreSystem").gameObject;
        usernameText = GameObject.Find("PlayfabUsername").GetComponent<TMP_Text>();
        userAvatar = GameObject.Find("PlayfabUserAvatar").GetComponent<Image>();
    }

    public void LoginFacebookWithPlayfab(string accessToken, string displayName, string url)
    {
        LoginWithFacebookRequest request = new LoginWithFacebookRequest
        {
            AccessToken = accessToken,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithFacebook(request,
        result =>
        {
            UpdateDisplayNamePlayfab(displayName);
            UpdateAvatarPlayfab(url);
            Debug.Log("Log in Facebook with Playfab successfully!");
            playfabScoreSystem.gameObject.SetActive(true);
        }
        , OnLoginFailed);
    }

    private void OnLoginFailed(PlayFabError error)
    {
        Debug.Log(error.ErrorMessage);
    }

    private void UpdateAvatarPlayfab(string url)
    {
        UpdateAvatarUrlRequest request = new UpdateAvatarUrlRequest
        {
            ImageUrl = url
        };

        PlayFabClientAPI.UpdateAvatarUrl(request, OnUpdateSuccess, OnUpdateFailed);
    }

    private void OnUpdateSuccess(EmptyResponse response)
    {
        Debug.Log("Update Avatar Image's URL successfully!");
    }

    private void UpdateDisplayNamePlayfab(string username)
    {
        UpdateUserTitleDisplayNameRequest request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = username
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnUpdateSuccess, OnUpdateFailed);

        usernameText.text = "Playfab: " + username;
    }

    private void OnUpdateFailed(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    private void OnUpdateSuccess(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Update Display Name successfully!");
    }
}
