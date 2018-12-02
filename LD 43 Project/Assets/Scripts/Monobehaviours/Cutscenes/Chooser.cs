using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chooser : MonoBehaviour {

    public RectTransform[] options;
    public RectTransform selectArrow;

    public int selectedIndex = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // TODO: Use the button things for this?
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
            selectedIndex--;
            if (selectedIndex < 0) {
                selectedIndex += options.Length;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
            selectedIndex++;
            selectedIndex %= options.Length;
        }

        RectTransform selectedOption = options[selectedIndex];
        Vector3 pos = selectArrow.position;
        pos.x = selectedOption.position.x;

        selectArrow.position = pos;
	}
}
