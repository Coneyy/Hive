using UnityEngine;
using System.Collections.Generic;

public class RtsManager : MonoBehaviour
{



    public static RtsManager Current = null;


    public TerrainCollider MapCollider;

    public List<PlayerSetupDefinitions> Players = new List<PlayerSetupDefinitions>();

    public List<GameObject> enemies = new List<GameObject>();

   
    public T[] _2Dto1D<T>(T[,] tab)
    {
        
        T[] _1Dtab = new T[tab.GetLength(0)*tab.GetLength(1)];
        int licznik = 0;
        for(int i=0;i<tab.GetLength(0);i++)
        {
            for(int j=0;j<tab.GetLength(1);j++)
            {
                _1Dtab[licznik++] = tab[i, j];
            }
        }

        return _1Dtab;
    }

    public void setDefaultPlayer(PlayerSetupDefinitions player)
    {
        Player.DefaultPlayer = player;
        BottomPanelConnector.Current.setResourceValue(player.Credits.ToString());

    }


    void Start()
    {

        Current = this;
      
    }

	public void setCredits(int credits)
	{
		Player.DefaultPlayer.Credits = credits;
		BottomPanelConnector.Current.setResourceValue (credits.ToString());
	}
	public int getCredits()
	{
		return Player.DefaultPlayer.Credits;
	}



}
