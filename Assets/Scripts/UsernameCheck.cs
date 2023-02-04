using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UsernameCheck : MonoBehaviour
{
    public GameObject UsernamePanel;
    public GameObject OptionsPanel;

    void Start()
    {
        string Username = PlayerPrefs.GetString("Username");
        if(Username != "")
        {
            // Username is set
            UsernamePanel.SetActive(false);
            OptionsPanel.SetActive(true);
        }
        else
        {
            // Username is not set
            UsernamePanel.SetActive(true);
            OptionsPanel.SetActive(false);
        }
    }

    void Update()
    {
        
    }
}
