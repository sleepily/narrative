﻿using System.Collections;
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

        if (!portal.isEnabled)
            return;

        TeleportIntoLevel(portal.destination);
    }

    public void Teleport(TeleportLocation destination)
    {
        if (!destination)
            return;

        this.transform.position = destination.location;

        GameManager.GLOBAL.fade.FadeTitleOut(fadeTime: .6f);
    }

    public void TeleportIntoLevel(TeleportLocation destination, bool loseItems = true) =>
        StartCoroutine(Coroutine_TeleportIntoLevel(destination, loseItems, 1f));

    public void TeleportIntoLevelFadeSkip(TeleportLocation destination) =>
        StartCoroutine(Coroutine_TeleportIntoLevel(destination, true, 0f));

    IEnumerator Coroutine_TeleportIntoLevel(TeleportLocation destination, bool loseItems, float fadeInTime)
    {
        float fadeTime = .6f;
        GameManager.GLOBAL.fade.FadeTitleIn(destination.title, fadeInTime, fadeTime);

        yield return new WaitForSeconds(fadeInTime + fadeTime);

        if (loseItems)
            GameManager.GLOBAL.inventory.ClearInventory();

        GameManager.GLOBAL.sceneLoader.LoadLevel(destination);

        while (!GameManager.GLOBAL.sceneLoader.hasFinishedLoading)
            yield return null;

        Teleport(destination);
    }
}
