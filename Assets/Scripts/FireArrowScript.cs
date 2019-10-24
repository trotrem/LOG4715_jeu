using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArrowScript : ArrowScript {

    [SerializeField]
    LayerMask WhatIsBurnable;

    protected override int getDamage()
    {
        return 2;
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
