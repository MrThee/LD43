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
        MusicController musicController = FindObjectOfType<MusicController>();

        cutsceneHandler.StartCutscene(new List<System.Action>
        {
            () => {
                camera.Focus(player.transform);
                camera.config.distanceToFocus = 10;
                camera.config.focusOffset = 0;
                cutsceneHandler.SetSpeech("Hello!");
            },
            () => {
                camera.Focus(transform);
                cutsceneHandler.SetSpeech("It's dangerous to go alone! Take this!");
            },
            () => {
                camera.Focus(player.transform);
                cutsceneHandler.SetSpeech("...");
            },
            () => {
                camera.Focus(transform);
                cutsceneHandler.SetSpeech("At this altar, the worthy receive a gift from The Gourd.");
            },
            () => {
                cutsceneHandler.SetSpeech("This weapon will let you defend against the rotten produce of this land.");
            },
            () => {
                camera.Focus(player.transform);
                cutsceneHandler.SetSpeech("Great!");
            },
            () => {
                camera.Focus(transform);
                camera.config.distanceToFocus = 7;
                cutsceneHandler.SetSpeech("Hold on!");
                musicController.TransitionTo(musicController.Scary, 4);
            },
            () => {
                cutsceneHandler.SetSpeech("Only the worthy receive the gift.");
            },
            () => {
                cutsceneHandler.SetSpeech("To prove your worth...");
            },
            () => {
                camera.config.distanceToFocus = 6;
                cutsceneHandler.SetSpeech("You must...");
            },
            () => {
                camera.config.distanceToFocus = 5;
                cutsceneHandler.SetSpeech("Sacrifice!");
            },
            () => {
                cutsceneHandler.SetSpeech("A sacrifice must be made of one of your abilities.");
            },
            () => {
                cutsceneHandler.SetSpeech("Choose wisely, for you shall not receive it again.");
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
                        "Sacrificing {0}. A wise choice.",
                        selectedAbility.ToString()));
                
                AbilityHandler handler = FindObjectOfType<AbilityHandler>();
                handler.RemoveAbility(selectedAbility);

                handler.GrantAbility(AbilityHandler.Ability.Gun);

                musicController.TransitionTo(musicController.Background1);
            },
            () => {
                cutsceneHandler.SetSpeech("Now behold! The peashooter!");
            },
            () => {
                cutsceneHandler.SetSpeech("Press Spacebar to fire your weapon!");
            },
            () => {
                shownCutscene = true;
            }
        });
    }
}
