public class CrashDamageCastEffect : CastEffectResolver
{
    public CrashDamageCastEffect(Ship origin, Ship target) : base(origin, target)
    {
    }

    internal override void ResolveDataEffect(Ship origin, Ship target)
    {
        targets.Add(target);
        target.health += -15;
        origin.health += -10;
    }

    internal override void ResolveVisualEffect(Ship origin, Ship target)
    {
        SetVisualEffectCompleted();
    }

    internal override void Cleanup()
    {       
    }
}
