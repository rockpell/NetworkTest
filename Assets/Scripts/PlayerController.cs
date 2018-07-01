using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    private Vector3 realPosition = Vector3.zero;
    private Quaternion realRotation;
    private Vector3 sendPosition = Vector3.zero;
    private Quaternion sendRotation;

    private Rigidbody myRigidbody;

    // Use this for initialization
    void Start () {
        myRigidbody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
        {
            return;
        }
        //float x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f * 1;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f * 2;

        //transform.Rotate(0, x, 0);
        //transform.Translate(0, 0, z);
        //transform.Translate(x, 0, z);

        //myRigidbody.MovePosition(transform.position + transform.forward * z + transform.right * x);

        Vector3 targetPosition = (transform.forward * z + transform.right * x) * 60f;
        targetPosition.y = myRigidbody.velocity.y;
        myRigidbody.velocity = targetPosition;

        //myRigidbody.AddForce(transform.forward * z + transform.right * x * 10f);
        MouseControl();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CmdFire(bulletSpawn.position, bulletSpawn.rotation);
        }
    }

    [Command]
    private void CmdFire(Vector3 spawnPosition, Quaternion spawnRotation)
    {
        //GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, spawnRotation);
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

        //transform.Rotate(0, horizontal, 0);
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, horizontal, 0));
        myRigidbody.MoveRotation(myRigidbody.rotation * deltaRotation);
    }
}
