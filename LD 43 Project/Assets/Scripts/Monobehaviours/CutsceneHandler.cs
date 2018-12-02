using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneHandler : MonoBehaviour
{

    private int actionIndex = 0;
    private List<Action> cutsceneActions;
    private GameStateHandler gameStateHandler;

    private FollowPlayer camera;
    private FollowPlayer.CameraConfig defaultCameraConfig;

    // Use this for initialization
    void Start()
    {
        gameStateHandler = FindObjectOfType<GameStateHandler>();
        camera = FindObjectOfType<FollowPlayer>();
        defaultCameraConfig = camera.config;

        cutsceneActions = new List<Action>{
            () => {
                ButterflyFriend friend = FindObjectOfType<ButterflyFriend>();
                camera.kFocus = friend.transform;
                camera.config.focusOffset = 0;
            },
            () => {
                camera.config.distanceToFocus = 10;
            },
            () => {
                camera.config.distanceToFocus = 5;
            },
            () => {
                camera.config.distanceToFocus = 2;
            },
            () => {
                Canvas canvas = FindObjectOfType<Canvas>();
                canvas.enabled = true;
            },
            () => {
                Canvas canvas = FindObjectOfType<Canvas>();
                canvas.enabled = false;
            },
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStateHandler.state != GameStateHandler.GameState.Cutscene) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {

            Action action = cutsceneActions[actionIndex];
            action.Invoke();

            actionIndex++;
            if (actionIndex >= cutsceneActions.Count)
            {
                EndCutscene();
                return;
            }
        }
    }

    private void EndCutscene() {
        ResetCamera();
        gameStateHandler.state = GameStateHandler.GameState.GamePlay;
    }

    private void ResetCamera() {
        PlayerController player = FindObjectOfType<PlayerController>();
        camera.kFocus = player.transform;
        camera.config = defaultCameraConfig;
    }
}
