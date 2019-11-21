using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_trajectory : MonoBehaviour
{

    public Rigidbody rb;
    private float trajectoire = 0;
    public int vitesseBombe;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        trajectoire = GameObject.Find("thomas").transform.position.z - GameObject.FindGameObjectWithTag("Player").transform.position.z;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(trajectoire <= 0)
        {
            rb.AddForce(Vector3.forward * vitesseBombe);
        }
        else
        {
            rb.AddForce(Vector3.back * vitesseBombe);
        }
    }
}
