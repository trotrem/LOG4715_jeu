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

    [SerializeField]
    float maxHoldTime = 0.5f;

    float timer = 0f;
    bool isHoldingMouse;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            isHoldingMouse = true;
        }
        if (isHoldingMouse)
        {
            timer += Time.deltaTime;
        }
        if (Input.GetMouseButtonUp(0))
        {
            ShootArrow();
            isHoldingMouse = false;
        }
	}

    void ShootArrow()
    {
        var arrow = Instantiate(ArrowPrefab, ArrowSpawnPoint.position, Quaternion.Euler(90, 0, 0), ProjectileParent);
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float shootingForcePercentage = Mathf.Clamp(Mathf.Lerp(0, 1, timer/maxHoldTime), 0.02f, 1);
        timer = 0;
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            var point = new Vector3(0f, hit.point.y, hit.point.z);
            arrow.GetComponent<ArrowScript>().Shoot(ArrowSpawnPoint.position, point, shootingForcePercentage);
            Debug.Log(shootingForcePercentage);
        }
        else
        {
            Debug.Log("t'as cliqué dans le vide t'es bad!");
        }
        
    }
}
