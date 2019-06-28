using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class GameManager : MonoBehaviour
{
    public static GameManager GLOBAL;

    public bool isPaused = false;
    
    [Header("Managers")]
    public InventoryManager inventory;
    public SceneLoader sceneLoader;
    public DialogueManager dialogue;

    [Header("Global References")]
    public SayDialog interactableSayDialog;
    public SayDialog characterSayDialog;

    [HideInInspector]
    public Player player;

    private void Awake()
    {
        GLOBAL = this;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
}
