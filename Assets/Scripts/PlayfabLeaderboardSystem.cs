using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabLeaderboardSystem : MonoBehaviour
{
    public void GetLeaderBoardFromPlayfab()
    {
        PlayerProfileViewConstraints playerProfileViewConstraints = new PlayerProfileViewConstraints { ShowAvatarUrl = true };

        GetLeaderboardRequest request = new GetLeaderboardRequest
        {
            ProfileConstraints = playerProfileViewConstraints,
            StatisticName = "Score"
        };

        PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboard, OnGetLeaderboardError);
    }

    public void GetFriendLeaderboardFromPlayfab()
    {
        PlayerProfileViewConstraints playerProfileViewConstraints = new PlayerProfileViewConstraints { ShowAvatarUrl = true };

        GetFriendLeaderboardAroundPlayerRequest request = new GetFriendLeaderboardAroundPlayerRequest
        {
            ProfileConstraints = playerProfileViewConstraints,
            ExternalPlatformFriends = ExternalFriendSources.Facebook,
            StatisticName = "Score"
        };

        PlayFabClientAPI.GetFriendLeaderboardAroundPlayer(request, OnGetLeaderboard, OnGetLeaderboardError);
    }

    private void OnGetLeaderboard(GetFriendLeaderboardAroundPlayerResult result)
    {
        FriendListUI.Instance.ClearList();

        for (int i = 0; i < result.Leaderboard.Count; i++)
        {
            string username = result.Leaderboard[i].DisplayName;
            string score = result.Leaderboard[i].StatValue.ToString();
            string url = result.Leaderboard[i].Profile.AvatarUrl;

            FriendListUI.Instance.uiList[i].SetNameUIData(username + " " + score);
            FriendListUI.Instance.uiList[i].SetImageUIData(url);
        }
    }

    private void OnGetLeaderboardError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    private void OnGetLeaderboard(GetLeaderboardResult result)
    {
        FriendListUI.Instance.ClearList();

        for (int i = 0; i < result.Leaderboard.Count; i++)
        {
            string username = result.Leaderboard[i].DisplayName;
            string score = result.Leaderboard[i].StatValue.ToString();
            string url = result.Leaderboard[i].Profile.AvatarUrl;

            FriendListUI.Instance.uiList[i].SetNameUIData(username + " " + score);
            FriendListUI.Instance.uiList[i].SetImageUIData(url);
        }
    }
}
