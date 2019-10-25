using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour {

    [SerializeField]
    int lives = 2;

    int livesleft;

    MeshRenderer renderer;

    // Use this for initialization
    void Start () {
        livesleft = lives;
        renderer = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    internal void Hit(int damage)
    {
        renderer.material.color = Color.red;
        livesleft -= damage;
        if (livesleft <= 0)
        {
            Destroy(this.gameObject);
        }
    }

}
