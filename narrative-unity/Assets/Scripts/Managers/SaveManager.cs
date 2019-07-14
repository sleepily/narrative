using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager
{
    /*
     * Singleton instance to allow global use
     */
    private static SaveManager global;
    public static SaveManager Global
    {
        get
        {
            if (global == null)
                global = new SaveManager();

            return global;
        }
    }

    int encryptionKey = 8573276;

    public Vector3 position;
    public int sceneIndex = -1;
    public int[] karma = { 0, 0, 0 };

    public struct SavePoint
    {
        public Vector3 position;
        public int sceneIndex;
        public int[] karma;

        public SavePoint(Vector3 position, int sceneIndex, int[] karma)
        {
            this.position = position;
            this.sceneIndex = sceneIndex;
            this.karma = karma;
        }

        public bool IsEmpty()
        {
            if (position == null
                || sceneIndex < 0
                || karma == null)
                return true;

            return false;
        }
    }

    public void CreateSavePoint()
    {
        Debug.Log($"Starting save...");

        SavePoint save = new SavePoint(GameManager.GLOBAL.player.transform.position, sceneIndex, karma);

        string decrypted = JsonUtility.ToJson(save);
        string encrypted = EncryptDecrypt(decrypted, encryptionKey);

        PlayerPrefs.SetString("save", encrypted);
        PlayerPrefs.Save();

        Debug.Log($"Saved.");
    }

    public SavePoint LoadSavePoint()
    {
        Debug.Log($"Getting PlayerPrefs...");
        string encrypted = PlayerPrefs.GetString("save");
        Debug.Log($"Encrypting...");
        string decrypted = EncryptDecrypt(encrypted, encryptionKey);
        Debug.Log($"Loading...");
        SavePoint loaded = JsonUtility.FromJson<SavePoint>(decrypted);

        return LoadSavePoint(loaded);
    }

    public SavePoint LoadSavePoint(SavePoint savePoint)
    {
        position = savePoint.position;
        sceneIndex = savePoint.sceneIndex;
        karma = savePoint.karma;

        SceneLoader.SceneIndices index = (SceneLoader.SceneIndices)sceneIndex;

        Debug.Log($"Loaded pos {position.ToString()} in scene {index.ToString()} with karma {karma[0]}:{karma[1]}:{karma[2]}.");

        return savePoint;
    }

    public string EncryptDecrypt(string input, int key)
    {
        string output = "";

        foreach (char letter in input.ToCharArray())
        {
            char coded = (char)(letter ^ key);
            output += coded;
        }

        return output;
    }
}
