using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupMan : MonoBehaviour {

    private bool shownCutscene = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController == null) {
            return;
        }

        if (shownCutscene) {
            return;
        }

        // Start a cutscene!
        CutsceneHandler cutsceneHandler = FindObjectOfType<CutsceneHandler>();
        FollowPlayer camera = FindObjectOfType<FollowPlayer>();
        PlayerController player = FindObjectOfType<PlayerController>();
        cutsceneHandler.StartCutscene(new List<System.Action>
        {
            () => {
                camera.Focus(transform);
                camera.config.distanceToFocus = 10;
                camera.config.focusOffset = 0;
                cutsceneHandler.SetSpeech("Hello friend");
            },
            () => {
                camera.FocusOnPlayer();
                cutsceneHandler.SetSpeech("Hi there");
            },
            () => {
                camera.Focus(transform);
                cutsceneHandler.chooser.gameObject.SetActive(true);
            },
            () => {
                cutsceneHandler.chooser.gameObject.SetActive(false);
                camera.Focus(transform);
                cutsceneHandler.SetSpeech(
                    string.Format(
                        "You picked item {0}. Thanks.",
                        cutsceneHandler.chooser.selectedIndex + 1));
            },
            () => {
                shownCutscene = true;
            }
        });
    }
}
