using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour {

    [SerializeField]
    float speed = 10f;

    float h;
    float k;
    float a;

    float u = 0;
    Vector3 startPos;

    bool initialized = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (initialized)
        {
            u += Time.deltaTime * speed;
            transform.position = new Vector3(0f, a * Mathf.Pow(u - h, 2) + k + startPos.y, u + startPos.z);
        }
	}

    public void Shoot(Vector3 startPos, Vector3 trajectoryTop)
    {
        this.startPos = startPos;
        h = trajectoryTop.z - startPos.z;
        k = trajectoryTop.y - startPos .y;
        a = (- k) / Mathf.Pow(- h, 2);
        initialized = true;
    }
}
