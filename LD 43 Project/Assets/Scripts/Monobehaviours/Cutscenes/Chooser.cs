using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chooser : MonoBehaviour {

    public List<AbilityHandler.Ability> abilities = new List<AbilityHandler.Ability>();
    public List<RectTransform> optionIcons = new List<RectTransform>();
    public RectTransform selectArrow;

    public AbilityHandler.Ability SelectedAbility {
        get {
            return abilities[selectedIndex];
        }
    }

    public int selectedIndex = 0;

    public void UpdateOptions()
    {
        ClearOptions();

        AbilityHandler handler = FindObjectOfType<AbilityHandler>();
        List<AbilityHandler.IconPair> iconPairs = handler.Icons;

        abilities.Clear();
        abilities.AddRange(handler.OwnedAbilities);

        for (int i = 0; i < abilities.Count; i ++) {
            AbilityHandler.Ability ability = abilities[i];

            // Get the icon
            AbilityHandler.IconPair iconPair = iconPairs.Find(pair => pair.name == ability);

            RectTransform icon = Instantiate(iconPair.icon, transform, false);
            float positionRatio = 0.5f;
            if (abilities.Count > 1) {
                positionRatio = ((float) i) / (abilities.Count - 1);
            }
            icon.transform.localPosition = (-200f + 400f * positionRatio) * Vector3.right;

            optionIcons.Add(icon);
        }
    }

    public void ClearOptions() {
        while (optionIcons.Count > 0) {
            Destroy(optionIcons[optionIcons.Count - 1].gameObject);
            optionIcons.RemoveAt(optionIcons.Count - 1);
        }
    }

	// Update is called once per frame
	void Update () {
        if (optionIcons.Count == 0) {
            return;
        }

        // TODO: Use the button things for this?
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
            selectedIndex--;
            if (selectedIndex < 0) {
                selectedIndex += abilities.Count;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
            selectedIndex++;
            selectedIndex %= abilities.Count;
        }

        RectTransform selectedOption = optionIcons[selectedIndex];
        Vector3 pos = selectArrow.position;
        pos.x = selectedOption.position.x;

        selectArrow.position = pos;
	}
}
