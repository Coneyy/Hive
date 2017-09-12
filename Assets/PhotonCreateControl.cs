using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonCreateControl : MonoBehaviour
{
    public MeshRenderer[] Renderes;
    private Material material;
    public Material materialBlue;
    public Material materialRed;
    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        PhotonView photonView = GetComponent<PhotonView>();


        if (photonView.isMine)
        {
            material = materialBlue;
        }
        else
        {
            material = materialRed;
            Renderer[] rs = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in rs)
                r.enabled = false;
        }

        foreach (var r in Renderes)
        {
            r.material = material;
        }

    }
}
