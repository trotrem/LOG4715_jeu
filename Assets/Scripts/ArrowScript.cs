using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ArrowScript : MonoBehaviour {


    [SerializeField]
    LayerMask WhatIsWall;

    [SerializeField]
    LayerMask WhatIsEnemy;

    [SerializeField]
    float maxForce = 800f;

    float h;
    float k;
    float a;

    float u = 0;
    Vector3 initialRot;

    bool initialized = false;

    Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (rigidBody.velocity.magnitude > 0)
            transform.rotation = Quaternion.FromToRotation(new Vector3(0,1,0), rigidBody.velocity);
	}

    public void Shoot(Vector3 startPos, Vector3 trajectoryTop, float forcePercentage)
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(Vector3.Normalize(trajectoryTop - startPos) * maxForce * forcePercentage);
    }

    protected void OnTriggerEnter(Collider coll)
    {
        if ((WhatIsWall & (1 << coll.gameObject.layer)) != 0)
        {
            float time = Time.fixedDeltaTime;
            Vector3 velocity = rigidBody.velocity;
            transform.position = transform.position + -velocity * time;

            GetComponent<BoxCollider>().isTrigger = false;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }

        if ((WhatIsEnemy & (1 << coll.gameObject.layer)) != 0)
        {
            int damage = getDamage();
            //TODO Damage Enemy
        }
    }

    protected abstract int getDamage();
}
