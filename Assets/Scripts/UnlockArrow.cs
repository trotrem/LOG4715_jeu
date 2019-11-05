using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockArrow : MonoBehaviour {

    [SerializeField] ArrowType arrowType;
    [SerializeField] GameObject bow;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.tag == "Player")
        {
            bow.GetComponent<BowController>().PickupArrow(arrowType);
            gameObject.SetActive(false);
        }
    }
}
