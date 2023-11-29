using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text;

public class GameController : MonoBehaviour
{
    public LevelLoader levelloader;
    
    public ObjectivesManager objectivesmanager;
    public GameObject Objective;
    public TMP_Text Objectivetext;
    public Light trashLight;
    public Animator anim;
    public int items = 0;
    public AudioSource paperdropsound;
    public AudioSource objectivecompletesound;
    public AudioSource backgroundmusic;
    public GameObject EndCredits;
    

    
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Trash"))
        {
            paperdropsound.Play();
            items++;
            Destroy(other.gameObject);
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
        GUI.Label(new Rect(1, 1, 200, 40), "Items Collected: " + items + "/5");

    }

    void gameComplete()   
    {
        backgroundmusic.Stop();
        EndCredits.SetActive(true);
        levelloader.Invoke("LoadMainMenu", 20);
    }
}
