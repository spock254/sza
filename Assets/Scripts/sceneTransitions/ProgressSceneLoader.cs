using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProgressSceneLoader : MonoBehaviour
{
    [SerializeField]
    Text progressText = null;

    [SerializeField]
    GameObject loadingScreen = null;
    
    AsyncOperation operation;

    public void LoadScene(int sceneIndex) 
    {
        UpdateProgress(0);
        loadingScreen.SetActive(true);

        StartCoroutine(BeginLoad(sceneIndex));
    }

    IEnumerator BeginLoad(int sceneIndex) 
    {
        operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (operation.isDone == false)
        {
            UpdateProgress(operation.progress);
            yield return null;
        }

        UpdateProgress(operation.progress);
        operation = null;
        loadingScreen.SetActive(false);
    }

    void UpdateProgress(float progress) 
    {
        progressText.text = (int)(progress * 100f) + "%";
    }
}
