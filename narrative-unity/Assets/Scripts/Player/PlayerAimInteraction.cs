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

    [SerializeField]
    [Tooltip("The radius in which an object can be focused by being near it.")]
    static float maxPlayerRadius = 1.4f;

    static Camera thirdPersonCamera;
    static Player player;

    private void Start()
    {
        StartCoroutine(Coroutine_WaitForGameManager());
    }

    IEnumerator Coroutine_WaitForGameManager()
    {
        while (!player)
        {
            yield return null;
            player = GameManager.GLOBAL.player;
        }

        thirdPersonCamera = player.thirdPersonCamera;
    }

    public static bool IsFocusable(Interactable focus)
    {
        if (GameManager.GLOBAL.inventory.isOpen)
            return false;

        // Prevent player from interacting with characters while in a menu
        if (GameManager.GLOBAL.dialogue.menuInProgress)
            return false;

        float distanceFromPlayer = Vector3.Distance(player.transform.position, focus.transform.position);
        float distanceFromCamera = Vector3.Distance(thirdPersonCamera.transform.position, focus.transform.position);

        if (focus.focusWhenPlayerIsNear)
            if (distanceFromPlayer < maxPlayerRadius)
            {
                Debug.Log($"Player distance: {distanceFromPlayer}");
                return true;
            }

        // object to focus is inside dead zone
        if (distanceFromCamera < minDistance)
        return false;

        // object to focus is too far away
        if (distanceFromCamera > maxDistance)
            return false;

        return true;
    }
}
