using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class FileManager
{
    private const string PERSISTENT_PROGRESS_DATA_PATH = "/appProgress.bin";

    public static GameProgress loadProgress()
    {
        GameProgress gameProgress = null;
        if (File.Exists(Application.persistentDataPath + PERSISTENT_PROGRESS_DATA_PATH))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + PERSISTENT_PROGRESS_DATA_PATH, FileMode.Open);
            gameProgress = (GameProgress)formatter.Deserialize(file);
            file.Close();
            return gameProgress;
        }
        else
        {
            Debug.LogWarning("No se encuentra el archivo de progresso de la App");
            return gameProgress;
        }
    }

    public static void saveProgress(GameProgress _gameProgress)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + PERSISTENT_PROGRESS_DATA_PATH);
        formatter.Serialize(file, _gameProgress);
        file.Close();
    }

#if UNITY_IOS
    public static void setNoBackUpIOS() {
        UnityEngine.iOS.Device.SetNoBackupFlag(Application.persistentDataPath + PERSISTENT_PROGRESS_DATA_PATH);
    }
#endif
}

