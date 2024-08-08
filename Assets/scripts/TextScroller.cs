using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TextScroller : MonoBehaviour
{
    public Text scrollingText;
    public float scrollSpeed = 20f;
    public string textFileName = "TextFile";

    private string textContent;

    void Start()
    {
        // Lataa tekstitiedosto Resources-kansiosta
        TextAsset textAsset = Resources.Load<TextAsset>(textFileName);
        if (textAsset != null)
        {
            textContent = textAsset.text;
            scrollingText.text = textContent;
        }
        else
        {
            Debug.LogError("Text file not found!");
        }
    }

    void Update()
    {
        // Skrollaa tekstiä ylöspäin
        scrollingText.rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        // Tarkista, onko teksti kokonaan ulkona ruudulta, ja siirrä se takaisin alas
        if (scrollingText.rectTransform.anchoredPosition.y > scrollingText.rectTransform.rect.height)
        {
            scrollingText.rectTransform.anchoredPosition = new Vector2(scrollingText.rectTransform.anchoredPosition.x, 0);
        }
    }
}
