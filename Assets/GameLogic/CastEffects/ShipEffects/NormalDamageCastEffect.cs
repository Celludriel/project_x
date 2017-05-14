using System;
using System.Collections.Generic;
using UnityEngine;

public class NormalDamageCastEffect : CastEffectResolver
{
    public NormalDamageCastEffect(Ship origin) : base(origin)
    {
    }

    internal override void ResolveDataEffect()
    {
        List<Transform> allShips = gameContext.initiativeManager.GetAllShips();
        foreach (Transform shipObject in allShips)
        {
            target = shipObject.GetComponent<Ship>();
            if (target.isTargeted)
            {
                targets.Add(target);
                target.health += -20;
            }
        }
    }

    internal override void ResolveVisualEffect()
    {
        MoveToData();
    }

    internal override void Cleanup()
    {
        gameContext.buttonManager.EndButtonAction();
        MoveToFinished();
    }
}
