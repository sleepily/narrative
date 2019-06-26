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

        Debug.Log("Teleporting to " + destination.location);

        this.transform.position = destination.location;
    }

    public void TeleportIntoLevel(TeleportLocation destination, bool loseItems = true)
    {
        if (loseItems)
            GameManager.GLOBAL.inventoryManager.ClearInventory();

        Teleport(destination);
        GameManager.GLOBAL.sceneLoader.LoadScene((int)destination.levelIndex);
    }
}
