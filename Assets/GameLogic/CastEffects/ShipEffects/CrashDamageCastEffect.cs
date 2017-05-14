public class CrashDamageCastEffect : CastEffectResolver
{
    public CrashDamageCastEffect(Ship origin, Ship target) : base(origin, target)
    {
    }

    internal override void ResolveDataEffect()
    {
        targets.Add(target);
        target.health += -15;
        origin.health += -10;
    }

    internal override void ResolveVisualEffect()
    {
        MoveToData();
    }

    internal override void Cleanup()
    {
        MoveToFinished();
    }
}
