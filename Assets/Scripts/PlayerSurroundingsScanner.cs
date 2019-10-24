﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSurroundingsScanner : MonoBehaviour
{
    [SerializeField]
    Rigidbody rigidBody;

    [SerializeField]
    PlayerControler player;

    [SerializeField]
    Transform feet;

    HashSet<Collider> colliding_arrows = new HashSet<Collider>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var arrow in colliding_arrows)
        {
            if (rigidBody.velocity.y < 0 || feet.position.y > arrow.transform.position.y || (transform.position.y > arrow.transform.position.y && player._Grounded))
            {
                arrow.isTrigger = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Arrow")
        {
            colliding_arrows.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Arrow")
        {
            colliding_arrows.Remove(other);
            other.isTrigger = true;
        }
    }
}