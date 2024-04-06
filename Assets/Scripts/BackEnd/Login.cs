using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{

    public TMP_InputField userNameInput;
    public TMP_InputField PasswordInput;
    public Button LoginButton;
    
    // Start is called before the first frame update
    [Obsolete("Obsolete")]
    void Start()
    {
        LoginButton.onClick.AddListener(() =>
        {
            StartCoroutine(Main.Instance.Web.Login(userNameInput.text, PasswordInput.text));
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
