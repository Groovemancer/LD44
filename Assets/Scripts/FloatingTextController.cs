using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextController : MonoBehaviour
{
    private static FloatingText popupText;
    private static GameObject canvas;

    public static void Initialize()
    {
        canvas = GameObject.Find("Canvas");
        if (!popupText)
            popupText = Resources.Load<FloatingText>("Prefabs/UI/PopupTextParent");
    }

    public static void CreateFloatingText(string text, Transform location, Color color, int fontSize = 32, bool worldPos = true)
    {
        FloatingText instance = Instantiate(popupText);
        Vector2 pos = location.position;
        pos.x += Random.Range(-0.5f, 0.5f);
        pos.y += Random.Range(-0.5f, 0.5f);
        Vector2 screenPosition;
        if (worldPos)
        {
            screenPosition = Camera.main.WorldToScreenPoint(pos);
        }
        else
        {
            screenPosition = pos;
        }
        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = screenPosition;
        instance.SetText(text, color, fontSize);
    }
}
