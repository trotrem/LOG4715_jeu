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

    bool _Flipped { get; set; }

    public Rigidbody _Rb { get; set; }
    public Animator _Anim { get; set; }

    void Awake()
    {
        _Anim = GetComponent<Animator>();
        _Rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //_Anim.SetBool("walk", true);
        _Flipped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (nearEdge())
        {
            FlipDemon();
        }
        Move();
    }

    void Move()
    {
        _Rb.velocity = new Vector3(0.0f, _Rb.velocity.y, walkSpeed);
        _Anim.SetFloat("MoveSpeed", Mathf.Abs(walkSpeed));
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
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, groundDistance, WhatIsGround))
        {
            nearEdge = true;
        }
        return nearEdge;
    }

}
