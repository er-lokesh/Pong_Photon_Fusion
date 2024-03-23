using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    private static float[] xPos = { -7.75f, -7.45f, 7.45f, 7.75f };

    public static Vector3 GetSpawnPoint(int index)
    {
        return new Vector3(xPos[index], 0f, 0f);
    }

    public static int GetMaxPlayers()
    {
        return 4;
    }
}
