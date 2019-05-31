using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechBubble : MonoBehaviour
{
    public TextMeshProUGUI dialogueName;
    public TextMeshProUGUI dialoguePhrase;

    private void Update()
    {
        GetInput();
        RotateTowardsPlayer();
    }

    void RotateTowardsPlayer()
    {
        transform.forward = GameManager.GLOBAL.thirdPersonCamera.transform.forward;
    }

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.GLOBAL.dialogueManager.DisplayNextPhrase();
        }
    }
}
