using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CastEffectResolver {

    public void ResolveCastEffect(Ship origin, Ship target)
    {
        ResolveVisualEffect(origin, target);
        ResolveDataEffect(origin, target);
    }

    internal abstract void ResolveDataEffect(Ship origin, Ship target);
    internal abstract void ResolveVisualEffect(Ship origin, Ship target);
}
