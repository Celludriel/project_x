using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastEffectFactory : MonoBehaviour {

    public CastEffectResolver GetCastEffectResolver(CastEffectEnum castEffect)
    {
        switch(castEffect)
        {
            case CastEffectEnum.NORMAL_DAMAGE: return new NormalDamageCastEffect();
            case CastEffectEnum.RANDOM_DAMAGE: return new RandomDamageCastEffect();
            default: throw new System.Exception("No casteffect found for [" + castEffect + "]");
        }        
    }
}
