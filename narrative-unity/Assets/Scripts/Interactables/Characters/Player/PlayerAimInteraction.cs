using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimInteraction : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The minimum distance for an object to be focusable.")]
    static float minDistance = 1.5f;

    [SerializeField]
    [Tooltip("The maximum distance for an object to still be focusable.")]
    static float maxDistance = 6f;

    static Camera thirdPersonCamera;

    private void Start()
    {
        thirdPersonCamera = GameManager.GLOBAL.player.thirdPersonCamera;
    }

    public static bool IsFocusable(Interactable focus)
    {
        if (GameManager.GLOBAL.inventoryManager.isOpen)
            return false;

        float distanceFromCamera = Vector3.Distance(thirdPersonCamera.transform.position, focus.transform.position);

        // object to focus is inside dead zone
        if (distanceFromCamera < minDistance)
            return false;

        // object to focus is too far away
        if (distanceFromCamera > maxDistance)
            return false;

        return true;
    }
}
