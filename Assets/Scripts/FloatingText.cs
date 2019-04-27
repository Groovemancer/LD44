using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    public Animator animator;
    private Text floatingText;

    void Awake()
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length - 0.1f);
        floatingText = animator.GetComponent<Text>();
    }

    public void SetText(string text)
    {
        floatingText.text = text;
    }

    public void SetText(string text, Color color, int fontSize = 32)
    {
        floatingText.text = text;
        floatingText.fontSize = fontSize;
        floatingText.color = color;
    }
}
