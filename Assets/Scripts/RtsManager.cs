using UnityEngine;
using System.Collections.Generic;

public class RtsManager : MonoBehaviour
{

    public static RtsManager Current = null;



    public TerrainCollider MapCollider;

    public List<PlayerSetupDefinitions> Players = new List<PlayerSetupDefinitions>();

    public List<GameObject> enemies = new List<GameObject>();

    public float scaleToResolution(float x)
    {

        x = x * Screen.height / 720; // 720 - reference resolution


        return x;
    }

    public Vector3 ScreenPointToMapPosition(Vector2 point)
    {
        var ray = Camera.main.ScreenPointToRay(point);
        RaycastHit hit;
        if (!MapCollider.Raycast(ray, out hit, Mathf.Infinity))
            return Vector3.zero;


        if (Physics.Raycast(ray, out hit)) return hit.point;
        else
            return Vector3.zero;


    }

    public Vector3 possibleToMove2D(Vector2 point)
    {
        var ray = Camera.main.ScreenPointToRay(point);
        RaycastHit hit;


        Physics.Raycast(ray, out hit);


        UnityEngine.AI.NavMeshHit Nhit;

        UnityEngine.AI.NavMesh.SamplePosition(hit.point, out Nhit, 5, 1);

        return Nhit.position;


    }
    public Vector3 possibleToMove3D(Vector3 point)
    {

        UnityEngine.AI.NavMeshHit Nhit;

        UnityEngine.AI.NavMesh.SamplePosition(point, out Nhit, 2, 1);

        return Nhit.position;


    }
    public bool isNotCloseTo(Vector3 start, Vector3 finish, Vector3 change) // funkcja odwrotna do funkcji w TouchNavigation
    {

        if ((finish.x > start.x - change.x && finish.x < start.x + change.x) && (finish.y > start.y - change.y && finish.y < start.y + change.y) && (finish.z > start.z - change.z && finish.z < start.z + change.z))
        {
            return false; // zwraca false jeśli finish zawiera się w start (+-) change
        }


        return true; // true jeśli wyjdzie się poza zadany obszar
    }
    public bool isClose(Vector3 position1, Vector3 position2, float distance) // funkcja sprawdzająca czy dwa punkty są blisko siebie
    {

        if (Vector3.Distance(position1, position2) < distance)
            return true;
        else return false;
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
    void Start()
    {
        GameObject IT = GameObject.Find("ImageTarget");
        float count = 0;
        float number = 0;
        Current = this;
        foreach (var p in Players)
            foreach (var u in p.startingUnits)
            {
                Vector3 temp = p.location.position;
                temp.x += count;
                count += 5f;
                var go = (GameObject)GameObject.Instantiate(u, temp, p.location.rotation);
                var player = go.AddComponent<Player>();
                player.Info = p;
                Player.DefaultPlayer = p;
                go.transform.SetParent(IT.transform);
                number++;
                go.name = "Jednostka: " + p.Name + " nr: " + number;


            }
        number = 0;

    }



    // Update is called once per frame
    void Update()
    {


    }
}
