using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHandler : MonoBehaviour {

    public enum Ability {
        Dash,
        Gun,
        Butterfly,
        Walljump,
    }

    [System.Serializable]
    public class IconPair
    {
        public Ability name;
        public RectTransform icon;
    }

    public List<Ability> OwnedAbilities;

    public List<IconPair> Icons;

    [Header("Other Things")]

    public ButterflyFriend butterflyPrefab;
    public FiringArray peashooter;

    private PlayerController player;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        OwnedAbilities = new List<Ability>();
        GrantAbility(Ability.Dash);
    }

    public bool HasAbility(Ability ability) {
        return OwnedAbilities.Contains(ability);
    }

    public void GrantAbility(Ability ability)
    {
        OwnedAbilities.Add(ability);

        // If needed, logic for adding the thing
        switch (ability) {
            case Ability.Butterfly:
                ButterflyFriend butterfly = Instantiate(butterflyPrefab);
                butterfly.transform.position = player.transform.position;
                break;
            case Ability.Gun:
                InventoryHarness harness = FindObjectOfType<InventoryHarness>();
                harness.AddGun(peashooter, true);
                break;
        }
    }

    public void RemoveAbility(Ability ability) {
        if (!HasAbility(ability)) {
            return;
        }
        OwnedAbilities.Remove(ability);

        // If needed, logic for removing the thing
        switch (ability)
        {
            case Ability.Butterfly:
                ButterflyFriend butterfly = FindObjectOfType<ButterflyFriend>();
                // TODO: butterfly dying VFX/SFX
                Destroy(butterfly.gameObject);
                break;
            case Ability.Gun:
                InventoryHarness harness = FindObjectOfType<InventoryHarness>();
                harness.UnEquipGun();
                break;
        }
    }
}
