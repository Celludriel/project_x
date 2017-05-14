using UnityEngine;

public class CastEffectFactory : MonoBehaviour {

    public GameContext gameContext;

    public CastEffectResolver GetCastEffectResolver(CastEffectEnum effectToCast, Ship origin)
    {
        return this.GetCastEffectResolver(effectToCast, origin, null);
    }

    public CastEffectResolver GetCastEffectResolver(CastEffectEnum effectToCast, Ship origin, Ship target)
    {
        CastEffectResolver retValue = null;
        switch (effectToCast)
        {
            case CastEffectEnum.NORMAL_DAMAGE: retValue = new NormalDamageCastEffect(origin);break;
            case CastEffectEnum.RANDOM_DAMAGE: retValue = new RandomDamageCastEffect(origin); break;
            case CastEffectEnum.CRASH_DAMAGE: retValue = new CrashDamageCastEffect(origin, target);break;
            default: throw new System.Exception("No casteffect found for [" + effectToCast + "]");
        }
        retValue.gameContext = gameContext;
        return retValue;
    }
}
