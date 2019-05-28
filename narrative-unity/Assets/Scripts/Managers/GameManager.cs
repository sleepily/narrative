using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    /*
     * singleton game manager instance
     */
    private static GameManager game;
    public static GameManager Global
    {
        get
        {
            if (game == null)
                game = new GameManager();

            return game;
        }
    }


}
