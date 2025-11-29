using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class GameSavesHandler
{
    // SAVE DATA

    public static void SavePlayerData(GameObject player, int saveNumber, Vector3 playerLocation)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = StaticData.SavePath + StaticData.PlayerSaveFile + saveNumber + StaticData.SaveExtension;
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData playerData = new PlayerData(player, playerLocation);

        formatter.Serialize(stream, playerData);
        stream.Close();
    }

    public static void SaveGameData(int saveNumber, bool died, int killed)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = StaticData.SavePath + StaticData.GameDataSaveFile + saveNumber + StaticData.SaveExtension;
        FileStream stream = new FileStream(path, FileMode.Create);

        int diffInSeconds = (int) Math.Round((System.DateTime.Now - StaticData.SessionStart).TotalSeconds);
        int deathCount = StaticData.OldDeathCount + (died ? 1 : 0);
        int killedCount = StaticData.OldKilledCount + killed;
        GameData gameData = new GameData(StaticData.OldPlayTime + diffInSeconds, deathCount, killedCount);

        formatter.Serialize(stream, gameData);
        stream.Close();
    }

    private static void SaveSimpleData<T>(int saveNumber, string saveFile, Func<GameObject, T> newData,
        GameObject gameObject) where T : class
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = StaticData.SavePath + saveFile + saveNumber + StaticData.SaveExtension;
        FileStream stream = new FileStream(path, FileMode.Create);

        T data = newData(gameObject);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveQuestsData(int saveNumber, GameObject questsGameObject)
    {
        SaveSimpleData(saveNumber, StaticData.QuestDataSaveFile, go => new QuestsData(go), questsGameObject);
    }

    public static void SaveWallsData(int saveNumber, GameObject wallsGameObject)
    {
        SaveSimpleData(saveNumber, StaticData.WallDataSaveFile, go => new WallsData(go), wallsGameObject);
    }

    public static void SaveItemsData(int saveNumber, GameObject itemsGameObject)
    {
        SaveSimpleData(saveNumber, StaticData.ItemDataSaveFile, go => new ItemsData(go), itemsGameObject);
    }

    public static void SaveInventoryData(int saveNumber, GameObject playerGameObject)
    {
        SaveSimpleData(saveNumber, StaticData.InventoryDataSaveFile, go => new InventoryData(go), playerGameObject);
    }

    // LOAD SAVED DATA

    private static T LoadData<T>(int saveNumber, string saveFileName) where T : class
    {
        string path = StaticData.SavePath + saveFileName + saveNumber + StaticData.SaveExtension;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            T data = formatter.Deserialize(stream) as T;
            stream.Close();
            return data;
        }

        Debug.LogError("Save file not found: " + path);
        return default(T);
    }

    public static PlayerData LoadPlayerData(int saveNumber)
    {
        return LoadData<PlayerData>(saveNumber, StaticData.PlayerSaveFile);
    }

    public static GameData LoadGameData(int saveNumber)
    {
        return LoadData<GameData>(saveNumber, StaticData.GameDataSaveFile);
    }

    public static QuestsData LoadQuestsData(int saveNumber)
    {
        return LoadData<QuestsData>(saveNumber, StaticData.QuestDataSaveFile);
    }

    public static WallsData LoadWallsData(int saveNumber)
    {
        return LoadData<WallsData>(saveNumber, StaticData.WallDataSaveFile);
    }

    public static ItemsData LoadItemsData(int saveNumber)
    {
        return LoadData<ItemsData>(saveNumber, StaticData.ItemDataSaveFile);
    }

    public static InventoryData LoadInventoryData(int saveNumber)
    {
        return LoadData<InventoryData>(saveNumber, StaticData.InventoryDataSaveFile);
    }

    // DELETE SAVED DATA

    private static void DeleteData(int saveNumber, string saveFileName)
    {
        string path = StaticData.SavePath + saveFileName + saveNumber + StaticData.SaveExtension;
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static void DeletePlayerData(int saveNumber)
    {
        DeleteData(saveNumber, StaticData.PlayerSaveFile);
    }

    public static void DeleteGameData(int saveNumber)
    {
        DeleteData(saveNumber, StaticData.GameDataSaveFile);
    }

    public static void DeleteQuestsData(int saveNumber)
    {
        DeleteData(saveNumber, StaticData.QuestDataSaveFile);
    }

    public static void DeleteWallsData(int saveNumber)
    {
        DeleteData(saveNumber, StaticData.WallDataSaveFile);
    }

    public static void DeleteItemsData(int saveNumber)
    {
        DeleteData(saveNumber, StaticData.ItemDataSaveFile);
    }

    public static void DeleteInventoryData(int saveNumber)
    {
        DeleteData(saveNumber, StaticData.InventoryDataSaveFile);
    }

    public static void DeleteAllData(int saveNumber)
    {
        DeletePlayerData(saveNumber);
        DeleteGameData(saveNumber);
        DeleteQuestsData(saveNumber);
        DeleteWallsData(saveNumber);
        DeleteItemsData(saveNumber);
        DeleteInventoryData(saveNumber);
    }
}