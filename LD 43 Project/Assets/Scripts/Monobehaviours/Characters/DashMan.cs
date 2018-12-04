using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashMan : MonoBehaviour {

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
        MusicController musicController = FindObjectOfType<MusicController>();

        cutsceneHandler.StartCutscene(new List<System.Action>
        {
            () => {
                musicController.TransitionTo(musicController.Background1);
               camera.Focus(transform);
                camera.config.distanceToFocus = 10;
                camera.config.focusOffset = 0;
                cutsceneHandler.SetSpeech("Greetings little one!");
            },
            () => {
                cutsceneHandler.SetSpeech("This is the grave of the legendary Dash Man!");
            },
            () => {
                cutsceneHandler.SetSpeech("but wait, I'm getting ahead of myself. Let's start with introductions.");
            },
            () => {
                cutsceneHandler.SetSpeech("I am the legendary Dash Man.");
            },
            () => {
                cutsceneHandler.SetSpeech("This is my grave.");
            },
            () => {
                camera.Focus(player.transform);
                cutsceneHandler.SetSpeech("...");
            },
            () => {
                camera.Focus(transform);
                cutsceneHandler.SetSpeech("I'm not dead yet. I'm just getting it ready.");
            },
            () => {
                camera.Focus(player.transform);
                cutsceneHandler.SetSpeech("...");
            },
            () => {
                camera.Focus(transform);
                camera.config.distanceToFocus = 6;
                cutsceneHandler.SetSpeech("Now, prepare to receive the power of dash!");
                musicController.TransitionTo(musicController.Scary, 4);
            },
            () => {
                camera.config.distanceToFocus = 10;
                camera.Focus(player.transform);
                cutsceneHandler.ShowChoice();
            },
            () => {
                cutsceneHandler.HideChoice();
                camera.Focus(transform);
                AbilityHandler.Ability selectedAbility = cutsceneHandler.chooser.SelectedAbility;
                cutsceneHandler.SetSpeech(
                    string.Format(
                        "Ah yes, who needs {0}. Into the grave it goes!",
                        selectedAbility.ToString()));
                
                AbilityHandler handler = FindObjectOfType<AbilityHandler>();
                handler.RemoveAbility(selectedAbility);

                handler.GrantAbility(AbilityHandler.Ability.Dash);

                musicController.TransitionTo(musicController.Full);
            },
            () => {
                cutsceneHandler.SetSpeech("As a student of the legendary Dash Man, press Shift to perform a legendary Dash!");
            },
            () => {
                shownCutscene = true;
            }
        });
    }
}
