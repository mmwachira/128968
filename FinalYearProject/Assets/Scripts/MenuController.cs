using System.Text;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuController : MonoBehaviour
{ 
    [Header ("Login")]
    [SerializeField] GameObject signUpTab, logInTab, resetTab, startPanel, usernamebackground;
    public TMP_InputField username, userEmail, userPassword, userEmailLogin, userPasswordLogin, userEmailRecovery;
    public TMP_Text errorSignUp, errorLogin, showusername;
    string encryptedPassword;
    UTF8Encoding utf8 = new UTF8Encoding();

    [Space] 

    [Header ("Menu")]
    public LevelLoader levelloader;
    public GameObject pausemenu;
    public GameObject menubackground;
    public GameObject player;
    public AudioSource menumusic;
    public AudioMixer audioMixer;
    public TMPro.TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    void Start()
    {
        levelloader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
        
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // Update is called once per frame
    public static bool gameIsPaused;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(SceneManager.GetActiveScene().buildIndex == 1)
            {
                gameIsPaused = gameIsPaused;
            }
            else
            {
                gameIsPaused = !gameIsPaused;
                PauseGame();
            }
            //gameIsPaused = !gameIsPaused;
            //PauseGame();
        }
    }

#region Login

    public void SwitchToSignUpTab() 
    {
        signUpTab.SetActive(true);
        logInTab.SetActive(false);
        resetTab.SetActive(false);
        errorSignUp.text = "";
        errorLogin.text = "";
    }

    public void SwitchToLoginTab()
    {
        signUpTab.SetActive(false);
        logInTab.SetActive(true);
        resetTab.SetActive(false);
        errorSignUp.text = "";
        errorLogin.text = "";
    }

    public void SwitchToResetTab()
    {
        signUpTab.SetActive(false);
        logInTab.SetActive(false);
        resetTab.SetActive(true);
        errorSignUp.text = "";
        errorLogin.text = "";
    }

    string Encrypt(string pass)
    {
        System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] bs = utf8.GetBytes(pass);
        bs = x.ComputeHash(bs);
        System.Text.StringBuilder s = new System.Text.StringBuilder();
        foreach(byte b in bs){
            s.Append(b.ToString("x2").ToLower());
        }
        return s.ToString();
    }


    //Register/Login/ResetPassword
    public void SignUp()
    {
        if (userPassword.text.Length < 6) 
        {
            errorSignUp.text = "Password too short!";
            return;
        }
        var registerRequest = new RegisterPlayFabUserRequest{
            Email = userEmail.text,
            Password = Encrypt(userPassword.text),
            Username = username.text,
            DisplayName = username.text};
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, RegisterSuccess, RegisterError);        
    }

    public void RegisterSuccess(RegisterPlayFabUserResult result) 
    {
        //PlayerPrefs.SetString("EMAIL", userEmailLogin.text);
        //PlayerPrefs.SetString("PASSWORD", userPasswordLogin.text);
        errorSignUp.text = "";
        errorLogin.text = "";
        StartGame();
    }

    public void RegisterError(PlayFabError error){
        errorSignUp.text = error.GenerateErrorReport();
    }


    public void LogIn(){
        var request = new LoginWithEmailAddressRequest {
            Email = userEmailLogin.text,
            Password = Encrypt(userPasswordLogin.text),
            
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, LogInSuccess, LogInError);
    }

    public void LogInSuccess(LoginResult result){
        Debug.Log("Successful Login");

        string name = null;
        if(result.InfoResultPayload != null)
        {
           name = result.InfoResultPayload.PlayerProfile.DisplayName; 
        }
        

        usernamebackground.SetActive(true);

        showusername.text = "Welcome " + name; 
        StartCoroutine(showUsername());

        errorSignUp.text = "";
        errorLogin.text = "";
        PlayFabManager.PFM.GetLeaderboard();
        
    }

    public void LogInError(PlayFabError error){
        errorLogin.text = error.GenerateErrorReport();
    }

    IEnumerator showUsername()
    {
        yield return new WaitForSeconds(3);
        StartGame();
    }
    
    
    public void ResetPasswordButton() {
        var request = new SendAccountRecoveryEmailRequest 
        {
            Email = userEmailRecovery.text,
            TitleId = "38CDB",
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, LogInError);
    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result) {
        SwitchToLoginTab();
        errorLogin.text = "Password reset mail sent!";
    }   

    void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("Data sent successfully!");
    }

    public void SaveUsername()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                //"Username", characterEditor.Username
                //"Username", JsonConvert.SerializeObject(characterBoxes[0].ReturnClass())
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, RegisterError);
    }

    #endregion Login

    void StartGame(){
        startPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void PlayGame ()
    {
        menumusic.Stop();
        levelloader.LoadFirstLevel();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        //gameObject.GetComponent<LevelLoader>().LoadFirstLevel();

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void PauseGame()
    {
        if(gameIsPaused)
        {
            Time.timeScale = 0f;
            menubackground.SetActive(true);
            pausemenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1;
            menubackground.SetActive(false);
            pausemenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        menubackground.SetActive(false);
        pausemenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    } 

    public void setVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void QuitGame ()
    {
        Debug.Log("Quit");
        SceneManager.LoadScene(1);
        //Application.Quit();
    }
}
