using System.Collections.Generic;

[System.Serializable]
public class FacebookFriendUser
{
    public string id;
    public string name;
    public Picture picture;
}

[System.Serializable]
public class Picture
{
    public PictureData data;
}

[System.Serializable]
public class PictureData
{
    public int height;
    public bool is_silhouette;
    public string url;
    public int width;
}

[System.Serializable]
public class FBFriendData
{
    public List<FacebookFriendUser> listUserData;
}

