using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;
    Player player;

    private void Start()
    {
        StartCoroutine(Coroutine_WaitForGameManager());
    }

    IEnumerator Coroutine_WaitForGameManager()
    {
        while (!GameManager.GLOBAL)
            yield return null;

        Spawn();
    }

    public void Spawn()
    {
        if (player)
            return;

        player = Instantiate(playerPrefab).GetComponent<Player>();
        GameManager.GLOBAL.player = player;

        player.teleportPlayer.TeleportIntoLevelFadeSkip(GameManager.GLOBAL.sceneLoader.currentLevel);
    }

    public void Kill()
    {
        if (!player)
            player = FindObjectOfType<Player>();

        if (!player)
            return;

        Destroy(player.gameObject);
    }
}
