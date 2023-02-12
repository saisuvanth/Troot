using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BannerAnimationScript : MonoBehaviour
{
    public Transform banner;
    public CanvasGroup background;
    public GameObject bannerObject;
    public void showBanner()
    {
        background.alpha = 0;
        background.LeanAlpha(0.8f, 0.5f);

        banner.localPosition = new Vector2(0, -Screen.height);
        banner.LeanMoveLocalY(0, 0.5f).setEaseOutExpo().delay=0.1f;
    }

    public void hideBanner()
    {
        Debug.Log("hide banner");
        background.LeanAlpha(0, 0.5f);
        banner.LeanMoveLocalY(-Screen.height, 0.5f).setEaseInExpo().setOnComplete(onCompleted);   
    }

    public void onCompleted()
    {
        bannerObject.SetActive(false);
    }
}
