using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetHighScore : MonoBehaviour
{
    string scoreBoardText = "Max Blood Collected: {0}\nTotal Blood Collected: {1}\nMax Blood Spent: {2}\nTotal Blood Spent: {3}\n" +
        "Max Kills: {4}\nTotal Kills: {5}\nMax Damage Dealt: {6}\nTotal Damage Dealt: {7}\n";
    // Start is called before the first frame update
    void Start()
    {
        Text highScoreText = GetComponent<Text>();
        int maxBlood = 0;
        int totalBlood = 0;
        int maxBloodSpent = 0;
        int totalBloodSpent = 0;
        int maxKills = 0;
        int totalKills = 0;
        int maxDamage = 0;
        int totalDamage = 0;

        if (PlayerPrefs.HasKey("MaxBlood"))
        {
            maxBlood = PlayerPrefs.GetInt("MaxBlood");
        }
        if (PlayerPrefs.HasKey("TotalBlood"))
        {
            totalBlood = PlayerPrefs.GetInt("TotalBlood");
        }
        if (PlayerPrefs.HasKey("MaxBloodSpent"))
        {
            maxBloodSpent = PlayerPrefs.GetInt("MaxBloodSpent");
        }
        if (PlayerPrefs.HasKey("TotalBloodSpent"))
        {
            totalBloodSpent = PlayerPrefs.GetInt("TotalBloodSpent");
        }
        if (PlayerPrefs.HasKey("MaxKills"))
        {
            maxKills = PlayerPrefs.GetInt("MaxKills");
        }
        if (PlayerPrefs.HasKey("TotalKills"))
        {
            totalKills = PlayerPrefs.GetInt("TotalKills");
        }
        if (PlayerPrefs.HasKey("MaxDamage"))
        {
            maxDamage = PlayerPrefs.GetInt("MaxDamage");
        }
        if (PlayerPrefs.HasKey("TotalDamage"))
        {
            totalDamage = PlayerPrefs.GetInt("TotalDamage");
        }

        highScoreText.text = string.Format(scoreBoardText,
            maxBlood, totalBlood, maxBloodSpent, totalBloodSpent,
            maxKills, totalKills, maxDamage, totalDamage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
