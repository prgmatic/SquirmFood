using UnityEngine;
using System.IO;

public static class SaveData
{

    private const byte SAVE_VERSION = 0;

    public static bool[] LevelsCompleted;
    public static byte MusicVolume;
    public static byte SFXVolume;

    public static int CurrentLevel
    {
        get { return _currentLevel; }
        set
        {
            if (value >= LevelsCompleted.Length)
            {
                _currentLevel = -1;
            }
            else _currentLevel = value;
        }
    }
    public static string SAVE_PATH
    {
        get { return Application.persistentDataPath + Path.DirectorySeparatorChar + "player.sav"; }
    }

    private static int _currentLevel;

    static SaveData()
    {
        Init();
    }
    public static void Save()
    {
        using (FileStream fileStream = new FileStream(SAVE_PATH, FileMode.Create))
        {
            using (BinaryWriter writer = new BinaryWriter(fileStream))
            {
                writer.Write(SAVE_VERSION);
                writer.Write(MusicVolume);
                writer.Write(SFXVolume);
                writer.Write(CurrentLevel);
                writer.Write(LevelsCompleted.Length);
                foreach(var levelComplete in LevelsCompleted)
                {
                    writer.Write(levelComplete);
                }
                writer.Close();
            }
        }
        Debug.Log(string.Format("Data saved at {0}", SAVE_PATH));
    }

    public static void Load()
    {
        if(!File.Exists(SAVE_PATH))
        {
            Debug.Log("No save file exists");
            return;
        }
        try
        {
            using (FileStream fileStream = new FileStream(SAVE_PATH, FileMode.Open))
            {
                using (BinaryReader reader = new BinaryReader(fileStream))
                {
                    // Read the save version
                    reader.ReadByte();
                    // Read Volume Levels
                    MusicVolume = reader.ReadByte();
                    SFXVolume = reader.ReadByte();
                    // Read the current level
                    CurrentLevel = reader.ReadInt32();
                    // Load LevelsCompleted Data
                    var arySize = reader.ReadInt32();
                    for (int i = 0; i < LevelsCompleted.Length || i < arySize; i++)
                    {
                        LevelsCompleted[i] = reader.ReadBoolean();
                    }
                    reader.Close();
                }
            }
            GameManager.Instance.SetSliderValues();
        }
        catch
        {
            Delete();
        }
    }

    public static void Delete()
    {
        if (File.Exists(SAVE_PATH))
        {
            File.Delete(SAVE_PATH);
            Debug.Log("Save Data Deleted.");
        }
        else
        {
            Debug.Log("No Save Data to Delete.");
        }
        Init();
    }

    private static void Init()
    {
        LevelsCompleted = new bool[GameManager.Instance.LevelSet.Levels.Count];
        CurrentLevel = -1;
        SFXVolume = 5;
        MusicVolume = 5;
    }
}
