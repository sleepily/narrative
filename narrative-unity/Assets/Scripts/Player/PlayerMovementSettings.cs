using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementSettings", menuName = "Game Mechanics/Player Movement Settings")]
public class PlayerMovementSettings : ScriptableObject
{
    public bool isRunning = false;

    [Range(.3f, 2f)]
    public float mouseSpeed = 1f;

    [Range(1.4f, 2.2f)]
    public float walkingSpeed = 1.8f;

    [Range(2.2f, 3.4f)]
    public float runningSpeed = 2.8f;

    [Range(.02f, .4f)]
    public float rotationSpeed = .02f;
}
