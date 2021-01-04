using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class GpgsScript : MonoBehaviour
{
    long points = 0;
    public Text pointsText;
    public string leaderBoard, acheievementIDs;

    private String _authCode;
    void Start()
    {
        // Recommended for debugging
        PlayGamesPlatform.DebugLogEnabled = true;

        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
    }

    /// <summary>
    /// Make Login and manage the succes or failure
    /// </summary>
    public void LogIn()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                _authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                ILocalUser instanceLocalUser = PlayGamesPlatform.Instance.localUser;
                var userName = instanceLocalUser.userName;
                Debug.Log("Login Sucess");

                LoadScores();
            }
            else
            {
                Debug.Log("Login failed");
            }
        });
    }

    /// <summary>
    /// Shows Leaderboard
    /// </summary>
    public void OnShowLeaderBoard()
    {
        //Social.ShowLeaderboardUI (); // Show all leaderboard
        PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderBoard);
    }

    /// <summary>
    /// Adds score to Leaderboard
    /// </summary>
    public void addScoreLeaderBoard()
    {
        Debug.Log("addScoreLeaderBoard: points = " + points);
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(points, leaderBoard, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Points: " + points);
                }
                else
                {
                    Debug.Log("Update Score Fail");
                }
            });
        }
    }

    /// <summary>
    /// Unlock Reward
    /// </summary>
    public void rewardAchiv()
    {
        Social.ReportProgress(acheievementIDs, 200.0f, (bool success) =>
        {
            // handle success or failure
        });
    }    
    
    /// <summary>
    /// Showing the Achievements UI
    /// </summary>
    public void showAchievementsUI()
    {
        // show achievements UI
        Social.ShowAchievementsUI();
    }

    /// <summary>
    /// Adding points
    /// </summary>
    public void MorePoints(long addPoints)
    {
        points += addPoints;
        Debug.Log("morePoints after addPoints: " + points);
    }

    /**
     * Load scores from board
     */
    private void LoadScores()
    {
        Social.LoadScores(leaderBoard, scores =>
        {
            if (scores.Length > 0)
            {
                Debug.Log("Got " + scores.Length + " scores");
                string myScores = "Leaderboard:\n";
                foreach (IScore score in scores)
                {
                    myScores +="Your scores: " + "\t" + score.userID + " " + score.formattedValue + " " + score.date + "\n";
                    points = score.value;
                }

                Debug.Log(myScores);
                Debug.Log("My points: " + points);
            }
            else
                Debug.Log("No scores loaded");
        });
    }

    /// <summary>
    /// Log Out
    /// </summary>
    public void OnLogOut()
    {
        ((PlayGamesPlatform) Social.Active).SignOut();
    }
}