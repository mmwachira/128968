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

    // public void Start()
    // {
    //     if (PlayerPrefs.HasKey("EMAIL"))
    //     {
    //         userEmail.text = PlayerPrefs.GetString("EMAIL");
    //         userPassword.text = PlayerPrefs.GetString("PASSWORD");
    //         var request = new LoginWithEmailAddressRequest {Email = userEmailLogin.text, Password = userPasswordLogin.text};
    //         PlayFabClientAPI.LoginWithEmailAddress(request, LogInSuccess, LogInError);
    //     }
    // }

// #region Login

//     public void RegisterSuccess(RegisterPlayFabUserResult result) 
//     {
//         PlayerPrefs.SetString("EMAIL", userEmailLogin.text);
//         PlayerPrefs.SetString("PASSWORD", userPasswordLogin.text);
//         errorSignUp.text = "";
//         errorLogin.text = "";
//         StartGame();
//     }

//     public void RegisterError(PlayFabError error){
//         errorSignUp.text = error.GenerateErrorReport();
//     }


//     public void LogInSuccess(LoginResult result){
//         Debug.Log("Successful Login");

//         PlayerPrefs.SetString("EMAIL", userEmailLogin.text);
//         PlayerPrefs.SetString("PASSWORD", userPasswordLogin.text);
//         errorSignUp.text = "";
//         errorLogin.text = "";
//         GetLeaderboard();
//         StartGame();
//     }

//     public void LogInError(PlayFabError error){
//         errorLogin.text = error.GenerateErrorReport();
//     }
    
//     void StartGame(){
//         startPanel.SetActive(false);
//         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
//     }

// #endregion Login


    public int playerLevel;
    public int gameLevel;

    public int playerHighscore;

#region PlayerStatistics

    public void SendLeaderboard()
    {
        PlayFabClientAPI.UpdatePlayerStatistics (new UpdatePlayerStatisticsRequest
        {
            //request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate { StatisticName = "PlayerLevel", Value = playerLevel},
                new StatisticUpdate { StatisticName = "PlayerHighScore", Value = playerHighscore},
                new StatisticUpdate { StatisticName = "GameLevel", Value = gameLevel},

            }
        },
        result => {Debug.Log("User statistics updated"); },
        error => {Debug.LogError(error.GenerateErrorReport()); });
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
                case "PlayerLevel":
                    playerLevel = eachStat.Value;
                    break;
                case "PlayerHighScore":
                    playerHighscore = eachStat.Value;
                    break;   
                case "GameLevel":
                    gameLevel = eachStat.Value;
                    break;
                 
            }
        }
    } 

    //Build the request object and access the API
    public void StartCloudUpdatePlayerStats()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdatePlayerStats", //Arbitrary function name (must exist in uploaded Cloud .js file)
            FunctionParameter = new {Level = playerLevel, PlayerHighScore = playerHighscore, GameLevel = gameLevel}, //The parameter provided to your function
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
