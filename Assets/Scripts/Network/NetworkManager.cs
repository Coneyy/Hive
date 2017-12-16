using Hive.Assets.Scripts.Network.PlayerInfrastructure;
using Hive.Assets.Scripts.Network.PlayerInfrastructure.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : Photon.PunBehaviour
{

	#region Events 
	public static event GameStart GameStarted;
	public static event RoomJoin RoomJoined;
	public static event ObtainPlayerName PlayersNamesObtained;
	public delegate void GameStart();
	public delegate void RoomJoin();
	public delegate void ObtainPlayerName(string playerName, string opponentName);

	#endregion

	#region Public Variables
	public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;

	/// <summary>
	/// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
	/// </summary>   
	[Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
	public byte MaxPlayersPerRoom = 2;


	public GameObject antPrefab;
	public GameObject buildingPrefab;

	public GameObject spawnPoint;
	public GameObject spawnPoint2;
	#endregion
	public bool DEVELOPMENT_ENV = false;


	#region Private Variables


	PhotonView photonView;
	GameObject manager;
	FogOfWar fog;
	BuildingManager buildingManager;

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

		Debug.Log ("MANAGER AWAKE");
		photonView = PhotonView.Get(this);
		manager = GameObject.Find ("Manager");
		fog = manager.GetComponent<FogOfWar>();
		buildingManager = manager.GetComponent<BuildingManager> ();

		//subskrpcja eventu dołączenia do pokoju innej osoby
		PhotonNetwork.OnEventCall += PunEvent;

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
	//void Start()
	//{
	//    Connect();
	//}


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

	//trzeba dodac skipowanie spawnowania sie jednostek i brak mozliwosci interakcji + ekran "czekanie na drugiego gracza" jak sie jest samemu, 
	// ponadto metode RPC ktora będzie wywoływana podczas połączenia sie z pokojem i która sprawi, że odblokują się interakcje i zespawują jednostki

	public override void OnJoinedRoom()
	{
		if(RoomJoined!= null)
		{
			RoomJoined();
		}

		if (!DEVELOPMENT_ENV)
		{
			if (PhotonNetwork.room.playerCount != 1)
			{
				CallEventThatIHaveJoinedRoom();
				StartGame();
			}
			else
			{
				//Wyswietl informacje "Oczekiwanie na przeciwnika"
			}
		}
		else
		{
			StartGame();
		}
	}

	public void StartGame()
	{
		Debug.Log ("START");


		if (GameStarted != null)
		{
			GameStarted();
		}
		//Wyslij inormacje o swoim imieniu przez photon rpc       
		photonView.RPC("ReceiveMessageFromOpponent", PhotonTargets.Others, SessionSingleton.Session.Player.Username);

		Debug.Log("DemoAnimator/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");


		if (antPrefab == null)
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
				RtsManager.Current.setDefaultPlayer(RtsManager.Current.Players[0]);

				Vector3 buildingSpawn = new Vector3(spawnPoint2.transform.position.x + 30, spawnPoint2.transform.position.y, spawnPoint2.transform.position.z);


				var go = PhotonNetwork.Instantiate("Photon_prefabs/NotAnimated/"+this.antPrefab.name, spawnPoint2.transform.position, Quaternion.identity, 0);
				ShowUnitInfo info = go.GetComponent<ShowUnitInfo>();
				info.create("Remigiusz", ShowUnitInfo.TYPE.WARRIORANT);
				if (info.photonView.isMine)
					fog.addRevealer(go);


				SpawnBuilding (buildingSpawn);


			}
			else
			{
				RtsManager.Current.setDefaultPlayer(RtsManager.Current.Players[1]);

				var go = PhotonNetwork.Instantiate("Photon_prefabs/NotAnimated/" + this.antPrefab.name, spawnPoint.transform.position, Quaternion.identity, 0);
				ShowUnitInfo info = go.GetComponent<ShowUnitInfo>();
				info.create("Arkadiusz", ShowUnitInfo.TYPE.ANT);
				if (info.photonView.isMine)
					fog.addRevealer(go);
				else
					RtsManager.Current.enemies.Add(go);


				Vector3 spawnPoint2x = new Vector3(spawnPoint.transform.position.x + 30, spawnPoint.transform.position.y, spawnPoint.transform.position.z);



				SpawnBuilding (spawnPoint2x);
			}

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


	private void PunEvent(byte eventcode, object content, int senderid)
	{
		switch (eventcode)
		{
		case 0:
			{
				//Pobierz dane przeciwnika
				//Zamknij informacje o czekaniu na przeciwnika
				StartGame();
				break;
			}
		}

	}

	private void CallEventThatIHaveJoinedRoom()
	{
		var options = new RaiseEventOptions();
		options.Receivers = ReceiverGroup.Others;
		PhotonNetwork.RaiseEvent(0, null, true, options);
	}

	[PunRPC]
	private void ReceiveMessageFromOpponent(string username)
	{
		var playerName = SessionSingleton.Session.Player.Username;
		var opponentName = username;

		if (PlayersNamesObtained != null)
		{
			PlayersNamesObtained(playerName, username);
		}
	}
		

	public void SpawnNewUnit(Vector3 position,string name, ShowUnitInfo.TYPE type)
	{
		Spawn (position, name, type, antPrefab.name);
	}
	public void SpawnBuilding(Vector3 position)
	{
		GameObject building=Spawn (position, "MOTHERBASE", ShowUnitInfo.TYPE.MOTHERBASE, buildingPrefab.name);
		buildingManager.Building = building;
	}

	public GameObject Spawn(Vector3 position,string name, ShowUnitInfo.TYPE type,string prefabName)
	{
		GameObject go = null;
		if (PhotonNetwork.connected) 
		{
			go = PhotonNetwork.Instantiate("Photon_prefabs/NotAnimated/"+prefabName, position, Quaternion.identity, 0);
			ShowUnitInfo info = go.GetComponent<ShowUnitInfo>();
			info.create(name, type);
			if (info.photonView.isMine)
				fog.addRevealer(go);
			else
				RtsManager.Current.enemies.Add(go);
		}
		return go;
	}


	#endregion

}
