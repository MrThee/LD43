using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationMixer : MonoBehaviour {
    // Turns out legacy Unity had a primitive means of masking animation playback on
    // a transform hierarchy
    public Animation kAnimation;
    public Transform shoulderLeft;
    public Transform shoulderRight;

    void Start(){
        // TODO: single-point initialization
        kAnimation["FireFromRun"].AddMixingTransform(shoulderLeft, true);
        kAnimation["FireFromRun"].AddMixingTransform(shoulderRight, true);
        kAnimation["FireFromRun"].layer = 1;
    }

}