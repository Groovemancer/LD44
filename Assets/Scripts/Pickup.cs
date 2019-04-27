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

    void Start()
    {
        vel.x = speedX * Random.Range(-1f, 1f);
        vel.y = speedY * Random.Range(1, 1.5f);

        elapsedTime = Random.Range(0, fallDuration * 0.3f);
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
    }

    // Update is called once per frame
    public void PickupItem()
    {
        if (elapsedTime <= fallDuration)
            return;

        Debug.Log("Collect Blood: " + bloodValue);
        GameState.HealPlayer(bloodValue);
        Destroy(gameObject);
    }
}
