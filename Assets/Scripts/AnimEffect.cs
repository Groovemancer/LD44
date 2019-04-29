using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEffect : MonoBehaviour
{
    public float lifeSpan = 3f;
    public float rotationVar = 15;
    public bool canFlip = true;

    private Color baseColor;

    private float elapsedTime = 0f;
    private SpriteRenderer spriteRenderer;
    private float baseAlpha = 1;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseColor = spriteRenderer.color;
        baseAlpha = baseColor.a;
        spriteRenderer.color = baseColor;

        transform.Rotate(new Vector3(0, 0, Random.Range(-rotationVar, rotationVar)));

        Vector3 pos = transform.position;
        pos.x += Random.Range(-0.5f, 0.5f);
        pos.y += Random.Range(-0.5f, 0.5f);
        transform.position = pos;

        if (canFlip)
        {
            int f = Random.Range(0, 2);

            transform.localScale = new Vector3(f == 0 ? -1 : 1, 1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        baseColor.a = Mathf.Max((lifeSpan - elapsedTime), 0) / lifeSpan * baseAlpha;
        spriteRenderer.color = baseColor;

        if (elapsedTime >= lifeSpan)
        {
            Destroy(gameObject);
        }
    }
}
