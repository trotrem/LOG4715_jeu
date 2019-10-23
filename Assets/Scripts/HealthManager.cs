﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    Canvas canvas;

    [SerializeField]
    GameObject heartPrefab;

    [SerializeField]
    GameObject brokenHeartPrefab;

    [Range(1, 10)]
    [SerializeField]
    int lives = 3;

    [SerializeField]
    LayerMask whatIsHurtful;

    int livesLeft;
    List<GameObject> lifeGauge = new List<GameObject>();
    List<GameObject> deathGauge = new List<GameObject>();


    // Use this for initialization
    void Start () {
        livesLeft = lives;

        for (int i = 0; i < lives; i++)
        {
            GameObject heart = Instantiate(heartPrefab, canvas.transform);
            heart.transform.Translate(i * 25, 0, 0);
            lifeGauge.Add(heart);

            GameObject brokenHeart = Instantiate(brokenHeartPrefab, canvas.transform);
            brokenHeart.transform.Translate(i * 25, 0, 0);
            deathGauge.Add(brokenHeart);
            brokenHeart.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnCollisionEnter(Collision coll)
    {
        // On s'assure de bien être en contact avec le sol
        if ((whatIsHurtful & (1 << coll.gameObject.layer)) == 0)
            return;

        LooseLife();
    }

    void LooseLife()
    {
        livesLeft--;
        lifeGauge[livesLeft].SetActive(false);
        deathGauge[livesLeft].SetActive(true);

        if (livesLeft > 0)
        {
            transform.SetPositionAndRotation(new Vector3(0, 1, 0), transform.rotation);
        } else
        {
            canvas.transform.Find("GameOver").gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
}