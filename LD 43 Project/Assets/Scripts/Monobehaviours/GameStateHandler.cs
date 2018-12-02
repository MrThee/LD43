using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateHandler : MonoBehaviour {

    public enum GameState {
        GamePlay,
        Cutscene,
    }

    public GameState state = GameState.GamePlay;
}
