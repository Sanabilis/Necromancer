using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float[] position;
    public int health;
    public int maxHealth;

    public PlayerData(GameObject player, Vector3 playerPosition)
    {
        position = new float[3];
        position[0] = playerPosition[0];
        position[1] = playerPosition[1];
        position[2] = playerPosition[2];

        health = player.GetComponent<Damageable>().GetHealthAmount();
        maxHealth = player.GetComponent<Damageable>().GetMaxHealthAmount();
    }
}