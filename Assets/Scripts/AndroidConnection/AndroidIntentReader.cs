using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hive.Assets.Scripts.Network.PlayerInfrastructure.Models;
using Hive.Assets.Scripts.Network.PlayerInfrastructure;

public class AndroidIntentReader : MonoBehaviour {

    SessionService sessionService = new SessionService();

	public void startGame(string name,string email)
	{

		Debug.Log ("AndroidIntentReader START");


		HivePlayer player = new HivePlayer ();
		player.Username = name;
		player.Email = email;
        sessionService.UpdateCurrentSession(player);
		var manager=GetComponent<NetworkManager> ();

        Debug.Log("Session before connecting: " + SessionSingleton.Session.Player.Username);
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
            string name="example";
		    string email = "example@example.com";

		    startGame (name, email);
        }



#endif

#if UNITY_ANDROID && UNITY_EDITOR


        string name ="test";
		string email = "test@o2.pl";

		startGame (name, email);

		#endif


    }


}
