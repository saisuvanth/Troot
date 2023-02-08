using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResponsiveMainMenu : MonoBehaviour
{
    public RectTransform image;
    void Start()
    {
        if (Screen.width < Screen.height)
        {
            image.anchoredPosition = new Vector2(0, -300);

        }
    }
}
