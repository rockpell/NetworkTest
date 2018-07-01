using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    private Rigidbody myRigidbody;

    // Use this for initialization
    void Start () {
        myRigidbody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 targetPosition = (transform.forward + transform.right);
        targetPosition.y = myRigidbody.velocity.y;
        myRigidbody.velocity = targetPosition;
        myRigidbody.MoveRotation(Quaternion.Euler(0, 0 ,0));
    }
}
