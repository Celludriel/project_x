using System.Collections.Generic;
using UnityEngine;

public class CastEffectPlayer : MonoBehaviour {

    public GameContext gameContext;
    private Queue<CastEffectResolver> effectQueue = new Queue<CastEffectResolver>();
    private CastEffectResolver currentCastEffect = null;
	
	void Update () {
        if(currentCastEffect != null)
        {
            currentCastEffect.ResolveCastEffect();
            if (currentCastEffect.IsDataEffectCompleted())
            {
                gameContext.informationManager.UpdateHoverInfoPanel(currentCastEffect.GetTarget());
                currentCastEffect = null;                
            }
        }
        else if (effectQueue.Count > 0)
        {
            currentCastEffect = effectQueue.Dequeue();
        }		
	}

    public void AddCastEffectResolver(CastEffectResolver resolver)
    {
        effectQueue.Enqueue(resolver);
    }
}
