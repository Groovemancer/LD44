using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    public static GameState gameState;

    public GameObject playerObj;
    public GameObject levelObj;
    public GameObject damageObj;
    public GameObject killsObj;

    public GameObject currentEnemy;

    [SerializeField]
    public List<EnemyLevel> EnemyLevels = new List<EnemyLevel>();

    public List<int> KillsPerLevel = new List<int>();

    public List<Upgrade> Upgrades = new List<Upgrade>();

    public int currentLevel = 1;

    public int blood = 100;
    public int initialBlood = 100;

    public int bloodSpent = 0;

    public int damage = 1;
    public int initialDamage = 1;
    public int kills = 0;

    public int totalDamageDealt = 0;

    public Color damageColor = Color.red;
    public Color healColor = Color.green;

    private string bloodText = "Blood: {#}";
    private string levelText = "Level: {#}";
    private string damageText = "Damage: {#}";
    private string killsText = "Kills: {#}";

    private float elapsedTime = 0f;
    private float checkForEnemy = 0.3f; // Check for an enemy every x seconds

    private bool gameOver = false;

    private void Awake()
    {
        if (gameState == null)
        {
            DontDestroyOnLoad(gameObject);
            gameState = this;
        }
        else if (gameState != this)
        {
            Destroy(gameObject);
        }
        ResetGame();
    }

    private void Update()
    {
        if (gameOver)
            return;

        if (currentEnemy == null)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= checkForEnemy)
            {
                elapsedTime = 0;
                FindEnemy();
            }
        }

        Upgrades.ForEach(u => u.Update());
    }

    private void Initialize()
    {
        ResetGame();
    }

    public static void ResetGame()
    {
        Debug.Log("Reset game");
        FloatingTextController.Initialize();

        gameState.Upgrades.Clear();

        gameState.damage = gameState.initialDamage;
        gameState.kills = 0;
        gameState.currentLevel = 1;

        gameState.gameOver = false;
        gameState.blood = gameState.initialBlood;
        gameState.bloodSpent = 0;
        FindPlayer();
        FindLevel();
        FindDamage();
        FindKills();
        UpdatePlayerBlood();
        UpdatePlayerLevel();
        UpdatePlayerDamage();
        UpdatePlayerKills();
    }

    public void UpdateInfo()
    {
        FindPlayer();
        FindLevel();
        FindDamage();
        FindKills();
        UpdatePlayerBlood();
        UpdatePlayerLevel();
        UpdatePlayerDamage();
        UpdatePlayerKills();
    }

    public int GetClickDamage()
    {
        int result = damage;

        UpgradeClickDamage upgrade = FindUpgradeOfType(typeof(UpgradeClickDamage)) as UpgradeClickDamage;

        if (upgrade != null)
        {
            result += upgrade.GetClickDamageBonus();
        }

        return result;
    }

    public Upgrade FindUpgradeOfType(Type type)
    {
        foreach (Upgrade upgrade in Upgrades)
        {
            if (upgrade.GetType() == type)
            {
                return upgrade;
            }
        }

        return null;
    }

    public bool PurchaseUpgrade(Upgrade upgrade)
    {
        if (blood >= upgrade.GetBloodCost())
        {
            blood = Math.Max(blood - upgrade.GetBloodCost(), 0);

            bloodSpent += upgrade.GetBloodCost();

            UpdateMaxBloodSpent();
            UpdateTotalBloodSpent(upgrade.GetBloodCost());

            UpdatePlayerBlood();

            if (blood <= 0)
            {
                GameOver();
            }

            Upgrade u = FindUpgradeOfType(upgrade.GetType());

            if (u != null)
            {
                u.SetLevel(upgrade.Level);
            }
            else
            {
                Upgrades.Add(upgrade);
            }

            UpdateInfo();
            return true;
        }
        return false;
    }

    private void FindEnemy()
    {
        if (currentEnemy == null)
        {
            currentEnemy = GameObject.FindGameObjectWithTag("Enemy");

            if (currentEnemy == null)
            {
                CreateEnemy();
            }
        }
    }

    public static void CreateEnemy()
    {
        EnemyLevel enemyLevel = gameState.EnemyLevels.Find(el => el.Level == gameState.currentLevel);

        int prevLevel = 1;
        while (enemyLevel == null)
        {
            enemyLevel = gameState.EnemyLevels.Find(el => el.Level == gameState.currentLevel - prevLevel);
            prevLevel--;

            if (prevLevel <= 0)
                break;
        }

        if (enemyLevel != null)
        {
            List<EnemyPrefab> enemyPrefabs = enemyLevel.EnemyPrefabs;

            List<GameObject> prefabs = new List<GameObject>();

            foreach (EnemyPrefab prefab in enemyPrefabs)
            {
                for (int i = 0; i < prefab.Weight; i++)
                {
                    prefabs.Add(prefab.Prefab);
                }
            }

            int idx = UnityEngine.Random.Range(0, prefabs.Count);

            gameState.currentEnemy = GameObject.Instantiate(prefabs[idx]);
        }
    }

    private static void FindPlayer()
    {
        if (gameState.playerObj == null)
        {
            gameState.playerObj = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private static void FindLevel()
    {
        if (gameState.levelObj == null)
        {
            gameState.levelObj = GameObject.FindGameObjectWithTag("Level");
        }
    }

    private static void FindDamage()
    {
        if (gameState.damageObj == null)
        {
            gameState.damageObj = GameObject.FindGameObjectWithTag("Damage");
        }
    }

    private static void FindKills()
    {
        if (gameState.killsObj == null)
        {
            gameState.killsObj = GameObject.FindGameObjectWithTag("Kills");
        }
    }

    public static void DamagePlayer(int damage)
    {
        FindPlayer(); // Player health bar/text/whatever we call it in UI

        if (gameState.playerObj != null)
        {
            FloatingTextController.CreateFloatingText(damage.ToString(), gameState.playerObj.transform, gameState.damageColor, 44, false);
        }

        ModifyBlood(-damage);
    }

    public static void ModifyBlood(int amount)
    {
        gameState.blood = Math.Max(gameState.blood + amount, 0);

        UpdateMaxBloodCollected();

        if (amount > 0)
            UpdateTotalBloodCollected(amount);

        UpdatePlayerBlood();

        if (gameState.blood <= 0)
        {
            GameOver();
        }
    }

    public static void UpdateMaxBloodCollected()
    {
        int maxBlood = 0;
        if (PlayerPrefs.HasKey("MaxBlood"))
        {
            maxBlood = PlayerPrefs.GetInt("MaxBlood");
        }
        if (gameState.blood > maxBlood)
        {
            maxBlood = gameState.blood;
        }
        PlayerPrefs.SetInt("MaxBlood", maxBlood);
    }

    public static void UpdateTotalBloodCollected(int blood)
    {
        int totalBlood = 0;
        if (PlayerPrefs.HasKey("TotalBlood"))
        {
            totalBlood = PlayerPrefs.GetInt("TotalBlood");
        }
        totalBlood += blood;
        PlayerPrefs.SetInt("TotalBlood", totalBlood);
    }

    public static void UpdateMaxBloodSpent()
    {
        int maxBloodSpent = 0;
        if (PlayerPrefs.HasKey("MaxBloodSpent"))
        {
            maxBloodSpent = PlayerPrefs.GetInt("MaxBloodSpent");
        }
        if (gameState.bloodSpent > maxBloodSpent)
        {
            maxBloodSpent = gameState.bloodSpent;
        }
        PlayerPrefs.SetInt("MaxBloodSpent", maxBloodSpent);
    }

    public static void UpdateTotalBloodSpent(int bloodSpent)
    {
        int totalBloodSpent = 0;
        if (PlayerPrefs.HasKey("TotalBloodSpent"))
        {
            totalBloodSpent = PlayerPrefs.GetInt("TotalBloodSpent");
        }
        totalBloodSpent += bloodSpent;
        PlayerPrefs.SetInt("TotalBloodSpent", totalBloodSpent);
    }

    public static void UpdateMaxKills()
    {
        int maxKills = 0;
        if (PlayerPrefs.HasKey("MaxKills"))
        {
            maxKills = PlayerPrefs.GetInt("MaxKills");
        }
        if (gameState.kills > maxKills)
        {
            maxKills = gameState.kills;
        }
        PlayerPrefs.SetInt("MaxKills", maxKills);
    }

    public static void UpdateTotalKills()
    {
        int totalKills = 0;
        if (PlayerPrefs.HasKey("TotalKills"))
        {
            totalKills = PlayerPrefs.GetInt("TotalKills");
        }
        totalKills++;
        PlayerPrefs.SetInt("TotalKills", totalKills);
    }

    public static void UpdateMaxDamage()
    {
        int maxDamage = 0;
        if (PlayerPrefs.HasKey("MaxDamage"))
        {
            maxDamage = PlayerPrefs.GetInt("MaxDamage");
        }
        if (gameState.totalDamageDealt > maxDamage)
        {
            maxDamage = gameState.totalDamageDealt;
        }
        PlayerPrefs.SetInt("MaxDamage", maxDamage);
    }

    public static void UpdateTotalDamage(int damage)
    {
        int totalDamage = 0;
        if (PlayerPrefs.HasKey("TotalDamage"))
        {
            totalDamage = PlayerPrefs.GetInt("TotalDamage");
        }
        totalDamage += damage;
        PlayerPrefs.SetInt("TotalDamage", totalDamage);
    }

    public static void IncreaseTotalDamageDealt(int damage)
    {
        gameState.totalDamageDealt += damage;
        UpdateMaxDamage();
        UpdateTotalDamage(damage);
    }

    public static void GameOver()
    {
        Destroy(gameState.currentEnemy);
        Destroy(gameState.playerObj);
        Destroy(gameState.levelObj);
        Destroy(gameState.killsObj);
        Destroy(gameState.damageObj);

        gameState.currentEnemy = null;
        gameState.playerObj = null;
        gameState.levelObj = null;
        gameState.killsObj = null;
        gameState.damageObj = null;

        gameState.gameOver = true;
        UpdateMaxBloodCollected();
        Debug.Log("You lose!");
        SceneManager.LoadScene("TitleScreen");
    }

    public static void HealPlayer(int blood)
    {
        FindPlayer(); // Player health bar/text/whatever we call it in UI

        if (gameState.playerObj != null)
        {
            FloatingTextController.CreateFloatingText(blood.ToString(), gameState.playerObj.transform, gameState.healColor, 44, false);
        }

        ModifyBlood(blood);
    }

    public static void IncreaseKillCount()
    {
        gameState.kills++;

        UpdateMaxKills();
        UpdateTotalKills();

        UpdatePlayerKills();

        if (gameState.currentLevel < gameState.KillsPerLevel.Count)
        {
            if (gameState.kills >= gameState.KillsPerLevel[gameState.currentLevel - 1])
            {
                IncreaseLevel();
            }
        }
    }

    public static void IncreaseLevel()
    {
        gameState.currentLevel++;
        UpdatePlayerLevel();
    }

    public static void UpdatePlayerBlood()
    {
        if (gameState.playerObj == null)
            return;
        Text text = gameState.playerObj.GetComponent<Text>();
        text.text = gameState.bloodText.Replace("{#}", gameState.blood.ToString());
    }

    public static void UpdatePlayerLevel()
    {
        if (gameState.levelObj == null)
            return;
        Text text = gameState.levelObj.GetComponent<Text>();
        text.text = gameState.levelText.Replace("{#}", gameState.currentLevel.ToString());
    }

    public static void UpdatePlayerDamage()
    {
        if (gameState.damageObj == null)
            return;
        Text text = gameState.damageObj.GetComponent<Text>();
        text.text = gameState.damageText.Replace("{#}", gameState.GetClickDamage().ToString());
    }

    public static void UpdatePlayerKills()
    {
        if (gameState.killsObj == null)
            return;
        Text text = gameState.killsObj.GetComponent<Text>();
        text.text = gameState.killsText.Replace("{#}", gameState.kills.ToString());
    }
}