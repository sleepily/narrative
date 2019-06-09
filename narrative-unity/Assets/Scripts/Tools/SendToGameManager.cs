using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SendToGameManager : MonoBehaviour
{
    private void Start()
    {
        GameManager.GLOBAL.player.thirdPersonCamera = GetComponent<Camera>();
    }
}
