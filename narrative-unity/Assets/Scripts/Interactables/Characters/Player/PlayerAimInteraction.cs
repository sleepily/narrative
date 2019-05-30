using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimInteraction : MonoBehaviour
{
    public float rayForwardOffset = 3f;
    public float maxDistance = 6f;

    public Camera thirdPersonCamera;

    private void Update()
    {
        Raycast();
    }

    void Raycast()
    {
        Ray cameraRay = new Ray();
        cameraRay.origin = thirdPersonCamera.transform.position + (thirdPersonCamera.transform.forward * rayForwardOffset);
        cameraRay.direction = thirdPersonCamera.transform.forward;

        RaycastHit hitInfo;

        if (!Physics.Raycast(cameraRay, out hitInfo, maxDistance))
            return;

        GameObject collider = hitInfo.collider.gameObject;

        if (collider.GetComponent<Interactable>())
        {
            string action = "focus";

            if (Input.GetAxisRaw("Use") > float.Epsilon)
                action = "use";
            
            if (Input.GetAxisRaw("Search") > float.Epsilon)
                action = "search";

            EventManager.GlobalManager.TriggerEvent(collider.tag + "_" + collider.name, action);
        }
    }
}
