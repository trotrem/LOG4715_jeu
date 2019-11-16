using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autosave : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Update is called once per frame
    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {
            c.gameObject.GetComponent<PlayerControler>().Autosave(new Vector3(0, gameObject.transform.position.y, gameObject.transform.position.z));
        }
    }
}
