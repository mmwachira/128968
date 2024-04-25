using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text;

public class GameController : MonoBehaviour
{
    public static GameController GC;
    public LevelLoader levelloader;
    
    public ObjectivesManager objectivesmanager;
    public GameObject Objective;
    public TMP_Text Objectivetext;
    public TMP_Text itemsCollected;
    public Light trashLight;
    public Animator anim;
    public int items = 0;
    public int level = 0;
    public int score = 0;
    public Text levelDisplay;
    public Text itemscollected;
    public Text playerscore;
    public AudioSource paperdropsound;
    public AudioSource objectivecompletesound;
    public AudioSource backgroundmusic;
    public GameObject EndCredits;
    


    #region Player Data
    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerDetails details = SaveSystem.LoadPlayer();

        items = details.items;
        level = details.level;
        score = details.score;

        itemsCollected.text = "Items Collected: " + items + "/5";
        //Instantiate (other.gameObject, spawnPoint, Quaternion.identity);

        Vector3 position;
        position.x = details.position[0];
        position.y = details.position[1];
        position.z = details.position[2];
        transform.position = position;
    }

    #endregion

    public void getLeaderboardPlayFab()
    {
        PlayFabManager.PFM.GetLeaderboard();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Trash"))
        {
            paperdropsound.Play();
            items++;
            score+=10;
            level = SceneManager.GetActiveScene().buildIndex;
            itemsCollected.text = "Items Collected: "+ items + "/5";
            Destroy(other.gameObject);
            PlayFabManager.PFM.SendLeaderboard(SceneManager.GetActiveScene().buildIndex, score, items);
            if(items >= 5)
            {
                objectivecompletesound.Play();
                anim.SetBool("isComplete", true);
                StartCoroutine(objectivesmanager.objComplete());

                if(SceneManager.GetActiveScene().buildIndex == 4)
                {
                    Invoke("gameComplete", 6);
                }
                else
                {
                    levelloader.Invoke("LoadNextLevel", 5);
                }
                
            }
            
        }

    }

    void OnGUI()
    {
        //GUI.Label(new Rect(1, 1, 200, 40), "Items Collected: " + items + "/5");

    }

    void gameComplete()   
    {
        backgroundmusic.Stop();
        EndCredits.SetActive(true);
        levelloader.Invoke("LoadMainMenu", 20);
    }
}
