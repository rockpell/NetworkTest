using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    private GameObject mainCamera;
    private int spawnIndex = 0;
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopHost();
        }
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        Debug.Log("A client connected to the server: " + conn);
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        NetworkServer.DestroyPlayersForConnection(conn);

        if (conn.lastError != NetworkError.Ok)
        {

            if (LogFilter.logError) { Debug.LogError("ServerDisconnected due to error: " + conn.lastError); }

        }

        Debug.Log("A client disconnected from the server: " + conn);

    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        Vector3 spawnPosition = Vector3.zero;
        if (playerSpawnMethod == PlayerSpawnMethod.Random)
        {
            spawnPosition = startPositions[Random.Range(0, startPositions.Count)].position;
        } else
        {
            if (spawnIndex >= startPositions.Count) spawnIndex = 0;
            spawnPosition = startPositions[spawnIndex++].position;
        }
        var player = (GameObject)GameObject.Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        Debug.Log("Client has requested to get his player added to the game");
    }


    public override void OnStopServer()
    {
        Debug.Log("Server has stopped");
    }

    public override void OnStopHost()
    {
        Debug.Log("Host has stopped");
        if(mainCamera != null)
            mainCamera.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        Debug.Log("OnClientConnect");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        Debug.Log("OnClientDisconnect");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
