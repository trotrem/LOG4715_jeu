﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowController : MonoBehaviour {

    [SerializeField]
    GameObject NormalArrowPrefab;

    [SerializeField]
    GameObject FireArrowPrefab;

    [SerializeField]
    Transform ProjectileParent;

    [SerializeField]
    float maxHoldTime = 0.5f;

    [SerializeField]
    LayerMask WhatIsInvisibleWall;

    [SerializeField]
    float minAngleUnderHorizon = 30;

    [SerializeField]
    GameObject ArrowSelectionGUI;

    [SerializeField]
    float capsuleRadius = 1f;

    [SerializeField]
    float forceLength = 10f;

    [SerializeField]
    float KnockBackForce = 15f;

    [SerializeField]
    PlayerControler playerControler;

    [SerializeField]
    GameObject lightSource;

    [SerializeField]
    Color chargedColor;

    [SerializeField]
    float fireArrowSelectedLightRadius = 2f;

    [SerializeField]
    Light fireArrowSelectedLight;

    MeshRenderer renderer;
    [SerializeField]
    Color startColor;

    ArrowType selectedArrow = ArrowType.NORMAL;

    Queue<Tuple<Guid, GameObject>> normalArrows = new Queue<Tuple<Guid, GameObject>>();
    Queue<Tuple<Guid, GameObject>> fireArrows = new Queue<Tuple<Guid, GameObject>>();


    List<ArrowType> unlockedArrows = new List<ArrowType>();


    float timer = 0f;
    bool isHoldingMouse;
	// Use this for initialization
	void Start () {
        unlockedArrows.Add(ArrowType.NORMAL);
        ArrowSelectionGUI.SetActive(true);
        ArrowSelectionGUI.GetComponent<ArrowSelectionGUI>().Select(selectedArrow);
        renderer = GetComponent<MeshRenderer>();
        fireArrowSelectedLight.range = fireArrowSelectedLightRadius;
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

        renderer.material.SetColor("_Color", Color.Lerp(startColor, chargedColor, timer / maxHoldTime));

        float scroll = Input.mouseScrollDelta.y;
        
        if (scroll != 0)
        {
            selectedArrow = unlockedArrows[(unlockedArrows.IndexOf(selectedArrow) + Mathf.FloorToInt(scroll) + unlockedArrows.Count) % unlockedArrows.Count];
            SwitchArrowType(selectedArrow);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            selectedArrow = unlockedArrows[(unlockedArrows.IndexOf(selectedArrow) + 1) % unlockedArrows.Count];
            SwitchArrowType(selectedArrow);
        }
    }

    void SwitchArrowType(ArrowType type)
    {
        ArrowSelectionGUI.GetComponent<ArrowSelectionGUI>().Select(selectedArrow);
        if (selectedArrow == ArrowType.FIRE)
        {
            lightSource.SetActive(true);
        }
        else
        {
            lightSource.SetActive(false);
        }
    }

    internal void DestroyArrow(Guid id)
    {
        int c;
        c = fireArrows.Count;
        for (int i = 0; i < c; i++)
        {
            Tuple<Guid, GameObject> arrow = fireArrows.Dequeue();
            if (arrow.Item1 == id)
            {
                Destroy(arrow.Item2.gameObject);
            }
            else
            {
                fireArrows.Enqueue(arrow);
            }
        }
        c = normalArrows.Count;
        for (int i = 0; i < c; i++)
        {
            Tuple<Guid, GameObject> arrow = normalArrows.Dequeue();
            if (arrow.Item1 == id)
            {
                Destroy(arrow.Item2.gameObject);
            }
            else
            {
                normalArrows.Enqueue(arrow);
            }
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

    internal void PickupArrow(ArrowType arrowType)
    {
        if (!unlockedArrows.Contains(arrowType))
            unlockedArrows.Add(arrowType);

        selectedArrow = arrowType;
        this.SwitchArrowType(arrowType);
    }

    void ShootArrow()
    {
        float shootingForcePercentage = Mathf.Clamp(Mathf.Lerp(0, 1, timer / maxHoldTime), 0.02f, 1);
        timer = 0;
        if (selectedArrow != ArrowType.WIND)
        {
            GameObject arrow = Instantiate(
                selectedArrow == ArrowType.NORMAL ? NormalArrowPrefab : FireArrowPrefab,
                transform.position, Quaternion.Euler(90, 0, 0), ProjectileParent
                );
            Guid id = Guid.NewGuid();
            arrow.GetComponent<ArrowScript>().setguid(id);
            arrow.GetComponent<ArrowScript>().setParent(this);
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, WhatIsInvisibleWall))
            {
                var point = new Vector3(0f, hit.point.y, hit.point.z);
                arrow.GetComponent<ArrowScript>().Shoot(transform.position, point, shootingForcePercentage);

                if (selectedArrow == ArrowType.NORMAL)
                {
                    if (normalArrows.Count >= 3)
                        Destroy(normalArrows.Dequeue().Item2);
                    normalArrows.Enqueue(new Tuple<Guid, GameObject>(id, arrow));
                }
                else
                {
                    if (fireArrows.Count >= 3)
                        Destroy(fireArrows.Dequeue().Item2);
                    fireArrows.Enqueue(new Tuple<Guid, GameObject>(id, arrow));
                }
            }
            else
            {
                Debug.Log("t'as cliqué dans le vide t'es bad!");
            }
        } else
        {
            playerControler.kinematicTimer = 0.05f;
            playerControler._Rb.isKinematic = false;
            RaycastHit direction;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out direction, 100.0f, WhatIsInvisibleWall))
            {
                Vector3 mousePosition = new Vector3(0f, direction.point.y, direction.point.z);
                Vector3 playerPosition = new Vector3(0f, transform.parent.parent.position.y, transform.parent.parent.position.z);
                Vector3 vector = transform.parent.position - mousePosition;
                playerControler.knockBack(vector.normalized, shootingForcePercentage);
                KnockBack(shootingForcePercentage);
                transform.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                Debug.Log("t'as cliqué dans le vide t'es bad!");
            }
        }

        void KnockBack(float forcePercentage)
        {
            Collider[] hitColliders = Physics.OverlapCapsule(transform.position, transform.forward*forceLength, capsuleRadius);
            int i = 0;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].gameObject.tag == "Player")
                {
                    i++;
                    continue;
                }
                Rigidbody objectPushed;
                hitColliders[i].TryGetComponent(out objectPushed);
                if (objectPushed != null)
                {
                    Vector3 pushDirection = objectPushed.transform.position - transform.parent.position;
                    objectPushed.AddForce(pushDirection.normalized * KnockBackForce * forcePercentage * hitColliders[i].gameObject.GetComponent<Rigidbody>().mass, ForceMode.Impulse);
                }  
                i++;
            }
        }
    }
}
