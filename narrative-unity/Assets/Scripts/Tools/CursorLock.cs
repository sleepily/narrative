using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLock : MonoBehaviour
{
    public bool paused = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            TogglePause();
    }

    void TogglePause()
    {
        paused = !paused;
        Time.timeScale = paused ? 0 : GameManager.timeScale;
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = (Cursor.lockState == CursorLockMode.None);
    }
}
