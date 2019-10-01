using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowController : MonoBehaviour {

    [SerializeField]
    Transform ArrowSpawnPoint;

    [SerializeField]
    GameObject ArrowPrefab;

    [SerializeField]
    GameObject BowAxisCenter;

    [SerializeField]
    Transform ProjectileParent;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            ShootArrow();
        }
	}

    void ShootArrow()
    {
        var arrow = Instantiate(ArrowPrefab, ArrowSpawnPoint.position, Quaternion.Euler(90, 0, 0), ProjectileParent);
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            var point = new Vector3(0f, hit.point.y, hit.point.z);
            arrow.GetComponent<ArrowScript>().Shoot(ArrowSpawnPoint.position, point);
            Debug.Log(point);
        }
        else
        {
            Debug.Log("t'as cliqué dans le vide t'es bad!");
        }
        
    }
}
