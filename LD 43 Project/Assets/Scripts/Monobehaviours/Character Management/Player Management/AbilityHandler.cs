using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHandler : MonoBehaviour {

    public enum Ability {
        Dash,
        Gun,
        Butterfly,
        Walljump
    }

    public List<Ability> OwnedAbilities;

    public ButterflyFriend butterflyPrefab;

    private PlayerController player;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        OwnedAbilities = new List<Ability>();
        GrantAbility(Ability.Dash);
        GrantAbility(Ability.Gun);
        GrantAbility(Ability.Butterfly);
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
        }
    }
}
