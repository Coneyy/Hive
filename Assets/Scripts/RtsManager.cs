using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class RtsManager : MonoBehaviour
{
    public static RtsManager Credits = null;
    
    public TerrainCollider MapCollider;

    public List<PlayerSetupDefinitions> Players = new List<PlayerSetupDefinitions>();

    public List<GameObject> enemies = new List<GameObject>();

    public static int Gold 
    {
        get
        {
            return Player.DefaultPlayer.Credits;
        }
        set
        {
            Player.DefaultPlayer.Credits = value;
            BottomPanelConnector.Current.setResourceValue(Credits.ToString());
        }
    }

    public static int Points
    {
        get
        {
            var myObjects = GameObject.FindObjectsOfType(typeof(PhotonView))
                                       .Where(o => ((PhotonView)o).isMine);

            return myObjects.Count()*100 + Player.DefaultPlayer.Credits;
        }
    }
   
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
        Credits = this;      
    }
}
