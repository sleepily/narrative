using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager
{
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

            Debug.Log($"Created SavePoint: {position}, Scene {sceneIndex}, Karma {karma[0] + ":" + karma[1] + ":" + karma[2]}.");
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
        if (!GameManager.GLOBAL?.player)
        {
            Debug.Log($"Player doesn't exist.");
            return;
        }

        SavePoint save = new SavePoint(GameManager.GLOBAL.sceneLoader.currentLevel.location, sceneIndex, karma);

        string decrypted = JsonUtility.ToJson(save);
        string encrypted = EncryptDecrypt(decrypted, encryptionKey);

        PlayerPrefs.SetString("save", encrypted);
        PlayerPrefs.Save();
    }

    public void CreateDummySavePoint()
    {
        Debug.Log($"Starting dummy save...");

        SavePoint save = new SavePoint(Vector3.zero, 5, karma);

        string decrypted = JsonUtility.ToJson(save);
        string encrypted = EncryptDecrypt(decrypted, encryptionKey);

        PlayerPrefs.SetString("save", encrypted);
        PlayerPrefs.Save();
    }

    public void ResetSave()
    {
        Debug.Log($"Resetting save...");

        SavePoint save = new SavePoint(Vector3.zero, 0, karma = new int[] { 0, 0, 0 });

        string decrypted = JsonUtility.ToJson(save);
        string encrypted = EncryptDecrypt(decrypted, encryptionKey);

        PlayerPrefs.SetString("save", encrypted);
        PlayerPrefs.Save();
    }

    public SavePoint LoadSavePoint()
    {
        string encrypted = PlayerPrefs.GetString("save");
        string decrypted = EncryptDecrypt(encrypted, encryptionKey);
        SavePoint loaded = JsonUtility.FromJson<SavePoint>(decrypted);

        return LoadSavePoint(loaded);
    }

    public SavePoint LoadSavePoint(SavePoint savePoint)
    {
        position = savePoint.position;
        sceneIndex = savePoint.sceneIndex;
        karma = savePoint.karma;

        SceneLoader.SceneIndices index = (SceneLoader.SceneIndices)sceneIndex;

        Debug.Log($"Loaded P {position.ToString()}, S {index.ToString()}, K {karma[0]}:{karma[1]}:{karma[2]}.");

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
