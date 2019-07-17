using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class KarmaManager : MonoBehaviour
{
    int levelIndex = 0;
    public Flowchart flowchart;

    public void SetLevel(int level)
    {
        this.levelIndex = level;
    }

    public void SaveKarma(int karma)
    {
        Debug.Log($"Setting {karma} karma for level {levelIndex}.");
        SaveManager.Global.karma[levelIndex] = karma;
    }

    public void LoadKarma()
    {
        int karma = SaveManager.Global.karma[levelIndex];

        if (flowchart)
            if (flowchart.HasVariable("karma"))
                flowchart.SetIntegerVariable("karma", karma);
    }
}
