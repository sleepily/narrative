using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class DialogueManager : MonoBehaviour
{
    public void WaitForPlayerRead(Flowchart flowchart)
    {
        // Prevent any player movement or interaction
        GameManager.GLOBAL.player.Lock();

        StartCoroutine(CheckPlayerRead(flowchart));
    }

    IEnumerator CheckPlayerRead(Flowchart flowchart)
    {
        // Wait for the player to end all dialogue
        while (flowchart.HasExecutingBlocks())
            yield return null;

        GameManager.GLOBAL.player.Unlock();
    }
}
