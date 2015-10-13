using UnityEngine;
using UnityEditor;
using System.IO;

public static class SaveDataUtils
{
    [MenuItem("Worm Food/Delete Save Data")]
    private static void DeleteSaveData()
    {
        SaveData.Delete();
    }
}
