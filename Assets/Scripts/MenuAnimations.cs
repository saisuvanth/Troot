using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimations : MonoBehaviour
{
    public void ShowMenu()
    {
        transform.LeanMoveLocal(new Vector3(0,-25,0),1).setEaseOutQuart();
    }

    public void HideMenuLeft()
    {
        transform.LeanMoveLocal(new Vector3(-1000,-25,0),1).setEaseOutQuart();
    }

    public void HideMenuRight()
    {
        transform.LeanMoveLocal(new Vector3(1000,-25,0),1).setEaseOutQuart();
    }
}
