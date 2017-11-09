using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : Photon.PunBehaviour
{

    #region Public Variables
    public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;

    /// <summary>
    /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
    /// </summary>   
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    public byte MaxPlayersPerRoom = 2;
   

    public GameObject prefab;
//    public GameObject baseprefab;
    public GameObject spawnPoint;
    public GameObject spawnPoint2;
    #endregion


    #region Private Variables
	GameObject manager;
	FogOfWar fog;

    /// <summary>
    /// This client's version number. Users are separated from each other by gameversion (which allows you to make breaking changes).
    /// </summary>
    string _gameVersion = "1";
    bool isPlayerOne = false;

    #endregion


    #region MonoBehaviour CallBacks


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
    {
		manager = GameObject.Find ("Manager");
		fog = manager.GetComponent<FogOfWar>();

        // #NotImportant
        // Force LogLevel
        PhotonNetwork.logLevel = Loglevel;

        // #Critical
        // we don't join the lobby. There is no need to join a lobby to get the list of rooms.
        PhotonNetwork.autoJoinLobby = false;


        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.automaticallySyncScene = true;
    }


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
    void Start()
    {
        Connect();
        // progressLabel.SetActive(false);
        // controlPanel.SetActive(true);
    }


    public override void OnConnectedToMaster()
    {


        Debug.Log("DemoAnimator/Launcher: OnConnectedToMaster() was called by PUN");
        // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnPhotonRandomJoinFailed()  
        PhotonNetwork.JoinRandomRoom();

    }


    public override void OnDisconnectedFromPhoton()
    {

       
        Debug.LogWarning("DemoAnimator/Launcher: OnDisconnectedFromPhoton() was called by PUN");
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("DemoAnimator/Launcher:OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions() {MaxPlayers =" + MaxPlayersPerRoom + "}, null);");

        isPlayerOne = true;
        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = MaxPlayersPerRoom }, null);

    }

    public override void OnJoinedRoom()
    {
        Debug.Log("DemoAnimator/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");


        if (prefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            Debug.Log("We are Instantiating LocalPlayer from " + Application.loadedLevelName);
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            GameObject Manager = GameObject.Find("Manager");
            FogOfWar fog = Manager.GetComponent<FogOfWar>();
            if (isPlayerOne)
            {
				Player.DefaultPlayer = RtsManager.Current.Players [0];
                

				var go = PhotonNetwork.Instantiate("Photon_prefabs/NotAnimated/"+this.prefab.name, spawnPoint2.transform.position, Quaternion.identity, 0);
                ShowUnitInfo info = go.GetComponent<ShowUnitInfo>();
                info.create("Remigiusz", "WARRIORANT");
                if (info.photonView.isMine)
                    fog.addRevealer(go);
                

                //
                //   var motherBase = PhotonNetwork.Instantiate(this.baseprefab.name, spawnPoint.transform.position, Quaternion.identity, 0);
                // fog.setBuilding(motherBase);
                //   ShowUnitInfo baseInfo = motherBase.GetComponent<ShowUnitInfo>();
                //  baseInfo.create("Baza", "MOTHERBASE");
                //  fog.addRevealer(motherBase);
                // fog.addRevealer(motherBase);


            }
            else
            {
				Player.DefaultPlayer = RtsManager.Current.Players [1];


                var go = PhotonNetwork.Instantiate(this.prefab.name, spawnPoint.transform.position, Quaternion.identity, 0);
                ShowUnitInfo info = go.GetComponent<ShowUnitInfo>();
                info.create("Arkadiusz", "ANT");
                if (info.photonView.isMine)
                    fog.addRevealer(go);
                else
                    RtsManager.Current.enemies.Add(go);


                Vector3 spawnPoint2x = new Vector3(spawnPoint.transform.position.x + 30, spawnPoint.transform.position.y, spawnPoint.transform.position.z);

                var go2 = PhotonNetwork.Instantiate(this.prefab.name, spawnPoint2x, Quaternion.identity, 0);
                ShowUnitInfo info2 = go2.GetComponent<ShowUnitInfo>();
                info2.create("Arkadiusz 2", "ANT");
                if (info2.photonView.isMine)
                    fog.addRevealer(go2);
                else
                    RtsManager.Current.enemies.Add(go2);
            }
            

            //Vector3 spawnPoint2 = new Vector3(spawnPoint.transform.position.x + 30, spawnPoint.transform.position.y, spawnPoint.transform.position.z);
            //var go2 = PhotonNetwork.Instantiate(this.prefab.name, spawnPoint2, Quaternion.identity, 0);
            //ShowUnitInfo info2 = go.GetComponent<ShowUnitInfo>();
            //info.create("Mrowka2", "ANT");




            //   var player = go.AddComponent<Player>();
            //   if (isPlayerOne)
            //   {

            //       player.Info = RtsManager.Current.Players[0];
            //        Debug.Log("Gracz numer 1, tworzymy mrowke");

            //     }
            //    else
            //     {
            //         player.Info = RtsManager.Current.Players[1];
            //          Debug.Log("Gracz numer 2, tworzymy mrowke");

            //      }

        }
    }

    #endregion


    #region Public Methods


    /// <summary>
    /// Start the connection process. 
    /// - If already connected, we attempt joining a random room
    /// - if not yet connected, Connect this application instance to Photon Cloud Network
    /// </summary>
    public void Connect()
    {

        // progressLabel.SetActive(true);
        // controlPanel.SetActive(false);
        Debug.Log("Popopo");

        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.connected)
        {
            Debug.Log("Połaczono z masterem.");

            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnPhotonRandomJoinFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings(_gameVersion);
        }



    }

	public void SpawnNewUnit(Vector3 position)
	{
		if (PhotonNetwork.connected) 
		{
			var go = PhotonNetwork.Instantiate(this.prefab.name, position, Quaternion.identity, 0);
			ShowUnitInfo info = go.GetComponent<ShowUnitInfo>();
			info.create("Arkadiusz", "ANT");
			if (info.photonView.isMine)
				fog.addRevealer(go);
			else
				RtsManager.Current.enemies.Add(go);
		}
	}


    #endregion

}
