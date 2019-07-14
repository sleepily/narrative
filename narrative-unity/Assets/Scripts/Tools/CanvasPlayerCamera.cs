using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasPlayerCamera : MonoBehaviour
{
    Canvas canvas;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        StartCoroutine(Coroutine_WaitForGameManager());
    }

    IEnumerator Coroutine_WaitForGameManager()
    {
        while (!GameManager.GLOBAL?.player?.thirdPersonCamera)
            yield return null;

        canvas.worldCamera = GameManager.GLOBAL.player.thirdPersonCamera;
    }
}
