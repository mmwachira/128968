using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public LevelLoader levelloader;
    //public Animator transition;
    //public float transitionTime = 1f;
    public GameObject loadingScreen;
    public Slider _loadingBar;
    //[SerializeField] 
    //Image _loadingBar;

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetMouseButtonDown(0))
        //{
        //    LoadNextLevel();
       // }
    }


    void OnCollisionEnter(Collision collision)
        {
            LoadNextLevel();
        }

    public void LoadFirstLevel()
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public IEnumerator LoadLevel(int levelIndex)
    {
        //transition.SetTrigger("Start");

        // load scene new
        AsyncOperation loadLevel = SceneManager.LoadSceneAsync(levelIndex);

        loadingScreen.SetActive(true);

        //wait
        while (!loadLevel.isDone)
        {
            float progress = Mathf.Clamp01(loadLevel.progress / .9f);

            _loadingBar.value = progress;
            
            yield return null;
        }

        //yield return new WaitForSeconds(transitionTime);

        //load scene
        //SceneManager.LoadScene(levelIndex);
    }
}
