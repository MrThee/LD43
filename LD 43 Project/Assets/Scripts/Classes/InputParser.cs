using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputParser {

    // buffer variables
    // Input.GetKeyDown can't be reliably sampled from FixedUpdate(),
    // so instead this object's UpdateInputBuffers call should be called from a Monobehaviour's
    // Update() callback to buffer the appropriate GetKeyDown/Up inputs.
    public bool jumpPressed { get; private set; }

    public InputParser() {
        this.jumpPressed = false;
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
        total.Normalize();
        return total;
    }

    // Call from an owning monobehaviour's "Update"
    public void UpdateInputBuffers(){
        if(Input.GetKeyDown(KeyCode.Space)) {
            // Buffer
            jumpPressed = true;
        }
    }

    // Call from a "FixedUpdate" after all inputs have been read/consumed
    public void ClearInputBuffers() {
        jumpPressed = false;
    }
}