using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        CheckCollision(collision.collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckCollision(other);
    }

    void CheckCollision(Collider other)
    {
        TransportPortal portal = other.GetComponent<TransportPortal>();

        if (!portal)
            return;

        TeleportIntoLevel(portal.destination);
    }

    public void Teleport(TeleportLocation destination)
    {
        if (!destination)
            return;

        // Debug.Log("Teleporting to " + destination.location);

        this.transform.position = destination.location;
    }

    public void TeleportIntoLevel(TeleportLocation destination, bool loseItems = true) =>
        StartCoroutine(Coroutine_TeleportIntoLevel(destination, loseItems));

    IEnumerator Coroutine_TeleportIntoLevel(TeleportLocation destination, bool loseItems)
    {
        GameManager.GLOBAL.fade.FadeToTitle(destination.title, 1f);

        yield return new WaitForSeconds(2f);

        if (loseItems)
            GameManager.GLOBAL.inventory.ClearInventory();

        Teleport(destination);
        GameManager.GLOBAL.sceneLoader.LoadLevel((int)destination.levelIndex);
    }
}
