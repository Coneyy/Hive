using Assets.Scripts.Network.PlayerInfrastructure.Models;
using Hive.Assets.Scripts.Network.PlayerInfrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class LoginClick : MonoBehaviour
{
    private InputField emailOrUsername;
    private InputField password;

    public static event LogIn LoginClicked;
    public delegate void LogIn(string emailOrUsername, string password);

    public void Start()
    {
        var usernameField = GameObject.FindGameObjectWithTag("LoginUsername");
        var passwordField = GameObject.FindGameObjectWithTag("LoginPassword");

        emailOrUsername = usernameField.GetComponent<InputField>();
        password = passwordField.GetComponent<InputField>();
    }

    public void onClick()
    {
        Debug.Log("Login clicked, email: " + emailOrUsername + " password: " + password);

        if (LoginClicked != null)
        {
            LoginClicked(emailOrUsername.text, password.text);
        }
    }


}
