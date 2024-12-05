using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverIncreaseSize : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    private Vector3 originalScale;

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        originalScale = transform.localScale; // Save the original scale of the text
    }

    public void OnPointerEnter()
    {
         transform.localScale = originalScale * 9f; // 1.2 times the original size
    }

    public void OnPointerExit()
    {
        transform.localScale = originalScale;
    }
}
