using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ArrowScript : MonoBehaviour {

    [SerializeField]
    LayerMask WhatIsEnemy;

    [SerializeField]
    float maxForce = 800f;

    BowController bowController;

    float h;
    float k;
    float a;

    float u = 0;
    Vector3 initialRot;

    bool initialized = false;
    public bool stuck = false;

    protected Rigidbody rigidBody;
    private Guid guid;

    private float lifespan = 0;

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!stuck && rigidBody.velocity.magnitude > 0)
        {
            rigidBody.AddForce(-Physics.gravity / 2);
        }
    }

    // Update is called once per frame
    void Update () {
        lifespan += Time.deltaTime;
        if (!stuck && rigidBody.velocity.magnitude > 0)
        {
            transform.rotation = Quaternion.FromToRotation(new Vector3(0, 1, 0), rigidBody.velocity);
        }
    }

    public void Shoot(Vector3 startPos, Vector3 trajectoryTop, float forcePercentage)
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(Vector3.Normalize(trajectoryTop - startPos) * maxForce * forcePercentage);
    }

    protected virtual void OnTriggerEnter(Collider coll)
    {
        if (!stuck && (getWhatIsSticking() & (1 << coll.gameObject.layer)) != 0)
        {
            if (lifespan < 0.1)
            {
                Bounce();
                return;
            }
            GetComponents<AudioSource>()[0].Play();
            stuck = true;
            float time = Time.fixedDeltaTime;
            Vector3 velocity = rigidBody.velocity;
            transform.position = transform.position + -velocity * time;
            rigidBody.velocity = Vector3.zero;
            GameObject inbetween = new GameObject();
            inbetween.transform.SetParent(coll.transform);
            transform.SetParent(inbetween.transform, false);
            rigidBody.useGravity = false;
            rigidBody.constraints = RigidbodyConstraints.FreezeAll;
            rigidBody.isKinematic = true;
        }

        if (!stuck && (getWhatIsDestroy() & (1 << coll.gameObject.layer)) != 0)
        {
            GetComponents<AudioSource>()[0].Play();
            bowController.DestroyArrow(guid);
        }

        if (!stuck && (WhatIsEnemy & (1 << coll.gameObject.layer)) != 0)
        {
            GetComponents<AudioSource>()[0].Play();
            coll.gameObject.GetComponent<Ennemy>().Hit(getDamage());
        }
    }

    private void Bounce()
    {
        GetComponents<AudioSource>()[0].Play();
        rigidBody.velocity = -rigidBody.velocity;
    }

    protected abstract int getDamage();
    protected abstract int getWhatIsDestroy();
    protected abstract int getWhatIsSticking();

    internal void setguid(Guid guid)
    {
        this.guid = guid;
    }

    internal void setParent(BowController bowController)
    {
        this.bowController = bowController;
    }
}
