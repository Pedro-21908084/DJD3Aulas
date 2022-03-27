using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class Looder
{
    public static void Save(PlayerManagement player, string fileName)
    {
        
        SaveData data = new SaveData(player);

        BinaryFormatter binery = new BinaryFormatter();
        string path = Application.persistentDataPath + fileName;

        FileStream file = new FileStream(path, FileMode.Create);

        binery.Serialize(file, data);
        file.Close();
    }

    public static void Load(PlayerManagement player, string fileName)
    {
        string path = Application.persistentDataPath + fileName;

        
        if (File.Exists(path))
        {
            BinaryFormatter binery = new BinaryFormatter();
            FileStream file = new FileStream(path, FileMode.Open);

            SaveData data = binery.Deserialize(file) as SaveData;
            player.LoadData(data);
        }
        else
        {
            Debug.LogError("Error: File doesn't exist.");
        }
    }

}
