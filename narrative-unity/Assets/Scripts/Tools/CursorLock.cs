using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLock : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            TogglePause();
    }

    static void TogglePause()
    {
        GameManager.GLOBAL.isPaused = !GameManager.GLOBAL.isPaused;
        Cursor.lockState = GameManager.GLOBAL.isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = (Cursor.lockState == CursorLockMode.None);
    }
}
