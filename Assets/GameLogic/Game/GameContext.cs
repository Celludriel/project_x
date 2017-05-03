using UnityEngine;

public class GameContext : MonoBehaviour {

    public GameManager gameManager;
    public InitiativeManager initiativeManager;
    public ButtonManager buttonManager;
    public InformationManager informationManager;
    public CastEffectFactory castEffectFactory;
    public CastEffectPlayer castEffectPlayer;

    public Canvas canvas;

    public Transform shipPrefab;
    public GameObject selectionCirclePrefab;
    public GameObject targetCirclePrefab;
    public Material playerOneMaterial;
    public Material playerTwoMaterial;
}
