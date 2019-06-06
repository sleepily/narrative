using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public bool LoadScene(int sceneBuildIndex = 1, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        StartCoroutine(Coroutine_LoadScene(sceneBuildIndex, loadSceneMode));

        return true;
    }

    public bool UnloadScene(int sceneBuildIndex = 1)
    {
        StartCoroutine(Coroutine_UnloadScene(sceneBuildIndex));

        return true;
    }

    public int GetSceneBuildIndex(string sceneName)
    {
        return SceneManager.GetSceneByName(sceneName).buildIndex;
    }

    IEnumerator Coroutine_LoadScene(int sceneBuildIndex, LoadSceneMode loadSceneMode)
    {
        yield return SceneManager.LoadSceneAsync(sceneBuildIndex, loadSceneMode);
    }

    IEnumerator Coroutine_UnloadScene(int sceneBuildIndex)
    {
        yield return SceneManager.UnloadSceneAsync(sceneBuildIndex);
    }
}
