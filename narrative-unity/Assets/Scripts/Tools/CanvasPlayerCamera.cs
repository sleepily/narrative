using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasPlayerCamera : MonoBehaviour
{
    Canvas canvas;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = GameManager.GLOBAL.player.thirdPersonCamera;
    }
}
