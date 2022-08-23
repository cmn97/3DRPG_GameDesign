using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StatSheet : MonoBehaviour
{
    public int STR; // 0, Strength
    public int STRMod;
    public int DEX; // 1, Dexterity
    public int DEXMod;
    public int CON; // 2, Constitution
    public int CONMod;

    public int HP;
    public int DEF;

    //public int XP;
    //public int level;

    public List<StatBase> CharacterStats = new List<StatBase>();
    public List<int> StatModifiers = new List<int>();
    System.Random dice = new System.Random(); // Random number generator

    public void Start()
    {
        calculateStats();
    }

    public void calculateStats()
    {
        // Strength
        STRMod = calculateMod(STR);
        CharacterStats.Add(new StatBase(STR));
        StatModifiers.Add(STRMod);

        // Dexterity
        DEXMod = calculateMod(DEX);
        CharacterStats.Add(new StatBase(DEX));
        StatModifiers.Add(DEXMod);

        // Constitution
        CONMod = calculateMod(CON);
        CharacterStats.Add(new StatBase(CON));
        StatModifiers.Add(CONMod);

        // Defense
        DEF = 10 + DEXMod;

        // Hit points. Add more to player
        if(gameObject.tag == "Player")
        {
            HP = 20 + CONMod;
        }
        else
        {
            HP = 10 + CONMod;
        }

        //level = 1;
        //XP = 0;

        PrintStats();
    }

    public void PrintStats()
    {
        // Print all stat values to console for debugging
        Debug.Log(
            "  Strength: " + CharacterStats[0].Value.ToString() + ", mod " + StatModifiers[0].ToString() +
            ", Dexterity: " + CharacterStats[1].Value.ToString() + ", mod " + StatModifiers[1].ToString() +
            ", Constitution: " + CharacterStats[2].Value.ToString() + ", mod " + StatModifiers[2].ToString() +
            ", HP: " + HP.ToString() +
            ", Defense: " + DEF.ToString());
    }

    private int calculateMod(int stat)
    {
        int mod;
        if (stat <= 5) { mod = -3; }
        else if (stat == 6 || stat == 7) { mod = -2; }
        else if (stat == 8 || stat == 9) { mod = -1; }
        else if (stat == 10 || stat == 11) { mod = 0; }
        else if (stat == 12 || stat == 13) { mod = 1; }
        else if (stat == 14 || stat == 15) { mod = 2; }
        else if (stat == 16 || stat == 17) { mod = 3; }
        else if (stat == 18 || stat == 19) { mod = 4; }
        else { mod = 5; }
        return mod;
    }
}
