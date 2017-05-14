using System;
using System.Collections.Generic;
using UnityEngine;

public class NormalDamageCastEffect : CastEffectResolver
{
    public NormalDamageCastEffect(Ship origin) : base(origin)
    {
    }

    internal override void ResolveDataEffect(Ship origin, Ship target)
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
        MoveToClean();
    }

    internal override void ResolveVisualEffect(Ship origin, Ship target)
    {
        MoveToData();
    }

    internal override void Cleanup()
    {
        gameContext.buttonManager.EndButtonAction();
        MoveToFinished();
    }
}
