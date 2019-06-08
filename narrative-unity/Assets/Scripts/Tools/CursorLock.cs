using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLock : MonoBehaviour
{
    public static bool isLocked = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SetCursorLock(true);
    }

    public static void SetCursorLock(bool isLocked = true)
    {
        Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = (Cursor.lockState == CursorLockMode.None);
    }
}
