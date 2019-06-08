using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GLOBAL;

    public bool isPaused = false;
    public float timeScale = 1f;
    
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
