using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public int bloodValue = 5;

    private float gravity = 0.5f;
    private float speedX = 6f;
    private float speedY = 1f;
    private float fallDuration = 0.5f;
    private Vector2 vel = Vector2.zero;
    private float elapsedTime = 0f;
    private float evaporateTime = 10f;
    private bool pickingUp = false;
    private float pickUpRange = 0.5f;
    private float pickUpSpeed = 1.5f;
    private float pickUpAccel = 0.2f;

    private Vector3 playerPos;

    void Start()
    {
        vel.x = speedX * Random.Range(-1f, 1f);
        vel.y = speedY * Random.Range(1, 1.5f);

        elapsedTime = Random.Range(0, fallDuration * 0.3f);
        playerPos = Camera.main.ScreenToWorldPoint(GameState.gameState.playerObj.transform.position);
        playerPos.z = 0;
    }

    public void SetBloodValue(int value)
    {
        bloodValue = value;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime <= fallDuration)
        {
            transform.position = transform.position + (new Vector3(vel.x, vel.y, 0) * Time.deltaTime);
            vel.y -= gravity;
        }

        if (pickingUp)
        {
            Vector3 direction = playerPos - transform.position;
            
            direction.z = 0;

            transform.position = transform.position + (direction * pickUpSpeed * Time.deltaTime);
            pickUpSpeed += pickUpAccel;

            if (Vector3.Distance(transform.position, playerPos) <= pickUpRange)
            {
                CompletePickup();
            }
        }
        else
        {
            if (elapsedTime >= evaporateTime)
            {
                Destroy(gameObject);
            }
        }
    }

    public void PickupItem()
    {
        PickupItem(1.5f, 0.2f);
    }

    // Update is called once per frame
    public void PickupItem(float pickUpSpeed, float pickUpAccel)
    {
        if (elapsedTime <= fallDuration)
            return;

        this.pickUpSpeed = pickUpSpeed;
        this.pickUpAccel = pickUpAccel;
        pickingUp = true;
    }

    private void CompletePickup()
    {
        GameState.HealPlayer(bloodValue);
        Destroy(gameObject);
    }
}
