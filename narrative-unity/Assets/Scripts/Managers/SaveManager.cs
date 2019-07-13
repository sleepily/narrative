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

    public void LoadSavePoint()
    {
        string encrypted = PlayerPrefs.GetString("save");
        string decrypted = EncryptDecrypt(encrypted, encryptionKey);
        SavePoint loaded = JsonUtility.FromJson<SavePoint>(decrypted);

        LoadSavePoint(loaded);
    }

    public void LoadSavePoint(SavePoint savePoint)
    {
        GameManager.GLOBAL.player.transform.position = savePoint.position;
        sceneIndex = savePoint.sceneIndex;
        karma = savePoint.karma;
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
