using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{

    public const int maxHealth = 100;
    public bool destroyOnDeath;
    [SyncVar(hook = "OnChangeHealth")]
    public int currnetHealth = maxHealth;

    public RectTransform healthBar;

    private NetworkStartPosition[] spawnPoints;

    void Start()
    {
        if (isLocalPlayer)
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        }
    }

    public void TakeDamage(int amount)
    {
        if (!isServer)
            return;

        currnetHealth -= amount;
        if (currnetHealth <= 0)
        {
            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
            else
            {
                currnetHealth = maxHealth;
                RpcRespawn();
            }
        }
        
    }

    void OnChangeHealth(int value)
    {
        healthBar.sizeDelta = new Vector2(value, healthBar.sizeDelta.y);
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            Vector3 spawnPoint = Vector3.zero;

            if(spawnPoints != null && spawnPoints.Length > 0)
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }
            transform.position = spawnPoint;
        }
    }
}
