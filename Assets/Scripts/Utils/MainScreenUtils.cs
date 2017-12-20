using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreenUtils: MonoBehaviour
{



    public static float scaleToResolution(float x)
    {

        x = x * Screen.height / 720; // 720 - reference resolution


        return x;
    }

    public static Vector3 ScreenPointToMapPosition(Vector2 point)
    {
        var ray = Camera.main.ScreenPointToRay(point);
        RaycastHit hit;
        if (!RtsManager.Credits.MapCollider.Raycast(ray, out hit, Mathf.Infinity))
            return Vector3.zero;


        if (Physics.Raycast(ray, out hit)) return hit.point;
        else
            return Vector3.zero;


    }

    public static Vector3 possibleToMove2D(Vector2 point)
    {
        var ray = Camera.main.ScreenPointToRay(point);
        RaycastHit hit;


        Physics.Raycast(ray, out hit);


        UnityEngine.AI.NavMeshHit Nhit;

        UnityEngine.AI.NavMesh.SamplePosition(hit.point, out Nhit, 5, 1);

        return Nhit.position;


    }
    public static Vector3 possibleToMove3D(Vector3 point)
    {

        UnityEngine.AI.NavMeshHit Nhit;

        UnityEngine.AI.NavMesh.SamplePosition(point, out Nhit, 2, 1);

        return Nhit.position;


    }
    public static bool isNotCloseTo(Vector3 start, Vector3 finish, Vector3 change) // funkcja odwrotna do funkcji w TouchNavigation
    {

        if ((finish.x > start.x - change.x && finish.x < start.x + change.x) && (finish.y > start.y - change.y && finish.y < start.y + change.y) && (finish.z > start.z - change.z && finish.z < start.z + change.z))
        {
            return false; // zwraca false jeśli finish zawiera się w start (+-) change
        }


        return true; // true jeśli wyjdzie się poza zadany obszar
    }
    public static bool isClose(Vector3 position1, Vector3 position2, float distance) // funkcja sprawdzająca czy dwa punkty są blisko siebie
    {

        if (Vector3.Distance(position1, position2) < distance)
            return true;
        else return false;
    }
}
