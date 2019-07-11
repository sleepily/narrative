using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Teleport Location", menuName = "Game Mechanics/Teleport Location")]
public class TeleportLocation : ScriptableObject
{
    public SceneLoader.SceneIndices levelIndex;
    public Vector3 location;
    public string title;
}
