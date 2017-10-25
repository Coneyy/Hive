using Assets.Scripts.Network.PlayerInfrastructure.Models;
using Hive.Assets.Scripts.Network.PlayerInfrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class RegisterClick : MonoBehaviour
{
    public static event Registered RegisteredClicked;
    public delegate void Registered(string email, string username, string password, string confirmPassword);
    
    private InputField email;
    private InputField username;
    private InputField password;
    private InputField confirmPassword;

    public void Start()
    {
        var usernameField = GameObject.FindGameObjectWithTag("RegisterUsername");
        var emailField = GameObject.FindGameObjectWithTag("RegisterEmail");
        var passwordField = GameObject.FindGameObjectWithTag("RegisterPassword");
        var passwordConfirmField = GameObject.FindGameObjectWithTag("RegisterConfirmPassword");

        username = usernameField.GetComponent<InputField>();
        email = emailField.GetComponent<InputField>();
        password = passwordField.GetComponent<InputField>();
        confirmPassword = passwordConfirmField.GetComponent<InputField>();
    }

    public void onClick()
    {
        Debug.Log("Register clicked!");

        if (RegisteredClicked != null)
        {
            RegisteredClicked(email.text, username.text, password.text, confirmPassword.text);
        }
    }
}
