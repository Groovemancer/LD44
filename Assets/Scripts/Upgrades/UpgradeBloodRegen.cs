using UnityEngine;

public class UpgradeBloodRegen : Upgrade
{
    public float RegenFreq;
    public float RegenFreqFreqPerLevel;

    public int RegenAmount;
    public int RegenAmountPerLevel;
    public int RegenCap;
    public int RegenCapPerLevel;

    private float elapsedTime = 0f;

    public UpgradeBloodRegen() : base()
    {
        Name = "Blood Generator";
        Description = "Automatically generates {0} blood every {1} seconds. Stops generating when at {2} blood. Lvl:{3}";
        BloodCostBase = 150;
        BloodCostPerLevel = 100;
        Level = 1;
        MaxLevel = 5;

        RegenFreq = 4;
        RegenFreqFreqPerLevel = -0.5f;
        RegenAmount = 3;
        RegenAmountPerLevel = 2;
        RegenCap = 300;
        RegenCapPerLevel = 100;

        SetLevel(1);
    }

    public float GetRegenFrequency()
    {
        return RegenFreq + (RegenFreqFreqPerLevel * (Level - 1));
    }

    public int GetRegenAmount()
    {
        return RegenAmount + (RegenAmountPerLevel * (Level - 1));
    }

    public int GetRegenCap()
    {
        return RegenCap + (RegenCapPerLevel * (Level - 1));
    }

    public override string GetDescription()
    {
        return string.Format(Description, GetRegenAmount(), GetRegenFrequency(), GetRegenCap(), Level);
    }

    public override void Update()
    {
        base.Update();

        elapsedTime += Time.deltaTime;
        if (elapsedTime >= GetRegenFrequency() && GameState.gameState.blood < GetRegenCap())
        {
            elapsedTime = 0;

            GameState.HealPlayer(GetRegenAmount());
        }
    }
}