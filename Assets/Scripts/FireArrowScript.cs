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

    protected void OnTriggerEnter(Collider coll)
    {
        base.OnTriggerEnter(coll);
        if ((WhatIsBurnable & (1 << coll.gameObject.layer)) != 0)
        {

        }
    }
}
