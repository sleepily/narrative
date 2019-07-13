using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public enum SceneIndices
    {
        Menu,
        Setup,
        Stairs1,
        Library,
        Stairs2,
        Greenhouse,
        Stairs3,
        Void,
        Stairs4,
        Ending
    }

    [Header("Playtesting")]
    public SceneIndices currentLevelIndex = SceneIndices.Stairs1;

    public List<TeleportLocation> levelTeleportLocations;

    public bool hasFinishedLoading { get; private set; }

    public bool hasFinishedUnloading { get; private set; }

    private void Start() => ReloadScene();

    int SetCurrentLevel(int index = 1)
    {
        currentLevelIndex = (SceneIndices)index;
        SaveManager.Global.sceneIndex = index;
        return index;
    }

    public bool LoadScene(int sceneBuildIndex = 0, LoadSceneMode loadSceneMode = LoadSceneMode.Single, bool teleportPlayer = true)
    {
        if (sceneBuildIndex <= 0)
            sceneBuildIndex = SetCurrentLevel();

        // Debug.Log("Loading scene " + sceneBuildIndex + " with mode " + loadSceneMode.ToString());

        StartCoroutine(Coroutine_LoadScene(sceneBuildIndex, loadSceneMode, teleportPlayer));

        SetCurrentLevel(sceneBuildIndex);

        return true;
    }

    public bool LoadLevel(int sceneBuildIndex = 0, bool teleportPlayer = true)
    {
        int sceneToUnload = SceneManager.GetActiveScene().buildIndex;

        if (sceneBuildIndex <= 0)
            sceneBuildIndex = SetCurrentLevel();

        StartCoroutine(Coroutine_LoadUnloadScenes(sceneToUnload, sceneBuildIndex));

        return true;
    }

    public void ReloadScene() => LoadScene((int)currentLevelIndex);

    public void UnloadScene(int sceneBuildIndex = 1) => StartCoroutine(Coroutine_UnloadScene(sceneBuildIndex));

    public int GetSceneBuildIndex(string sceneName) => SceneManager.GetSceneByName(sceneName).buildIndex;

    public bool HasFinishedLoading() => hasFinishedLoading;

    IEnumerator Coroutine_LoadScene(int sceneBuildIndex, LoadSceneMode loadSceneMode, bool teleportPlayer = true)
    {
        if (!hasFinishedLoading)
            yield return null;

        hasFinishedLoading = false;
        GameManager.GLOBAL.player.LockMovement();

        yield return SceneManager.LoadSceneAsync(sceneBuildIndex, loadSceneMode);

        hasFinishedLoading = true;
        GameManager.GLOBAL.player.UnlockMovement();

        if (teleportPlayer)
            GameManager.GLOBAL.player.teleportPlayer.Teleport(levelTeleportLocations[sceneBuildIndex - 1]);
    }

    IEnumerator Coroutine_UnloadScene(int sceneBuildIndex)
    {
        if (!hasFinishedUnloading)
            yield return null;

        hasFinishedUnloading = false;
        GameManager.GLOBAL.player.LockMovement();

        yield return SceneManager.UnloadSceneAsync(sceneBuildIndex);

        hasFinishedLoading = true;
        GameManager.GLOBAL.player.UnlockMovement();
    }

    IEnumerator Coroutine_LoadUnloadScenes(int sceneToUnload, int newSceneIndex)
    {
        StartCoroutine(Coroutine_LoadScene(newSceneIndex, LoadSceneMode.Additive, teleportPlayer: true));

        while (!hasFinishedLoading)
            yield return null;

        SetCurrentLevel(newSceneIndex);

        StartCoroutine(Coroutine_UnloadScene(sceneToUnload));
    }
}
