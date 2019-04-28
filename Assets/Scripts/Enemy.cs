using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;

    public float attackFreq = 3;
    public float attackFreqVar = 0.3f;

    public int damage = 5;
    public int damageVar = 1;

    public int bloodDrops = 3;
    public int bloodDropsVar = 1;

    public int bloodValue = 5;
    public int bloodValueVar = 2;

    public GameObject bloodDropPrefab;

    private float elapsedTime = 0f;
    private float attackSpeed = 0f;
    private int damageVal;

    // Start is called before the first frame update
    void Start()
    {
        ResetAttackStats();
    }

    private void ResetAttackStats()
    {
        attackSpeed = attackFreq + Random.Range(-attackFreqVar, attackFreqVar);
        elapsedTime = Random.Range(0, attackSpeed);
        damageVal = damage + Random.Range(-damageVar, damageVar + 1);
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= attackSpeed)
        {
            AttackPlayer();
            elapsedTime = 0f;
        }
    }

    public void AttackPlayer()
    {
        GameState.DamagePlayer(damageVal);
        ResetAttackStats();
    }

    public void DamageEnemy(int damage)
    {
        health -= damage;
        GameState.IncreaseTotalDamageDealt(damage);
        if (health <= 0)
        {
            DestroyEnemy();
        }
    }

    public void DestroyEnemy()
    {
        int count = bloodDrops + Random.Range(-bloodDropsVar, bloodDropsVar + 1);
        for (int i = 0; i < count; i++)
        {
            GameObject bloodDrop = GameObject.Instantiate(bloodDropPrefab, transform.position, Quaternion.identity);
            Pickup pickup = bloodDrop.GetComponent<Pickup>();
            pickup.SetBloodValue(bloodValue + Random.Range(-bloodValueVar, bloodValueVar + 1));
        }

        GameState.IncreaseKillCount();
        Destroy(gameObject);
    }
}
