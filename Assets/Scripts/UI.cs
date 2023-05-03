using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public TMP_Text userName;
    public Image userAvatar;

    private void Awake()
    {
        userName = transform.Find("Name").GetComponent<TMP_Text>();
        userAvatar = transform.Find("Avatar").GetComponent<Image>();
    }

    private void Reset()
    {
        userName = transform.Find("Name").GetComponent<TMP_Text>();
        userAvatar = transform.Find("Avatar").GetComponent<Image>();
    }

    public void SetNameUIData(string name)
    {
        userName.text = name;

    }

    public void SetImageUIData(string url)
    {
        StartCoroutine(LoadAvatarFromUrl(url));
    }

    public void SetImageUIData(Texture2D image)
    {
        userAvatar.sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), Vector2.one * 0.5f);
    }

    public void ResetUI()
    {
        userAvatar.sprite = null;
        userName.text = "";
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
