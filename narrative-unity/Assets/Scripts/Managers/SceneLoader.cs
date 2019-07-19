using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public enum SceneIndices
    {
        Menu,
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

    public TeleportLocation currentLevel;

    public bool hasFinishedLoading { get; private set; } = true;

    public bool hasFinishedUnloading { get; private set; } = true;

    public bool hasFinishedLevelLoading { get; private set; } = true;

    // private void Start() => ReloadScene();

    public int SetCurrentLevel(int index = 1)
    {
        currentLevelIndex = (SceneIndices)index;
        SaveManager.Global.sceneIndex = index;
        return index;
    }
    public int SetCurrentLevel(TeleportLocation currentLevel)
    {
        this.currentLevel = currentLevel;
        return SetCurrentLevel((int)currentLevel.levelIndex);
    }

    public bool LoadScene(int sceneBuildIndex = 0, LoadSceneMode loadSceneMode = LoadSceneMode.Single, bool teleportPlayer = true)
    {
        if (sceneBuildIndex <= 0)
            sceneBuildIndex = SetCurrentLevel();

        StartCoroutine(Coroutine_LoadScene(sceneBuildIndex, loadSceneMode, teleportPlayer));

        SetCurrentLevel(sceneBuildIndex);

        return true;
    }

    public void LoadLevel(int sceneBuildIndex)
    {
        if (!hasFinishedLevelLoading)
        {
            Debug.Log($"Level loading in progress. Cancelling...");
            return;
        }

        hasFinishedLevelLoading = false;

        int sceneToUnload = SceneManager.GetActiveScene().buildIndex;

        if (sceneBuildIndex <= 0)
            sceneBuildIndex = SetCurrentLevel();

        StartCoroutine(Coroutine_LoadUnloadScenes(sceneToUnload, sceneBuildIndex));
    }

    public void LoadLevel(TeleportLocation level)
    {
        int sceneToUnload = SceneManager.GetActiveScene().buildIndex;

        currentLevel = level;

        SetCurrentLevel((int)currentLevel.levelIndex);

        StartCoroutine(Coroutine_LoadUnloadScenes(sceneToUnload, (int)currentLevel.levelIndex));
    }

    public void ReloadScene() => LoadScene((int)currentLevelIndex);

    public void UnloadScene(int sceneBuildIndex = 1) => StartCoroutine(Coroutine_UnloadScene(sceneBuildIndex));

    public int GetSceneBuildIndex(string sceneName) => SceneManager.GetSceneByName(sceneName).buildIndex;

    public bool HasFinishedLoading() => hasFinishedLoading;

    IEnumerator Coroutine_LoadScene(int sceneBuildIndex, LoadSceneMode loadSceneMode, bool teleportPlayer = true)
    {
        if (!hasFinishedLoading)
        {
            // Debug.Log($"Previous scene has not been loaded yet. Cannot load scene {sceneBuildIndex}");
            yield break;
        }

        hasFinishedLoading = false;

        if (GameManager.GLOBAL.player)
            GameManager.GLOBAL.player.LockMovement();

        // Debug.Log($"Attempting to load scene {sceneBuildIndex}:{loadSceneMode.ToString()}");

        yield return SceneManager.LoadSceneAsync(sceneBuildIndex, loadSceneMode);

        hasFinishedLoading = true;

        if (GameManager.GLOBAL.player && teleportPlayer)
        {
            GameManager.GLOBAL.player.teleportPlayer.Teleport(levelTeleportLocations[sceneBuildIndex]);
            GameManager.GLOBAL.player.UnlockMovement();
        }
    }

    IEnumerator Coroutine_UnloadScene(int sceneBuildIndex)
    {
        if (!hasFinishedUnloading)
        {
            // Debug.Log($"Previous scene has not been unloaded yet. Cannot unload scene {sceneBuildIndex}");
            yield break;
        }

        hasFinishedUnloading = false;

        if (GameManager.GLOBAL.player)
            GameManager.GLOBAL.player.LockMovement();

        // Debug.Log($"Attempting to unload scene {sceneBuildIndex}");

        yield return SceneManager.UnloadSceneAsync(sceneBuildIndex);

        hasFinishedUnloading = true;

        if (GameManager.GLOBAL.player)
            GameManager.GLOBAL.player.UnlockMovement();
    }

    IEnumerator Coroutine_LoadUnloadScenes(int sceneToUnload, int newSceneIndex)
    {
        if (!GameManager.GLOBAL)
        {
            StartCoroutine(Coroutine_LoadScene(1, LoadSceneMode.Single));

            while (!hasFinishedLoading)
                yield return null;
        }

        StartCoroutine(Coroutine_LoadScene(newSceneIndex, LoadSceneMode.Additive, teleportPlayer: true));

        while (!hasFinishedLoading)
            yield return null;

        yield return new WaitForSeconds(.1f);

        SetCurrentLevel(newSceneIndex);

        StartCoroutine(Coroutine_UnloadScene(sceneToUnload));

        while (!hasFinishedUnloading)
            yield return null;

        hasFinishedLevelLoading = true;

        yield return new WaitForSeconds(.1f);

        SaveManager.Global.CreateSavePoint();

        // GameManager.GLOBAL.player.teleportPlayer.Teleport(currentLevel);
    }
}
