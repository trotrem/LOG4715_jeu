using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArrowScript : ArrowScript
{

    [SerializeField]
    LayerMask WhatIsSticking;

    [SerializeField]
    LayerMask WhatIsDestroy;

    [SerializeField]
    List<Light> lights;

    [SerializeField]
    float illumination_radius = 8f;


    [SerializeField]
    int Damage = 2;

    private void Start()
    {
        foreach (var l in lights)
        {
            l.range = illumination_radius;
        }
    }

    protected override int getDamage()
    {
        return Damage;
    }

    protected override int getWhatIsDestroy()
    {
        return WhatIsDestroy;
    }

    protected override int getWhatIsSticking()
    {
        return WhatIsSticking;
    }

    protected override void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Burnable")
        {
            coll.transform.Find("Burnable").GetComponent<Burnable>().Burn(this);
        }
        base.OnTriggerEnter(coll);
    }
}
