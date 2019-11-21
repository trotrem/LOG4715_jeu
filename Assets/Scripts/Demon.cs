using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : Ennemy
{
    [SerializeField]
    float walkSpeed = 5.0f;

    [SerializeField]
    float runSpeed = 15.0f;

    [SerializeField]
    LayerMask WhatIsGround;

    [SerializeField]
    float groundDistance = 0.5f;

    Vector3 LastKnownPlayerPosition;

    bool detected;
    bool _Flipped { get; set; }
    public Rigidbody _Rb { get; set; }
    public Animator _Anim { get; set; }

    void Awake()
    {
        _Anim = GetComponent<Animator>();
        _Rb = GetComponent<Rigidbody>();
        
    }

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        GetComponents<AudioSource>()[0].Play();
        detected = false;
        _Flipped = false;
    }

    // Update is called once per frame
    void Update()
    {
        detectPlayer();
        if (nearEdge() && !detected)
        {
            FlipDemon();
            Move();
        }
        else if (detected)
        {
            _Anim.SetTrigger("run");
            chasePlayer();
        }
        else
        {
            _Anim.SetTrigger("walk");
            Move();
        }
    }

    void Move()
    {
        _Rb.velocity = new Vector3(0.0f, _Rb.velocity.y, walkSpeed * transform.forward.z);
    }

    void chasePlayer()
    {
        Vector3 direction = LastKnownPlayerPosition - transform.position;
        _Rb.velocity = new Vector3(0.0f, _Rb.velocity.y, runSpeed * direction.normalized.z);
    }

    void FlipDemon()
    {
        if (!_Flipped)
        {
            _Flipped = true;
            transform.RotateAround(transform.position, new Vector3(0, 1, 0), 180);
        }
        else if (_Flipped)
        {
            _Flipped = false;
            transform.RotateAround(transform.position, new Vector3(0, 1, 0), -180);
        }
    }

    bool nearEdge()
    {
        bool nearEdge = false;
        RaycastHit hitGround;
        RaycastHit hitWall;
        Physics.Raycast(transform.position, new Vector3(0.0f, -transform.up.y, 5*transform.forward.z), out hitGround, 10.0f, WhatIsGround);
        Physics.Raycast(transform.position, new Vector3(0.0f, 0.0f, 5 * transform.forward.z), out hitWall, 100.0f, WhatIsGround);
        if (hitGround.distance > 2.0f || hitWall.distance < 2.0f)
        {
            nearEdge = true;
        }
        
        return nearEdge;
    }

    bool detectPlayer()
    {
        
        detected = false;
        RaycastHit hit;
        if (Physics.Raycast(transform.position - new Vector3 (0f,0.1f,0f), new Vector3(0.0f,0.0f,transform.forward.z), out hit, 100.0f) && hit.transform.tag == "Player")
        {
            LastKnownPlayerPosition = hit.transform.position;
            detected = true;
            GetComponents<AudioSource>()[1].Play();
        }
        return detected;
    }
}
