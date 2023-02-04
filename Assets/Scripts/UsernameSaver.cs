using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UsernameSaver : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public TMP_InputField usernameInput;
    [SerializeField] public TextMeshProUGUI errorText;
    public GameObject UsernamePanel;
    public GameObject OptionsPanel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void saveUsername()
    {
        if(usernameInput.text != "")
        {
            PlayerPrefs.SetString("Username", usernameInput.text);
            UsernamePanel.SetActive(false);
            OptionsPanel.SetActive(true);
        }
        else
        {
            errorText.text = "Please enter a username";
        }
    }
}
