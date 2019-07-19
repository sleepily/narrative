using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoBehaviour
{
    public static GameManager GLOBAL;

    public bool isPaused = false;

    public Player player;

    public InventoryManager inventory { get; private set; }
    public SceneLoader sceneLoader { get; private set; }
    public DialogueManager dialogue { get; private set; }
    public FadeManager fade { get; private set; }
    public PostProcessVolume postProcessVolume { get; private set; }
    public PlayerManager playerManager { get; private set; }

    private void Awake()
    {
        GLOBAL = this;
        // player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        inventory = GetComponentInChildren<InventoryManager>();
        sceneLoader = GetComponentInChildren<SceneLoader>();
        dialogue = GetComponentInChildren<DialogueManager>();
        postProcessVolume = GetComponentInChildren<PostProcessVolume>();
        fade = GetComponentInChildren<FadeManager>();
        playerManager = GetComponentInChildren<PlayerManager>();
    }

    public static void SetCursorLock(bool isLocked) => CursorLock.SetCursorLock(isLocked);

    // Playtesting  
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            CursorLock.SetCursorLock(false);
    }
}
