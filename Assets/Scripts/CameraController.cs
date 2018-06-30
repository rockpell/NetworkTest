using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraController : NetworkBehaviour
{
    [SerializeField] GameObject camera;

	// Use this for initialization
	void Start () {
        GameObject mainCamera = GameObject.Find("Main Camera");
        if (mainCamera != null)
            mainCamera.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
        {
            return;
        }

        if(camera != null)
        {
            camera.SetActive(true);
        }
    }

    public override void OnNetworkDestroy()
    {
        Debug.Log("OnNetworkDestroy");
    }
}
