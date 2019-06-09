using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class GameManager : MonoBehaviour
{
    public static GameManager GLOBAL;

    public bool isPaused = false;
    
    [Header("Managers")]
    public InventoryManager inventoryManager;
    public SceneLoader sceneLoader;

    [Header("Global References")]
    public Camera thirdPersonCamera;
    public SayDialog interactableSayDialog, characterSayDialog;

    private void Awake()
    {
        GLOBAL = this;
    }
}
