using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDamageCastEffect : CastEffectResolver
{
    public RandomDamageCastEffect(Ship origin, Ship target) : base(origin, target)
    {
    }

    internal override void ResolveDataEffect(Ship origin, Ship target)
    {
        target.health += -(UnityEngine.Random.Range(10, 30));
    }

    internal override void ResolveVisualEffect(Ship origin, Ship target)
    {
        SetVisualEffectCompleted();
    }
}
