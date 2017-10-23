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

    public GameObject imageTarget { get; set; }
    public GameObject stringOne { get; set; }

    private GameObject manager;
    private IPlayerManager playerManager;

    private InputField emailOrPassword;
    private InputField password;

    private Text loginNotifier;

    public void Start()
    {
        manager = GameObject.Find("PlayerManager");
        playerManager = manager.GetComponent<IPlayerManager>();

        var usernameField = GameObject.FindGameObjectWithTag("LoginUsername");
        var passwordField = GameObject.FindGameObjectWithTag("LoginPassword");
        var notifier = GameObject.FindGameObjectWithTag("LoginNotifier");

        emailOrPassword = usernameField.GetComponent<InputField>();
        password = passwordField.GetComponent<InputField>();
        loginNotifier = notifier.GetComponent<Text>();
    }

    public void onClick()
    {
        try
        {
            Debug.Log("Clicked, email: " + emailOrPassword + " password: " + password);
            playerManager.SignPlayerIn(emailOrPassword.text, password.text);
            ClearNotifiactions();
        }
        catch (WebException ex)
        {
            SetError(ex.Message);
        }
        catch(NotFoundException ex)
        {
            SetError("Bad credentials");
        }
    }

    private void SetError(string error)
    {
        loginNotifier.text = error;
    }

    private void ClearNotifiactions()
    {
        loginNotifier.text = "";
    }
}
