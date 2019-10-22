using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overlord : MonoBehaviour {

    [SerializeField]
    float gravity = -30f;
	// Use this for initialization
	void Start () {
        Physics.gravity = new Vector3(0, gravity, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
