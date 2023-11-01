using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text;

public class GameController : MonoBehaviour
{
    public GameObject Objective;
    public TMP_Text Objectivetext;
    public Light trashLight;
    public Animator anim;
    

    // Start is called before the first frame update
    void Start()
    {
        Objective.SetActive(true);
        Objectivetext.text = "Clean Up The House";
    
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("HideObjective", 3);

        // if(Input.GetKeyDown(KeyCode.Escape))
        // {
        //     Objective.SetActive(false);
        // }
        // else
        // {
        //     Objective.SetActive(true);
        // }
    }

    void HideObjective()
    {
        Objective.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Trash"))
        {
            anim.SetBool("isComplete", true);
        }

    }
}
