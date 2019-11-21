using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosive : MonoBehaviour
{
    public GameObject explosion;
    bool exploded = false;
    MeshRenderer mesh_renderer;
    public float explosionTimer = 3f;
    public float dammageTimer = 0.1f;

    public float startTimer = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        mesh_renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        startTimer -= Time.deltaTime;
        if(exploded){
            explosionTimer -= Time.deltaTime;
            dammageTimer -= Time.deltaTime;
        }
        if(explosionTimer <= 0)
        {
            Destroy(gameObject);
        }
        if(dammageTimer <= 0)
        {
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
    }

    void OnCollisionEnter(Collision c)
    {
        if(startTimer <= 0 && !exploded)
        {
            this.explode();
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if(c.tag == "Player")
        {
            c.gameObject.GetComponent<HealthManager>().LooseLife();
        }
        if(c.gameObject.tag == "Boss") // ou ennemi?
        {
            //c.GetComponent<Boss>().damage()
        }
    }

    void explode()
    {
        exploded = true;
        Debug.Log("BOOM");
        explosion.SetActive(true);
        mesh_renderer.enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
    }
}
