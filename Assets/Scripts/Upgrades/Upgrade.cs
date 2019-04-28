using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Upgrade
{
    public string Name = "GenericUpgrade";
    public string Description = "Description";
    public int BloodCostBase = 1;
    public int BloodCostPerLevel = 0;
    public int Level = 1;
    public int MaxLevel = 1;

    public Upgrade()
    {
        Name = "GenericUpgrade";
        Description = "Description";
        BloodCostBase = 10;
        BloodCostPerLevel = 10;
        Level = 1;
        MaxLevel = 1;
    }

    public virtual string GetDescription()
    {
        return Description;
    }

    public virtual int GetBloodCost()
    {
        return BloodCostBase + (BloodCostPerLevel * (Level - 1));
    }

    public virtual void SetLevel(int level)
    {
        Level = level;
        if (Level > MaxLevel)
            Level = MaxLevel;
    }

    public virtual void Update()
    {
        //Debug.Log(string.Format("Upgrade::Update '{0}'", Name));
    }
}
