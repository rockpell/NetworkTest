using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
        {
            return;
        }
        //float x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        //transform.Rotate(0, x, 0);
        //transform.Translate(0, 0, z);
        transform.Translate(x, 0, z);
        MouseControl();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CmdFire();
        }
    }

    [Command]
    private void CmdFire()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 30;
        NetworkServer.Spawn(bullet);
        Destroy(bullet, 2.0f);
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    private void MouseControl()
    {
        float mouseSensitivity = 2.0f;
        float horizontal = Input.GetAxis("Mouse X") * mouseSensitivity;
        //float vertical = Input.GetAxis("Mouse Y") * mouseSensitivity;

        //vertical = Mathf.Clamp(vertical, -80, 80);

        transform.Rotate(0, horizontal, 0);
        
    }
}
