using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    public int damage = 1;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickAttempt();
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
                Debug.Log("ENEMY HIT!");
                object[] parms = new object[2] { damage.ToString(), goTargetHit.transform };
                goTargetHit.SendMessage("Clicked", damage, SendMessageOptions.DontRequireReceiver);
                this.SendMessage("DisplayText", parms, SendMessageOptions.DontRequireReceiver);
            }
            else if (goTargetHit.tag == "GameBoard")
            {
                Debug.Log("Board hit! Good Enough!");
            }
        }
    }
}
