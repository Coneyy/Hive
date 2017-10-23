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

    private GameObject manager;
    private IPlayerManager playerManager;

    private InputField email;
    private InputField username;
    private InputField password;
    private InputField confirmPassword;

    private Text registerNotifier;

    public void Start()
    {
        manager = GameObject.Find("PlayerManager");
        playerManager = manager.GetComponent<IPlayerManager>();

        var usernameField = GameObject.FindGameObjectWithTag("RegisterUsername");
        var emailField = GameObject.FindGameObjectWithTag("RegisterEmail");
        var passwordField = GameObject.FindGameObjectWithTag("RegisterPassword");
        var passwordConfirmField = GameObject.FindGameObjectWithTag("RegisterConfirmPassword");
        var notifier = GameObject.FindGameObjectWithTag("RegisterNotifier");

        username = usernameField.GetComponent<InputField>();
        email = emailField.GetComponent<InputField>();
        password = passwordField.GetComponent<InputField>();
        confirmPassword = passwordConfirmField.GetComponent<InputField>();
        registerNotifier = notifier.GetComponent<Text>();
    }

    public void onClick()
    {
        Debug.Log("Register click!");
        //try
        //{
            if (password.text.Equals(confirmPassword.text))
            {
                playerManager.RegisterPlayer(email.text, username.text, password.text);
                ClearNotifiactions();
            }
            else
            {
                SetError("Passwords does not match");
            }
        //}
        //catch(WebException ex)
        //{
        //    if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.BadRequest)
        //    {
        //        SetError("User already exists");
        //    }

        //    SetError(ex.Message);
        //}

    }

    private void SetError(string error)
    {
        registerNotifier.text = error;
    }

    private void ClearNotifiactions()
    {
        registerNotifier.text = "";
    }
}
