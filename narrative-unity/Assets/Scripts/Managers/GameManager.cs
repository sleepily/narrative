using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GLOBAL;

    public bool isPaused = false;
    
    [Header("Managers")]
    public InventoryManager inventoryManager;
    public SceneLoader sceneLoader;

    [Header("Global References")]
    public Camera thirdPersonCamera;

    private void Awake()
    {
        GLOBAL = this;
    }
}
