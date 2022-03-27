using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    public Purse purse;
    public Health hp;
    public Spells abilitys;
    public int tornLevel;

    private void Start()
    {
        purse = GetComponent<Purse>();
        hp = GetComponent<Health>();
        abilitys = GetComponent<Spells>();
    }

    public void LoadData(SaveData data)
    {
        transform.position = new Vector3(data.pos[0], data.pos[1], data.pos[2]);

        tornLevel = data.tornTier;

        hp.hp = data.health;
        abilitys.mana = data.mana;
        purse.money = data.gold;
    }

    public void Load()
    {
        Looder.Load(this, "wizard.triel");
    }

    public void save()
    {
        Looder.Save(this, "wizard.triel");
    }
}
