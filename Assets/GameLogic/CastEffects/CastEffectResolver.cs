using System.Collections.Generic;

public abstract class CastEffectResolver {

    public GameContext gameContext;

    private bool visualEffectCompleted = false;
    private bool dataEffectCompleted = false;
    private Ship origin, target;

    protected List<Ship> targets = new List<Ship>();

    public CastEffectResolver(Ship origin)
    {
        this.origin = origin;
    }

    public CastEffectResolver(Ship origin, Ship target)
    {
        this.origin = origin;
        this.target = target;
    }

    public void ResolveCastEffect()
    {
        ResolveVisualEffect(origin, target);
        if (visualEffectCompleted && !dataEffectCompleted)
        {
            ResolveDataEffect(origin, target);
            dataEffectCompleted = true;
            Cleanup();
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

    public List<Ship> GetTargets()
    {
        return targets;
    }

    internal abstract void ResolveDataEffect(Ship origin, Ship target);
    internal abstract void ResolveVisualEffect(Ship origin, Ship target);
    internal abstract void Cleanup();
}
