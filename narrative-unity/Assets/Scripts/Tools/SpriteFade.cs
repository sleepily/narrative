using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteFade : MonoBehaviour
{
    [SerializeField]
    bool forceActive = true;

    Image image;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        image = GetComponent<Image>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (!image && !spriteRenderer)
        {
            Debug.Log($"No fadeable component found on {gameObject.name}");
            return;
        }

        spriteRenderer.enabled = true;
    }

    public void FadeAway()
    {
        if (spriteRenderer)
            spriteRenderer.color = Color.clear;

        if (image)
            image.color = Color.clear;
    }

    public void FadeToColor(Color color, float fadeTime)
    {

    }
}
