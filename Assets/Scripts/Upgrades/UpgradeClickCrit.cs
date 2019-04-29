using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UpgradeClickCrit : Upgrade
{
    public float ClickCritChance;
    public float ClickCritChancePerLevel;

    public float ClickCritRate;

    public UpgradeClickCrit() : base()
    {
        Name = "Click Crits";
        Description = "Every click has a {0}% chance to deal {1}% damage. Lvl:{2}";
        BloodCostBase = 35;
        BloodCostPerLevel = 25;
        Level = 1;
        MaxLevel = 10;

        ClickCritChance = 5f;
        ClickCritChancePerLevel = 2.5f;

        ClickCritRate = 250f;

        SetLevel(1);
    }

    public float GetClickCritRate()
    {
        return ClickCritRate;
    }

    public float GetClickCritChance()
    {
        return ClickCritChance + (ClickCritChancePerLevel * (Level - 1));
    }

    public override string GetDescription()
    {
        return string.Format(Description, GetClickCritChance(), ClickCritRate, Level);
    }
}