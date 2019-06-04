using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimInteraction : MonoBehaviour
{
    static float minDistance = 2.5f;
    static float maxDistance = 6f;

    static Camera thirdPersonCamera;

    private void Start()
    {
        thirdPersonCamera = GetComponentInChildren<Camera>();
    }

    public static bool IsFocusable(Interactable focus)
    {
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
