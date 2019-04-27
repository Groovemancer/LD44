using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageNumbers : MonoBehaviour
{
    public GameObject canvas;
    // attributes
    public Vector2 velocity = Vector2.zero;
    public float duration = 20f;
    public float alpha = 1f;
    public bool bTextMesh = false;

    // this will create a UI text game object in the passed canvas transform
    // parameters: pos - it should be a world space position value
    //             parentTransform - it should be a canvas game object's transform
    public static DamageNumbers getInstance(string text, Vector3 pos, int fontSize, Color color, Vector2 velocity, float duration, Transform parentTransform)
    {
        // create game object
        GameObject obj = new GameObject("Flying Text");
        obj.transform.parent = parentTransform;
        obj.transform.position = Camera.main.WorldToScreenPoint(pos);
        obj.transform.localScale = new Vector3(1, 1, 1);

        // add text component
        Text t = obj.AddComponent<Text>();
        t.text = text;
        t.alignment = TextAnchor.MiddleCenter;
        t.font = Font.CreateDynamicFontFromOSFont("Arial", 20);
        //t.font = Resources.Load ("Fonts/slkscr") as Font; // use custom font
        t.fontSize = fontSize;
        t.color = color;

        // change rect transform value
        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(200, 40);

        // add script component
        DamageNumbers ft = obj.AddComponent<DamageNumbers>();
        ft.bTextMesh = false;
        ft.velocity = velocity;
        ft.duration = duration;

        return ft;
    }

    // this will create a TextMesh game object in the passed parent transform
    public static DamageNumbers getInstanceWithTextMesh(string text, Vector3 pos, int fontSize, Color color, Vector2 velocity, float duration, Transform parentTransform)
    {
        pos.z = parentTransform.position.z;

        // create game object
        GameObject obj = new GameObject("Flying Text");
        obj.transform.parent = parentTransform;
        obj.transform.position = pos;
        obj.transform.localScale = new Vector3(1, 1, 1);

        // add text component
        TextMesh t = obj.AddComponent<TextMesh>();
        t.text = text;
        t.alignment = TextAlignment.Left;
        t.font = Font.CreateDynamicFontFromOSFont("Arial", 20);
        //        t.font = Resources.Load ("Fonts/slkscr") as Font;
        t.fontSize = fontSize;
        t.color = color;
        t.characterSize = 0.1f;

        // assign renderer material
        Renderer r = obj.GetComponent<Renderer>();
        r.sharedMaterial = t.font.material;
        // set position, make the passed pos parameter as center
        pos.x -= r.bounds.size.x / 2;
        obj.transform.position = pos;

        // add script component
        DamageNumbers ft = obj.AddComponent<DamageNumbers>();
        ft.bTextMesh = true;
        ft.velocity = velocity;
        ft.duration = duration;

        return ft;
    }

    // Update is called once per frame
    void Update()
    {
        if (alpha > 0)
        {
            // change the y position
            Vector3 pos = transform.position;
            pos.x += velocity.x * Time.deltaTime;
            pos.y += velocity.y * Time.deltaTime;
            transform.position = pos;

            // change alpha value
            alpha -= Time.deltaTime / duration;

            if (bTextMesh)
            {
                TextMesh t = GetComponent<TextMesh>();
                if (t != null)
                {
                    Color color = t.color;
                    color.a = alpha;
                    t.color = color;
                }
            }
            else
            {
                Text t = GetComponent<Text>();
                if (t != null)
                {
                    Color color = t.color;
                    color.a = alpha;
                    t.color = color;
                }
            }
        }
    }

    public void DisplayText(object[] parms)
    {
        string text = (string)parms[0];
        Transform target = (Transform)parms[1];
        Vector2 pos = target.position;
        Vector2 velocity = new Vector2(
            30 * Random.Range(-1.0f, 1f),
            80);
        float duration = 3f;
        getInstance(text, pos, 32, Color.white, velocity, duration, canvas.transform);
    }
}

public struct DisplayTextParams
{
    public string text;
    public Transform target;
}