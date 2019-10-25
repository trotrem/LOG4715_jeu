using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalArrowScript : ArrowScript
{

    [SerializeField]
    LayerMask WhatIsSticking;

    [SerializeField]
    LayerMask WhatIsDestroy;

    protected override int getWhatIsDestroy()
    {
        return WhatIsDestroy;
    }

    protected override int getWhatIsSticking()
    {
        return WhatIsSticking;
    }

    protected override int getDamage()
    {
        return 1;
    }
}
