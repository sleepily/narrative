using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    bool isGlobal = false;

    [SerializeField] List<Flowchart> flowcharts = new List<Flowchart>();

    private void Awake()
    {
        isGlobal = GetComponentInParent<GameManager>() ?? false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            CreateSavePoint();
    }

    public void CreateSavePoint()
    {
        string[] savePoint = new string[flowcharts.Count + 1];

        string playerSave = "";
        playerSave += $"{GameManager.GLOBAL.player.transform.position}\n";
        playerSave += $"{SceneManager.GetActiveScene().buildIndex}\n";

        savePoint[0] = playerSave;
        Debug.Log(savePoint[0]);

        for (int i = 1; i < savePoint.Length; i++)
        {
            savePoint[i] = JsonUtility.ToJson(flowcharts[i - 1].Variables);

            Debug.Log(savePoint[i]);
        }
    }

    public void LoadSavePoint()
    {

    }

    // Receive Save Data from other SaveManagers
    public void ReceiveSaveData()
    {

    }

    // Send Save Data to other SaveManagers after loading the correct scene
    public void SendSaveData()
    {

    }
}
