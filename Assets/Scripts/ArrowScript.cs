using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour {

    [SerializeField]
    protected float maxForce = 100000f;

    float h;
    float k;
    float a;

    float u = 0;
    Vector3 initialRot;

    bool initialized = false;

    protected Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.FromToRotation(new Vector3(0,1,0), rigidBody.velocity);
	}

    public void Shoot(Vector3 startPos, Vector3 trajectoryTop, float forcePercentage)
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(Vector3.Normalize(trajectoryTop - startPos) * maxForce * forcePercentage);
    }
}
