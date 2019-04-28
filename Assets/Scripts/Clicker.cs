using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{
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
                int damage = GameState.gameState.GetClickDamage();
                //Debug.Log("ENEMY HIT!");
                goTargetHit.SendMessage("DamageEnemy", damage, SendMessageOptions.DontRequireReceiver);
                FloatingTextController.CreateFloatingText(damage.ToString(), goTargetHit.transform, Color.white);
            }
            else if (goTargetHit.tag == "GameBoard")
            {
                //Debug.Log("Board hit! Good Enough!");
            }
        }
    }
}
