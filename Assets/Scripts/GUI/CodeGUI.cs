using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Hive.Assets.Scripts.Network.PlayerInfrastructure;

public class CodeGUI : MonoBehaviour
{

    //  public Canvas GUI;
    public GameObject Panel;

    private static bool hideBottomBarBool = true;
    private bool Stop = false;
    private bool canBeHidedByTouch = false;
    private bool canBeShovedByTouch = false;

    public float HiddenBottomBar;
    public float ShowedBottomBar;

    public float panelHeight;


    private Vector3 startToTouch;

    GameObject manager;
    GameObject bottomBar;
    SelectManager SManager;
    GameObject LoggingMenu; //menu (canvas) z logowaniem i rejestracja
    

    private void OnEnable()
    {
        //menu       
        PlayerManager.PlayerSignedIn += DisableLoggingMenu;
        PlayerManager.PlayerSignedUp += DisableLoggingMenu;
    }
    private void OnDisable()
    {
        //menu       
        PlayerManager.PlayerSignedIn -= DisableLoggingMenu;
        PlayerManager.PlayerSignedUp -= DisableLoggingMenu;
    }

    void Start()
    {
        bottomBar = GameObject.Find("BottomBarController");
        manager = GameObject.Find("Manager");
        SManager = manager.GetComponent<SelectManager>();
        RectTransform objectRectTransform = Panel.GetComponent<RectTransform>();
        panelHeight = RtsManager.Current.scaleToResolution(objectRectTransform.rect.height);
        float y = bottomBar.transform.position.y - Screen.height + panelHeight;

        HiddenBottomBar = 0;
        ShowedBottomBar = panelHeight;

        Vector3 startingPosition;
        startingPosition.x = bottomBar.transform.position.x;
        startingPosition.z = bottomBar.transform.position.z;
        startingPosition.y = y;

        bottomBar.transform.position = startingPosition;

        LoggingMenu = GameObject.Find("LoginCanvas");
    }

    public void hideBottomBar()
    {
        hideBottomBarBool = true;

        Stop = false;
    }
    public void showBottomBar()
    {
        hideBottomBarBool = false;
        Stop = false;

    }
    public static bool returnBottomBarStatus()
    {
        return hideBottomBarBool;
    }
    public void hide()
    {
        Vector3 toChange = bottomBar.transform.position;
        
        if (bottomBar.transform.position.y <= HiddenBottomBar)
        {
            toChange.y = HiddenBottomBar;
            bottomBar.transform.position = toChange;
            Stop = true;
            return;
        }

        toChange.y -= Time.deltaTime * 250;
        bottomBar.transform.position = toChange;
    }
    public void show()
    {

        Vector3 toChange = bottomBar.transform.position;
        if (bottomBar.transform.position.y >= ShowedBottomBar)
        {
           
            toChange.y = ShowedBottomBar;
            bottomBar.transform.position = toChange;

            Stop = true;
            return;
        }

        toChange.y += Time.deltaTime * 250;
        bottomBar.transform.position = toChange;
    }
    void Update()
    {

        if (Input.touchCount != 0)
        {
            if (hideBottomBarBool && SManager.bottomMenuBlock && Input.GetTouch(0).phase == TouchPhase.Began) // jeżeli dolny pasek jest ukryty i ktoś rozpoczął klikanie na dolnym menu
            {
                canBeShovedByTouch = true;

            }
            if (canBeShovedByTouch)
            {
                if (Input.GetTouch(0).position.y > panelHeight/10)
                {
                    hideBottomBarBool = false;
                    Stop = false;
                    canBeShovedByTouch = false;
                }
            }
            if (hideBottomBarBool == false && SManager.bottomMenuBlock && Input.GetTouch(0).position.y > 3 * panelHeight / 10 && Input.GetTouch(0).position.y < panelHeight && Input.GetTouch(0).phase == TouchPhase.Began) // jeżeli dolny pasek jest pokaazany i ktoś rozpoczął klikanie na dolnym menu
            {
                startToTouch = Input.GetTouch(0).position;
                canBeHidedByTouch = true;

            }
            if (startToTouch.y - Input.GetTouch(0).position.y > 3*panelHeight/10 && canBeHidedByTouch)
            {
                canBeHidedByTouch = false;
                hideBottomBarBool = true;
                Stop = false;
            }

        }


        if (Stop)
            return;

        if (hideBottomBarBool == true)
        {
            hide();
        }
        else if (hideBottomBarBool == false)
        {
            show();
        }
    }

    private void DisableLoggingMenu()
    {
        LoggingMenu.SetActive(false);
    }

}
