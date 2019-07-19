using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuManager : MonoBehaviour
{
    public GameObject gameManagerPrefab;

    bool showTitle = true;

    public UnityEvent hideTitle;

    [Tooltip("Events that execute when the last save was in level 2.")]
    public UnityEvent level2Save;

    [Tooltip("Same as above for level 3.")]
    public UnityEvent level3Save;

    SceneLoader sceneLoader;
    FadeManager fadeManager;

    private void Start()
    {
        sceneLoader = GetComponentInChildren<SceneLoader>();
        fadeManager = GetComponentInChildren<FadeManager>();

        fadeManager.FadeFromBlack(2.5f);

        CheckForSave();
    }

    void CheckForSave()
    {
        Debug.Log("Checking for save...");
        SaveManager.SavePoint save = SaveManager.Global.LoadSavePoint();

        if (save.IsEmpty())
        {
            Debug.Log("No save found.");
            return;
        }

        if (save.sceneIndex >= (int)SceneLoader.SceneIndices.Stairs2)
        {
            Debug.Log("Found Save Level 2");
            level2Save.Invoke();
        }

        if (save.sceneIndex >= (int)SceneLoader.SceneIndices.Stairs3)
        {
            Debug.Log("Found Save Level 3");
            level3Save.Invoke();
        }
    }

    // TODO: replacew this and move to actual image gameobjects
    private void Update()
    {
        bool spaceEnter = Input.GetAxisRaw("Submit") > float.Epsilon;
        bool mouse = Input.GetMouseButtonDown(0);

        if (spaceEnter || mouse)
            HideTitle();
    }

    void HideTitle()
    {
        if (!showTitle)
            return;

        showTitle = false;

        hideTitle.Invoke();
    }

    public void LoadStairs(TeleportLocation destination)
    {
        StartCoroutine(Coroutine_LoadStairs(destination));
    }

    IEnumerator Coroutine_LoadStairs(TeleportLocation destination)
    {
        Debug.Log($"Loading {destination.levelIndex.ToString()}");

        if (destination.levelIndex == SceneLoader.SceneIndices.Stairs1)
            SaveManager.Global.ResetSave();

        fadeManager.FadeToWhite();

        yield return new WaitForSeconds(1f);

        GameManager gm = Instantiate(gameManagerPrefab).GetComponent<GameManager>();
        gm.sceneLoader.SetCurrentLevel(destination);
    }
}
