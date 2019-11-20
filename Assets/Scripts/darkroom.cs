using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class darkroom : MonoBehaviour
{
    Color col;
    bool isPlayerInsideDarkRoom;
    GameObject player;
    public float Cutoff = 5f;
    // Start is called before the first frame update
    void Start()
    {
        col = RenderSettings.ambientLight;
    }

    // Update is called once per frame
    void Update()
    {

        if (isPlayerInsideDarkRoom){
            var dist = Mathf.Abs(player.transform.position.z - gameObject.transform.position.z);
            if(dist > 35 - Cutoff){
                var factor = 1 - (35 - dist) / Cutoff;

                RenderSettings.ambientLight = new Color(col.r * factor, col.g * factor, col.b * factor, col.a);
            }
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {
            player = c.gameObject;
            isPlayerInsideDarkRoom = true;
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {
            isPlayerInsideDarkRoom = false;
            RenderSettings.ambientLight = col;
        }
    }
}
