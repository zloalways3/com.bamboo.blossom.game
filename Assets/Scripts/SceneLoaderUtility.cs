using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderUtility : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LaunchMainScene(UniversalConstants.MainStageDescription));
    }

    private IEnumerator LaunchMainScene(string nameScene)
    {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(nameScene);

        loadingOperation.allowSceneActivation = false;

        while (!loadingOperation.isDone)
        {
            if (loadingOperation.progress >= 0.9f)
            {
                loadingOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}