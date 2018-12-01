using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputParser {

    public class PressedRelease {
        public bool pressed;
        public bool released;
        public PressedRelease() {
            this.Reset();
        }
        public void Reset(){
            pressed = false;
            released = false;
        }
    }

    // buffer variables
    // Input.GetKeyDown can't be reliably sampled from FixedUpdate(),
    // so instead this object's UpdateInputBuffers call should be called from a Monobehaviour's
    // Update() callback to buffer the appropriate GetKeyDown/Up inputs.
    public PressedRelease space {get; private set;}
    public PressedRelease down {get; private set;}
    public PressedRelease left {get; private set;}
    public PressedRelease right {get; private set;}

    public InputParser() {
        this.space = new PressedRelease();
        this.down = new PressedRelease();
        this.left = new PressedRelease();
        this.right = new PressedRelease();
    }

    public Vector2 GetDirection() {
        Vector2 total = Vector2.zero;
        // Turns out Input.GetAxis("horizontal") will output non-zero if opposite
        // keys on the same axis (i.e. "A" and "D") are held at the same time...
        // ... hence why i'm doing this manually.
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            total +=  Vector2.right;
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            total -= Vector2.right;
        }
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            total += Vector2.up;
        }
        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            total -= Vector2.up;
        }
        return total;
    }

    // Call from an owning monobehaviour's "Update"
    public void UpdateInputBuffers(){
        // Jump
        if(Input.GetKeyDown(KeyCode.Space)) {
            space.pressed = true;
        }
        if(Input.GetKeyUp(KeyCode.Space)) {
            space.released = true;
        }

        // Fast-fall
        if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
            down.pressed = true;
        }
        else if(Input.GetKeyUp(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
            // (Else if prevent user from pressing up/down on 'S' and down/up on 'Up' 
            // at the same time.)
            down.released = true;
        }

        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)){
            right.pressed = true;
        }
        else if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)){
            right.released = true;
        }

        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)){
            left.pressed = true;
        }
        else if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)){
            left.released = true;
        }
    }

    // Call from a "FixedUpdate" after all inputs have been read/consumed
    public void ClearInputBuffers() {
        space.Reset();
        down.Reset();
        right.Reset();
        left.Reset();
    }
}