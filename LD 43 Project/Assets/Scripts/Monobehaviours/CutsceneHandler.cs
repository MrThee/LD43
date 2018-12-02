using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneHandler : MonoBehaviour
{

    private int actionIndex = -1;
    private List<Action> cutsceneActions;

    // Use this for initialization
    void Start()
    {
        cutsceneActions = new List<Action>{
            () => {
                Debug.Log("Hello");
            },
            () => {
                Debug.Log("Hello 2");
            },
            () => {
                Debug.Log("Hey there 3");
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            actionIndex++;
            actionIndex %= cutsceneActions.Count;

            Action action = cutsceneActions[actionIndex];
            action.Invoke();
        }
    }
}
