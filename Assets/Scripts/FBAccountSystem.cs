using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using TMPro;
using System;
using UnityEngine.Networking;
using System.Collections;

public class FBAccountSystem : MonoBehaviour
{
    #region public var
    #endregion

    #region private var
    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private Image userAvatar;
    private string username;
    private string url;
    #endregion

    private void Awake()
    {
        LoadComponents();

        InitializeFacebookSDK();
    }

    private void Reset()
    {
        LoadComponents();
    }

    private void LoadComponents()
    {
        usernameText = GameObject.Find("Username").GetComponent<TMP_Text>();
        userAvatar = GameObject.Find("UserAvatar").GetComponent<Image>();
    }

    private void InitializeFacebookSDK()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
            return;
        }

        FB.Init(SetInit, HideUnity);
    }

    private void SetInit()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
            Debug.Log("FacebookSDK is initialized!");
            return;
        }

        Debug.Log("Failed to initialize FacebookSDK");
    }

    private void HideUnity(bool isUnityShown)
    {
        if (isUnityShown)
        {
            Time.timeScale = 1;
            return;
        }

        Time.timeScale = 0;
    }

    public void FacebookLogOut()
    {
        if (!FB.IsLoggedIn) return;

        FB.LogOut();
        usernameText.text = null;
        userAvatar.sprite = null;
        Debug.Log("Facebook is logged out!");
    }

    public void FacebookLogin()
    {
        List<string> permissions = new List<string>();
        permissions.Add("public_profile");
        permissions.Add("gaming_profile");
        permissions.Add("gaming_user_picture");
        permissions.Add("email");
        permissions.Add("user_friends");

        FB.LogInWithReadPermissions(permissions, LoginCallBack);
    }

    private void LoginCallBack(ILoginResult result)
    {
        if (result.Error != null)
        {
            Debug.Log(result.Error);
            return;
        }

        if (FB.IsLoggedIn)
        {
            GetUserInformation();
        }
    }

    private void GetUserInformation()
    {
        FB.API("/me?fields=name", HttpMethod.GET, DisplayUsername);
        FB.API("/me?fields=picture", HttpMethod.GET, DisplayUserAvatar);
    }

    private void DisplayUsername(IGraphResult result)
    {
        if (result.Error != null)
        {
            Debug.Log(result.Error);
            return;
        }

        username = result.ResultDictionary["name"].ToString();
        usernameText.text = "Facebook: " + username;
    }

    private void DisplayUserAvatar(IGraphResult result)
    {
        if (result.Error != null)
        {
            Debug.Log(result.Error);
            return;
        }
        var picture = result.ResultDictionary["picture"];
        var data = ((Dictionary<string, object>)picture)["data"];

        url = ((Dictionary<string, object>)data)["url"].ToString();

        PlayfabAccountSystem.Instance.LoginFacebookWithPlayfab(AccessToken.CurrentAccessToken.TokenString, username, url);

        StartCoroutine(LoadAvatarFromUrl(url));
    }

    private IEnumerator LoadAvatarFromUrl(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error while downloading avatar image: " + www.error);
        }
        else
        {
            userAvatar.sprite = Sprite.Create
            (
                ((DownloadHandlerTexture)www.downloadHandler).texture,
                new Rect(0, 0, ((DownloadHandlerTexture)www.downloadHandler).texture.width,
                ((DownloadHandlerTexture)www.downloadHandler).texture.height),
                Vector2.zero
            );
        }
    }
}
