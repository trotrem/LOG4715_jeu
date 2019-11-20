using System;
using System.Collections;
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

    [SerializeField]
    int invincibilityTime = 1;

    float invincibleTimeLeft = 0;
    int livesLeft;
    List<GameObject> lifeGauge = new List<GameObject>();

    internal void UpdateSpawnPoint(Vector3 position)
    {
        respawnPoint = position;
    }

    List<GameObject> deathGauge = new List<GameObject>();
    Vector3 initialSpawnPoint;
    Vector3 respawnPoint;


    // Use this for initialization
    void Start () {
        livesLeft = lives;
        initialSpawnPoint = transform.position;
        respawnPoint = initialSpawnPoint;

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
        if (invincibleTimeLeft > 0)
        {
            invincibleTimeLeft -= Time.deltaTime;
        } 
	}

    void OnCollisionEnter(Collision coll)
    {
        // On s'assure de bien être en contact avec le sol
        if ((whatIsHurtful & (1 << coll.gameObject.layer)) == 0)
            return;

        if (invincibleTimeLeft > 0)
            return;

        invincibleTimeLeft = invincibilityTime;
        LooseLife();
    }

    public void LooseLife()
    {
        Debug.Log("OUCH");
        livesLeft--;
        lifeGauge[livesLeft].SetActive(false);
        deathGauge[livesLeft].SetActive(true);

        if (livesLeft > 0)
        {
            transform.SetPositionAndRotation(respawnPoint, transform.rotation);
        } else
        {
            respawnPoint = initialSpawnPoint;
            canvas.transform.Find("GameOver").gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
