using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastEffectFactory : MonoBehaviour {

    public CastEffectResolver GetCastEffectResolver(CastEffectEnum effectToCast, Ship origin, Ship ship)
    {
        switch (effectToCast)
        {
            case CastEffectEnum.NORMAL_DAMAGE: return new NormalDamageCastEffect(origin, ship);
            case CastEffectEnum.RANDOM_DAMAGE: return new RandomDamageCastEffect(origin, ship);
            default: throw new System.Exception("No casteffect found for [" + effectToCast + "]");
        }
    }
}
