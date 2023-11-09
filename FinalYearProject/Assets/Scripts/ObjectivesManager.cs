using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ObjectivesManager : MonoBehaviour
{
    public GameObject objectiveDisplay;
    public GameObject objectiveTrigger;
    public GameObject objectiveText;


    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(levelObj());
        }
    }

    public IEnumerator levelObj()
    {
        objectiveDisplay.SetActive(true);
        objectiveText.SetActive(true);
        objectiveDisplay.GetComponent<Animation>().Play("ObjectiveDisplayAnim");
        if(SceneManager.GetActiveScene().buildIndex == 3)
        {
            objectiveText.GetComponent<TMP_Text>().text = "Objective: Place all the dirty plates in the sink!";
        }
        else if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            objectiveText.GetComponent<TMP_Text>().text = "Objective: Collect all the trash in the apartment!";
        }
        
        yield return new WaitForSeconds(5.3f);
        objectiveText.GetComponent<TMP_Text>().text = "";
        objectiveTrigger.SetActive(false);
        objectiveDisplay.SetActive(false);
    }

    public IEnumerator objComplete()
    {
        objectiveDisplay.SetActive(true);
        objectiveText.SetActive(true);
        objectiveDisplay.GetComponent<Animation>().Play("ObjectiveDisplayAnim");
        objectiveText.GetComponent<TMP_Text>().text = "Objective: Objective Complete! Proceeding to next level.";
        yield return new WaitForSeconds(5.3f);
        objectiveText.GetComponent<TMP_Text>().text = "";
        objectiveTrigger.SetActive(false);
        objectiveDisplay.SetActive(false);
    }
}
//     public Objective[] objectives;

//     // Start is called before the first frame update
//     void Awake()
//     {
//         objectives = GetComponents<Objective>();
//     }

//     void OnGUI()
//     {
//         foreach (var objective in objectives)
//         {
//             objectives.DrawHUD();
//         }
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         foreach (var objective in objectives)
//         {
//             if (objective.IsAchieved())
//             {
//                 objective.Complete();
//                 Destroy(objective);
//             }
//         }
//     }
// }

// //This is the abstract base class for all objectives:
// public abstract class Objective : MonoBehaviour 
// {
//     public abstract bool IsAchieved();
//     public abstract void Complete();
//     public abstract void DrawHUD();
// }

// // Add this to ObjectivesManager to run a "collect items" goal:
// public class CollectItems : Objective {
 
//     public int items = 0;
//     public int requiredItems = 5;
 
//     public override bool IsComplete() {
//         return (items >= requiredItems);
//     }
 
//     public override void Complete() {
//         ScoreSingleton.score += 10;
//     }
 
//     public override void DrawHUD() {
//         GUILayout.Label(string.Format("Collected {0}/{1} items", items, requiredItems));
//     }
 
//     public OnTriggerEnter(Collider other) {
//         if (string.Equals(other.tag, "Trash")) {
//             items++;
//             Destroy(other.gameObject);
//         }
//     }
// }
