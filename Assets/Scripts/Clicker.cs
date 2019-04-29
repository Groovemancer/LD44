using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    public GameObject attackEffect;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickAttempt();
        }
        PickupItems();
    }

    private void PickupItems()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5f;

        Vector2 v = Camera.main.ScreenToWorldPoint(mousePos);

        Collider2D[] col = Physics2D.OverlapPointAll(v);

        if (col.Length > 0)
        {
            foreach (Collider2D c in col)
            {
                if (c.gameObject.tag == "Pickup")
                {
                    c.gameObject.SendMessage("PickupItem", null, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }

    private void ClickAttempt()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5f;

        Vector2 v = Camera.main.ScreenToWorldPoint(mousePos);

        Collider2D[] col = Physics2D.OverlapPointAll(v);

        GameObject goTargetHit = null;

        if (col.Length > 0)
        {
            foreach (Collider2D c in col)
            {
                goTargetHit = c.gameObject;
                if (c.gameObject.tag == "Enemy")
                {
                    break;
                }
            }
        }

        if (goTargetHit != null)
        {
            if (goTargetHit != null && goTargetHit.tag == "Enemy")
            {
                if (attackEffect != null)
                {
                    Instantiate(attackEffect, goTargetHit.transform.position, Quaternion.identity);
                }

                int damage = GameState.gameState.GetClickDamage();
                bool isCrit = GameState.gameState.IsHitACrit();

                Color color = Color.white;
                int fontSize = 32;

                if (isCrit)
                {
                    color = Color.red;
                    fontSize = 44;
                    damage = (int)(damage * GameState.gameState.GetCritRate() / 100f);
                }

                //Debug.Log("ENEMY HIT!");
                goTargetHit.SendMessage("DamageEnemy", damage, SendMessageOptions.DontRequireReceiver);
                FloatingTextController.CreateFloatingText(damage.ToString(), goTargetHit.transform, color, fontSize);
            }
            else if (goTargetHit.tag == "GameBoard")
            {
                //Debug.Log("Board hit! Good Enough!");
            }
        }
    }
}
