using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hive.Assets.Scripts.Network.PlayerInfrastructure.Models;

public class AndroidIntentReader : MonoBehaviour {


	public void startGame(string name,string email)
	{

		Debug.Log ("AndroidIntentReader START");


		HivePlayer player = new HivePlayer ();
		player.Username = name;
		player.Email = email;
		SessionSingleton.Session.Player = player;
		var manager=GetComponent<NetworkManager> ();

		manager.Connect ();
	}


     void Start () {
		

#if UNITY_ANDROID && !UNITY_EDITOR

		   AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
		AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

		AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");
		bool hasUsername = intent.Call<bool> ("hasExtra", "USERNAME");



		bool hasEmail = intent.Call<bool> ("hasExtra", "EMAIL");

		if (hasUsername && hasEmail) {
			AndroidJavaObject extras = intent.Call<AndroidJavaObject> ("getExtras");
			string email = extras.Call<string> ("getString", "EMAIL");
			string username = extras.Call<string> ("getString", "USERNAME");
		
			startGame(username,email);
		}
		else 
		{
		startTestGame();
		}



#endif

		#if UNITY_ANDROID && UNITY_EDITOR
	
		startTestGame();

		#endif


    }


	void startTestGame ()
	{
		string name="test";
		string email = "test@o2.pl";

		startGame (name, email);
	}
}
