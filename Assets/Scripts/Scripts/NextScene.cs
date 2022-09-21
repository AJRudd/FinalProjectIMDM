using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MinuteCoroutine());
       
    }

    IEnumerator MinuteCoroutine()
    {
        Debug.Log("Started Coroutine at timestamp: " + Time.time);

        yield return new WaitForSecondsRealtime(210);
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCountInBuildSettings > nextSceneIndex)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
