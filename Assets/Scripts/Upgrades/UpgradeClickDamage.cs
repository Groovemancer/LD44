using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UpgradeClickDamage : Upgrade
{
    public int ClickDamageBase;
    public int ClickDamagePerLevel;

    public UpgradeClickDamage() : base()
    {
        Name = "Click Damage";
        Description = "Increases damage dealt from your clicks.\n+{0} damage. Lvl:{1}";
        BloodCostBase = 25;
        BloodCostPerLevel = 15;
        Level = 1;
        MaxLevel = 10;

        ClickDamageBase = 1;
        ClickDamagePerLevel = 1;

        SetLevel(1);
    }

    public int GetClickDamageBonus()
    {
        return ClickDamageBase + (ClickDamagePerLevel * (Level - 1));
    }

    public override string GetDescription()
    {
        return string.Format(Description, GetClickDamageBonus(), Level);
    }
}