using System;
using System.Collections.Generic;

public abstract class CastEffectResolver {

    public enum CastEffectState { INIT, VISUAL, DATA, CLEAN, FINISHED }

    public GameContext gameContext;

    private CastEffectState state;
    private Ship origin, target;

    protected List<Ship> targets = new List<Ship>();

    public CastEffectResolver(Ship origin)
    {
        this.origin = origin;
        state = CastEffectState.INIT;
    }

    public CastEffectResolver(Ship origin, Ship target)
    {
        this.origin = origin;
        this.target = target;
        state = CastEffectState.INIT;
    }

    public void ResolveCastEffect()
    {
        if(state == CastEffectState.INIT)
        {
            state = CastEffectState.VISUAL;
        }

        if(state == CastEffectState.VISUAL)
        {
            ResolveVisualEffect(origin, target);
        }

        if(state == CastEffectState.DATA)
        {
            ResolveDataEffect(origin, target);
            FinishDataEffect();
        }

        if (state == CastEffectState.CLEAN)
        {
            Cleanup();
        }
    }

    public bool IsFinished()
    {
        return state == CastEffectState.FINISHED;
    }

    private void FinishDataEffect()
    {
        foreach (Ship target in targets)
        {
            gameContext.informationManager.UpdateHoverInfoPanel(target);
        }
        gameContext.informationManager.UpdateShipInfoPanel(origin);
    }

    protected void MoveToData()
    {
        state = CastEffectState.DATA;
    }

    protected void MoveToClean()
    {
        state = CastEffectState.CLEAN;
    }

    protected void MoveToFinished()
    {
        state = CastEffectState.FINISHED;
    }

    internal abstract void ResolveDataEffect(Ship origin, Ship target);
    internal abstract void ResolveVisualEffect(Ship origin, Ship target);
    internal abstract void Cleanup();
}
