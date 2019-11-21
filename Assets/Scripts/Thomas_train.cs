using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thomas_train : MonoBehaviour
{
    [SerializeField] Canvas canvas;

    public GameObject thomas;
    public GameObject bomb;

    public int lives = 3;

    private float nextActionTime = 3f;
    public float period = 0.1f;

    private Vector3 startPos;
    private Vector3 endPos;
    private float distance = 15f;

    private float lerpTime = 3;
    private float currentLerpTime = 0;
    private float lerpTimeRotation = 25;
    private float currentLerpTimeRotation = 0;

    static public bool start = false;
    private int state = 0;
    [SerializeField]
    public LayerMask WhatIsHurt;

    // Start is called before the first frame update
    void Start()
    {
        startPos = thomas.transform.position;
        endPos = thomas.transform.position + Vector3.back * distance;
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            if (Time.time > nextActionTime)
            {
                nextActionTime += period;
                // execute block of code here
                spawnBomb();
            }
            if (state == 0)
            {
                currentLerpTime += Time.deltaTime;
                if (currentLerpTime >= lerpTime)
                {
                    currentLerpTime = lerpTime;
                    state = 1;
                    currentLerpTimeRotation = 0;
                }

                float perc = currentLerpTime / lerpTime;
                thomas.transform.position = Vector3.Lerp(startPos, endPos, perc);
            }
            else if (state == 1)
            {
                currentLerpTimeRotation += Time.deltaTime;
                float perc = currentLerpTimeRotation / lerpTimeRotation;
                thomas.transform.rotation = Quaternion.Lerp(thomas.transform.rotation, Quaternion.Euler(0, 0, 0), perc);
                if (thomas.transform.rotation == Quaternion.Euler(0, 0, 0))
                {
                    state = 2;
                    currentLerpTime = 0;
                    startPos = thomas.transform.position;
                    endPos = thomas.transform.position + Vector3.forward * distance;
                }
            }
            else if (state == 2)
            {
                currentLerpTime += Time.deltaTime;
                if (currentLerpTime >= lerpTime)
                {
                    currentLerpTime = lerpTime;
                    state = 3;
                    currentLerpTimeRotation = 0;
                }

                float perc = currentLerpTime / lerpTime;
                thomas.transform.position = Vector3.Lerp(startPos, endPos, perc);
            }
            else if (state == 3)
            {
                currentLerpTimeRotation += Time.deltaTime;
                float perc = currentLerpTimeRotation / lerpTimeRotation;
                thomas.transform.rotation = Quaternion.Lerp(thomas.transform.rotation, Quaternion.Euler(0, 180, 0), perc);
                if (thomas.transform.rotation == Quaternion.Euler(0, -180, 0))
                {
                    state = 0;
                    currentLerpTime = 0;
                    startPos = thomas.transform.position;
                    endPos = thomas.transform.position + Vector3.back * distance;
                }
            }
        }
    }

    public void spawnBomb()
    {
        Instantiate(bomb, this.transform.position - Vector3.up * 2.5f , this.transform.rotation);
    }

    //quand thomas se fait attaquer par une bombe
    void OnCollisionEnter(Collision coll)
    {
        if ((WhatIsHurt & (1 << coll.gameObject.layer)) == 0)
            return;

        LooseLife();
    }

    void LooseLife()
    {
        lives--;

        if (lives <= 0)
        {
            canvas.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
