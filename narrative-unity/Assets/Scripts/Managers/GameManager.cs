using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GLOBAL;

    public static float timeScale = 1f;
    
    public InventoryManager inventoryManager;

    public Camera thirdPersonCamera;

    private void Awake()
    {
        GLOBAL = this;
    }
}
