using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_thomas : MonoBehaviour
{

    public Thomas_train train;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.GetComponent<HealthManager>().respawnActivated = false;
            GetComponent<AudioSource>().Play();
            train.start = true;
            CameraScript.SetupForBoss();
        }
        
    }
}
