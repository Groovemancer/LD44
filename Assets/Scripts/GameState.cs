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

    public int currentLevel = 1;

    public int blood = 100;
    public int maxBlood = 100;
    public int initialBlood = 100;

    public int damage = 1;
    public int initialDamage = 1;
    public int kills = 0;

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
    }

    private void Initialize()
    {
        ResetGame();
    }

    public static void ResetGame()
    {
        Debug.Log("Reset game");
        FloatingTextController.Initialize();

        gameState.damage = gameState.initialDamage;
        gameState.kills = 0;
        gameState.currentLevel = 1;

        gameState.gameOver = false;
        gameState.blood = gameState.initialBlood;
        gameState.maxBlood = gameState.initialBlood;
        FindPlayer();
        FindLevel();
        FindDamage();
        FindKills();
        UpdatePlayerBlood();
        UpdatePlayerLevel();
        UpdatePlayerDamage();
        UpdatePlayerKills();
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

        gameState.blood = Math.Max(gameState.blood - damage, 0);

        UpdatePlayerBlood();

        if (gameState.blood <= 0)
        {
            GameOver();
        }
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

        gameState.blood = Math.Max(gameState.blood + blood, 0);

        UpdatePlayerBlood();

        if (gameState.blood <= 0)
        {
            GameOver();
        }
    }

    public static void IncreaseKillCount()
    {
        gameState.kills++;
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
        text.text = gameState.damageText.Replace("{#}", gameState.damage.ToString());
    }

    public static void UpdatePlayerKills()
    {
        if (gameState.killsObj == null)
            return;
        Text text = gameState.killsObj.GetComponent<Text>();
        text.text = gameState.killsText.Replace("{#}", gameState.kills.ToString());
    }
}