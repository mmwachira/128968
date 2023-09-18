using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class PlayFabManager : MonoBehaviour
{
    //[Header("UI")]

    [SerializeField] GameObject signUpTab, logInTab, startPanel, HUD;
    public TMP_InputField username, userEmail, userPassword, userEmailLogin, userPasswordLogin;
    public TMP_Text errorSignUp, errorLogin;
    string encryptedPassword;
    UTF8Encoding utf8 = new UTF8Encoding();


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
        errorSignUp.text = "";
        errorLogin.text = "";
        StartGame();
    }

    public void LogInError(PlayFabError error){
        errorLogin.text = error.GenerateErrorReport();
    }

    void StartGame(){
        startPanel.SetActive(false);
        //HUD.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
    
}
