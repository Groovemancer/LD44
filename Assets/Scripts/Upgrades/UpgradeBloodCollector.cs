using UnityEngine;

public class UpgradeBloodCollector : Upgrade
{
    public float CollectorFreq;
    public float CollectorFreqPerLevel;

    private float elapsedTime = 0f;

    public UpgradeBloodCollector() : base()
    {
        Name = "Blood Collector";
        Description = "Collects uncollected blood every {0} seconds. Lvl:{1}";
        BloodCostBase = 50;
        BloodCostPerLevel = 50;
        Level = 1;
        MaxLevel = 6;

        CollectorFreq = 8;
        CollectorFreqPerLevel = -0.5f;

        SetLevel(1);
    }

    public float GetCollectorFrequency()
    {
        return CollectorFreq + (CollectorFreqPerLevel * (Level - 1));
    }

    public override string GetDescription()
    {
        return string.Format(Description, GetCollectorFrequency(), Level);
    }

    public override void Update()
    {
        base.Update();

        elapsedTime += Time.deltaTime;
        if (elapsedTime >= GetCollectorFrequency())
        {
            elapsedTime = 0;

            GameObject[] bloodDroplets = GameObject.FindGameObjectsWithTag("Pickup");
            foreach (GameObject bloodDroplet in bloodDroplets)
            {
                Pickup pickup = bloodDroplet.GetComponent<Pickup>();
                if (pickup != null)
                {
                    pickup.PickupItem(0.5f, 0.1f);
                }
            }
        }
    }
}