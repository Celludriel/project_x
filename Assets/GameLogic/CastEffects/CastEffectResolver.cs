using System;
using System.Collections.Generic;

public abstract class CastEffectResolver {

    public enum CastEffectState { INIT, BUSY, VISUAL, DATA, CLEAN, FINISHED }

    public GameContext gameContext;

    private CastEffectState state;
    protected Ship origin, target;

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
            ResolveVisualEffect();
        }

        if(state == CastEffectState.DATA)
        {
            MoveToBusy();
            ResolveDataEffect();
            FinishDataEffect();
            MoveToClean();
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

    protected void MoveToBusy()
    {
        state = CastEffectState.BUSY;
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

    internal abstract void ResolveVisualEffect();
    internal abstract void ResolveDataEffect();    
    internal abstract void Cleanup();
}
