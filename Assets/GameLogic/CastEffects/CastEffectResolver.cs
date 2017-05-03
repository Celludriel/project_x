using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CastEffectResolver {

    private bool visualEffectCompleted = false;
    private bool dataEffectCompleted = false;
    private Ship origin, target;

    public CastEffectResolver(Ship origin, Ship target)
    {
        this.origin = origin;
        this.target = target;
    }

    public void ResolveCastEffect()
    {
        ResolveVisualEffect(origin, target);
        if (visualEffectCompleted)
        {
            ResolveDataEffect(origin, target);
            dataEffectCompleted = true;
        }
    }

    public void SetVisualEffectCompleted()
    {
        visualEffectCompleted = true;
    }

    public bool IsDataEffectCompleted()
    {
        return dataEffectCompleted;
    }

    public Ship GetOrigin()
    {
        return origin;
    }

    public Ship GetTarget()
    {
        return target;
    }

    internal abstract void ResolveDataEffect(Ship origin, Ship target);
    internal abstract void ResolveVisualEffect(Ship origin, Ship target);
}
