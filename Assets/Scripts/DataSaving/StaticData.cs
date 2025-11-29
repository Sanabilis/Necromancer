using System;
using UnityEngine;

public static class StaticData
{
    public static string PlayerSaveFile { get; set; } = "/playerData_";
    public static string GameDataSaveFile { get; set; } = "/gameData_";
    public static string QuestDataSaveFile { get; set; } = "/questData_";
    public static string WallDataSaveFile { get; set; } = "/wallData_";
    public static string ItemDataSaveFile { get; set; } = "/itemData_";
    public static string InventoryDataSaveFile { get; set; } = "/inventoryData_";
    public static string SaveExtension { get; set; } = ".save";

    public static bool Tutorial { get; set; } = false;

    public static InputType InputType { get; set; } = InputType.Unknown;
    public static bool MouseActive { get; set; } = true;

    public static bool LoadGame { get; set; } = false;
    public static int SaveNumber { get; set; } = 1;
    public static string SavePath { get; set; } = Application.persistentDataPath;
    public static DateTime SessionStart { get; set; }
    public static int OldPlayTime { get; set; }
    public static int OldDeathCount { get; set; }
    public static int OldKilledCount { get; set; }

    public static bool BossKilled { get; set; } = false;
}