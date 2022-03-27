using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float[] pos;
    public int tornTier;
    public int health;
    public int mana;
    public int gold;

    public SaveData (PlayerManagement player)
    {
        Debug.Log(player.gameObject.transform.position.x);
        pos = new float[3];
        pos[0] = player.gameObject.transform.position.x;
        pos[1] = player.gameObject.transform.position.y;
        pos[2] = player.gameObject.transform.position.z;

        tornTier = player.tornLevel;

        health = player.hp.hp;
        mana = player.abilitys.mana;
        gold = player.purse.money;
    }
}
