using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowController : MonoBehaviour {

    [SerializeField]
    GameObject ArrowPrefab;

    [SerializeField]
    Transform ProjectileParent;

    [SerializeField]
    float maxHoldTime = 0.5f;

    [SerializeField]
    LayerMask WhatIsInvisibleWall;

    [SerializeField]
    float minAngleUnderHorizon = 30;

    float timer = 0f;
    bool isHoldingMouse;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        PositionBow();
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

    void PositionBow()
    {
        Vector3 parentPosition = transform.parent.position;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100.0f, WhatIsInvisibleWall))
        {
            var distance = new Vector3(0, hit.point.y - parentPosition.y, hit.point.z - parentPosition.z);
            var angle = Mathf.Atan(distance.y / distance.z) / Mathf.PI * 180 + (distance.z < 0 ? 180 : 0);
            if (angle < -minAngleUnderHorizon) angle = -minAngleUnderHorizon;
            if (angle > 180 + minAngleUnderHorizon) angle = 180 + minAngleUnderHorizon;
            transform.parent.SetPositionAndRotation(parentPosition, Quaternion.Euler(-angle, 0, 0));
        }
        else
        {
            Debug.Log("Ta souris est out of bounds");
        }
    }

    void ShootArrow()
    {
        var arrow = Instantiate(ArrowPrefab, transform.position, Quaternion.Euler(90, 0, 0), ProjectileParent);
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float shootingForcePercentage = Mathf.Clamp(Mathf.Lerp(0, 1, timer/maxHoldTime), 0.02f, 1);
        timer = 0;
        if (Physics.Raycast(ray, out hit, 100.0f, WhatIsInvisibleWall))
        {
            var point = new Vector3(0f, hit.point.y, hit.point.z);
            arrow.GetComponent<ArrowScript>().Shoot(transform.position, point, shootingForcePercentage);
        }
        else
        {
            Debug.Log("t'as cliqué dans le vide t'es bad!");
        }
        
    }
}
