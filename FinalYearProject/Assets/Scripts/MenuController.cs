using System.Text;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{ 
    
    [SerializeField] GameObject signUpTab, logInTab, startPanel, HUD;
    public TMP_InputField username, userEmail, userPassword, userEmailLogin, userPasswordLogin;
    public TMP_Text errorSignUp, errorLogin;
    string encryptedPassword;
    UTF8Encoding utf8 = new UTF8Encoding();

    [Space] 

    public LevelLoader levelloader;
    public GameObject pausemenu;
    public GameObject menubackground;
    public GameObject player;
    public AudioSource bgmusic;

    void Start()
    {
        levelloader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
    }

    // Update is called once per frame
    public static bool gameIsPaused;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
    }

#region Login

    public void SwitchToSignUpTab() 
    {
        signUpTab.SetActive(true);
        logInTab.SetActive(false);
        errorSignUp.text = "";
        errorLogin.text = "";
    }

    public void SwitchToLoginTab()
    {
        signUpTab.SetActive(false);
        logInTab.SetActive(true);
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
        var registerRequest = new RegisterPlayFabUserRequest{
            Email = userEmail.text,
            Password = Encrypt(userPassword.text),
            Username = username.text};
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, RegisterSuccess, RegisterError);        
    }

    public void RegisterSuccess(RegisterPlayFabUserResult result) 
    {
        PlayerPrefs.SetString("EMAIL", userEmailLogin.text);
        PlayerPrefs.SetString("PASSWORD", userPasswordLogin.text);
        errorSignUp.text = "";
        errorLogin.text = "";
        StartGame();
    }

    public void RegisterError(PlayFabError error){
        errorSignUp.text = error.GenerateErrorReport();
    }


    public void LogIn(){
        var request = new LoginWithEmailAddressRequest {Email = userEmailLogin.text, Password = Encrypt(userPasswordLogin.text)};
        PlayFabClientAPI.LoginWithEmailAddress(request, LogInSuccess, LogInError);
    }

    public void LogInSuccess(LoginResult result){
        Debug.Log("Successful Login");

        PlayerPrefs.SetString("EMAIL", userEmailLogin.text);
        PlayerPrefs.SetString("PASSWORD", userPasswordLogin.text);
        errorSignUp.text = "";
        errorLogin.text = "";
        PlayFabManager.PFM.GetLeaderboard();
        StartGame();
    }

    public void LogInError(PlayFabError error){
        errorLogin.text = error.GenerateErrorReport();
    }
    
    
    public void ResetPasswordButton() {
        var request = new SendAccountRecoveryEmailRequest {
            Email = userEmailLogin.text,
            TitleId = "38CDB"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, LogInError);
    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result) {
        errorLogin.text = "Password reset mail sent!";
    }   

    #endregion Login

    void StartGame(){
        startPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void PlayGame ()
    {
        bgmusic.Stop();
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


    public void QuitGame ()
    {
        Debug.Log("Quit");
        SceneManager.LoadScene(1);
        //Application.Quit();
    }
}
