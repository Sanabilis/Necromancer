using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int playTime;
    public int deaths;
    public int killed;
    public bool bossKilled;

    public GameData(int playTime, int deaths, int killed)
    {
        this.playTime = playTime;
        this.deaths = deaths;
        this.killed = killed;
        this.bossKilled = StaticData.BossKilled;
    }
}