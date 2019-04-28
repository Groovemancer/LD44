using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UpgradePanel : MonoBehaviour
{
    Type type;
    Upgrade Upgrade;
    public string UpgradeType;

    Text UpgradeName;
    Text UpgradeDesc;
    Button UpgradeButton;
    Text UpgradeCost;

    string bloodCostText = "Blood Cost: {0}";

    // Start is called before the first frame update
    void Start()
    {
        type = Type.GetType(UpgradeType);
        Upgrade = (Upgrade)Activator.CreateInstance(type);

        UpgradeButton = transform.Find("UpgradeButton").gameObject.GetComponent<Button>();
        UpgradeButton.onClick.AddListener(PurchaseUpgrade);

        UpdatePanelInfo();
    }

    // Update is called once per frame
    void Update()
    {
        UpgradeButton.interactable = GameState.gameState.blood >= Upgrade.GetBloodCost() && Upgrade.Level <= Upgrade.MaxLevel;
    }

    public void PurchaseUpgrade()
    {
        Upgrade u = (Upgrade)Activator.CreateInstance(type);
        u.SetLevel(Upgrade.Level);
        bool success = GameState.gameState.PurchaseUpgrade(u);
        if (success)
        {
            if (Upgrade.Level >= Upgrade.MaxLevel)
            {
                RemovePanel();
            }
            Upgrade.SetLevel(Upgrade.Level + 1);
            UpdatePanelInfo();
        }
    }

    public void RemovePanel()
    {
        Destroy(gameObject);
    }

    public void UpdatePanelInfo()
    {
        UpgradeName = transform.Find("UpgradeName").gameObject.GetComponent<Text>();
        UpgradeName.text = Upgrade.Name;

        UpgradeDesc = transform.Find("UpgradeDesc").gameObject.GetComponent<Text>();
        UpgradeDesc.text = Upgrade.GetDescription();

        UpgradeCost = transform.Find("UpgradeCost").gameObject.GetComponent<Text>();
        UpgradeCost.text = string.Format(bloodCostText, Upgrade.GetBloodCost());
    }
}
