using System.Collections.Generic;
using UnityEngine;

public class RandomDamageCastEffect : CastEffectResolver
{

    public RandomDamageCastEffect(Ship origin) : base(origin)
    {
    }

    internal override void ResolveDataEffect(Ship origin, Ship target)
    {
        List<Transform> allShips = gameContext.initiativeManager.GetAllShips();
        foreach(Transform shipObject in allShips)
        {
            target = shipObject.GetComponent<Ship>();
            if (target.isTargeted)
            {
                targets.Add(target);
                target.health += -(UnityEngine.Random.Range(10, 30));
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
