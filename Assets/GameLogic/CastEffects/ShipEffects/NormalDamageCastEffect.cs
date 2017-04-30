using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalDamageCastEffect : CastEffectResolver
{
    internal override void ResolveDataEffect(Ship origin, Ship target)
    {
        target.health += -20;
    }

    internal override void ResolveVisualEffect(Ship origin, Ship target)
    {
    }
}
