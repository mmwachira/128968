using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{    
    public LevelLoader levelloader;
    public GameObject pausemenu;
    public GameObject menubackground;
    public GameObject player;
    public AudioSource menumusic;

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


    public void PlayGame ()
    {
        menumusic.Stop();
        levelloader.LoadFirstLevel();
        
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
           // player.SetActive(false);
        }
        else
        {
            Time.timeScale = 1;
            menubackground.SetActive(false);
            pausemenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
           // player.SetActive(true);
        }
        
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        menubackground.SetActive(false);
        pausemenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
       // player.SetActive(true);
    }


    public void QuitGame ()
    {
        Debug.Log("Quit");
        SceneManager.LoadScene(1);
        //Application.Quit();
    }
}
