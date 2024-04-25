using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.Json;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class PlayFabManager : MonoBehaviour
{
   public static PlayFabManager PFM;

    public TMP_InputField username, userEmail, userPassword, userEmailLogin, userPasswordLogin;
    public TMP_Text errorSignUp, errorLogin;
    public Text levelDisplay;
    public Text itemsCollected;
    public Text playerscore;


    public void OnEnable()
    {
        if(PlayFabManager.PFM == null)
        {
            PlayFabManager.PFM = this;
        }
        else
        {
            if(PlayFabManager.PFM != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }


    public int items = GameController.GC.items;
    public int currentLevel;

    public int playerScore;

#region PlayerStatistics

    public void SendLeaderboard(int currentLevel, int playerScore, int items)
    {
        //PlayFabClientAPI.UpdatePlayerStatistics (new UpdatePlayerStatisticsRequest
        var request = new UpdatePlayerStatisticsRequest
        {
            //request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate { StatisticName = "CurrentLevel", Value = currentLevel},
                new StatisticUpdate { StatisticName = "PlayerScore", Value = playerScore},
                new StatisticUpdate { StatisticName = "Items", Value = items},

            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnErrorShared);
        //result => {Debug.Log("User statistics updated"); },
        //error => {Debug.LogError(error.GenerateErrorReport()); });
    } 

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successful leaderboard sent");
    }  

    public void GetLeaderboard()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnLeaderboardGet,
            error => Debug.LogError(error.GenerateErrorReport()));
    } 

    void OnLeaderboardGet(GetPlayerStatisticsResult result)
    {
        Debug.Log("Received the following statistics:");

        foreach(var eachStat in result.Statistics)
        {
            Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
            switch(eachStat.StatisticName)
            {
                case "CurrentLevel":
                    currentLevel = eachStat.Value;
                    break;
                case "PlayerScore":
                    playerScore = eachStat.Value;
                    break;   
                case "Items":
                    items = eachStat.Value;
                    break;
                 
            }    

        }

        UpdateUI();

    }
    
    void UpdateUI()
        {
            // Assuming LevelDisplay is a Text component in your Canvas
            if (GameController.GC.levelDisplay != null)
            {
                GameController.GC.levelDisplay.text = $"Level: {currentLevel}\nScore: {playerScore}\nItems: {items}";
            }
            else
            {
                Debug.LogWarning("LevelDisplay UI component not assigned!");
            }
        }
         
    //Build the request object and access the API
    public void StartCloudUpdatePlayerStats()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdatePlayerStats", //Arbitrary function name (must exist in uploaded Cloud .js file)
            FunctionParameter = new {Level = currentLevel, PlayerScore = playerScore, Items = items}, //The parameter provided to your function
            GeneratePlayStreamEvent = true, //Optional - Shows this event in PlayStream
        }, OnCloudUpdateStats, OnErrorShared);
        
    }
    //OnCloudUpdateStats defined in the next block

    private static void OnCloudUpdateStats(ExecuteCloudScriptResult result)
    {
        //Cloud Script returns arbitrary results, so you have to evaluate them one step and one parameter at a time
        //Debug.Log(JsonWrapper.SerializeObject(result.FunctionResult));
        JsonObject jsonResult = (JsonObject)result.FunctionResult;
        object messageValue;
        jsonResult.TryGetValue("messageValue", out messageValue);
        Debug.Log((string)messageValue);
    } 

    private static void OnErrorShared(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

#endregion PlayerStatistics    
}
