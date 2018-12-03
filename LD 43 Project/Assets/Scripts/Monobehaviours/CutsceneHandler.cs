using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CutsceneHandler : MonoBehaviour
{
    private int actionIndex = 0;
    private List<Action> cutsceneActions;
    private GameStateHandler gameStateHandler;

    private FollowPlayer camera;
    private FollowPlayer.CameraConfig defaultCameraConfig;

    public Chooser chooser;
    public TextMeshProUGUI textMesh;
    public RectTransform speechBubble;

    // Use this for initialization
    void Start()
    {
        gameStateHandler = FindObjectOfType<GameStateHandler>();
        camera = FindObjectOfType<FollowPlayer>();
        defaultCameraConfig = camera.config;

        cutsceneActions = new List<Action>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStateHandler.state != GameStateHandler.GameState.Cutscene) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {

            PerformNextStep();

       }
    }

    private void PerformNextStep() {
        HideSpeechBox();

        Action action = cutsceneActions[actionIndex];
        action.Invoke();

        actionIndex++;
        if (actionIndex >= cutsceneActions.Count)
        {
            EndCutscene();
            return;
        }
    }

    private void HideSpeechBox() {
        speechBubble.gameObject.SetActive(false);
        textMesh.gameObject.SetActive(false);
    }

    public void SetSpeech(String text) {
        speechBubble.gameObject.SetActive(true);
        textMesh.gameObject.SetActive(true);
        textMesh.SetText(text);
    }

    public void StartCutscene(List<Action> cutsceneSteps) {
        cutsceneActions.AddRange(cutsceneSteps);
        gameStateHandler.state = GameStateHandler.GameState.Cutscene;

        // Perform the next step.
        PerformNextStep();
    }

    private void EndCutscene() {
        ResetCamera();
        gameStateHandler.state = GameStateHandler.GameState.GamePlay;

        cutsceneActions.Clear();
        actionIndex = 0;
    }

    private void ResetCamera() {
        PlayerController player = FindObjectOfType<PlayerController>();
        camera.kFocus = player.transform;
        camera.config = defaultCameraConfig;
    }
}
