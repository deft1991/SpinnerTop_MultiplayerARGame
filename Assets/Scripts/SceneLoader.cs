using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    private string _sceneNameToBeLoaded;

    public void LoadScene(string sceneName)
    {
        _sceneNameToBeLoaded = sceneName;

        StartCoroutine(InitializeSceneLoading());
    }

    IEnumerator InitializeSceneLoading()
    {
        // First, we load the Loading scene
        yield return SceneManager.LoadSceneAsync("Scene_Loading");

        // Load the actual Scene
        StartCoroutine(LoadActuallyScene());
    }

    IEnumerator LoadActuallyScene()
    {
        var asyncSceneLoading = SceneManager.LoadSceneAsync(_sceneNameToBeLoaded);

        // this value stops the scene from displaying when it still downloading... 
        asyncSceneLoading.allowSceneActivation = false;

        while (!asyncSceneLoading.isDone)
        {
            Debug.Log("Scene loading %: " + asyncSceneLoading.progress);

            // if scene downloaded 90+ percents we can display it
            if (asyncSceneLoading.progress >= 0.9f)
            {
                // Finally show the scene
                asyncSceneLoading.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}