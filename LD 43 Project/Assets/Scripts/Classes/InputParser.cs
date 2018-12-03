using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputParser {

    public class PressedRelease {
        public bool pressed {get; private set;}
        public bool released {get; private set;}
        private readonly KeyCode key1;
        private readonly bool has2ndKey;
        private readonly KeyCode key2;
        public PressedRelease(KeyCode key) {
            this.key1 = key;
            this.has2ndKey = false;
            this.key2 = KeyCode.LeftWindows; // meme-key
            this.Reset();
        }

        public PressedRelease(KeyCode key1, KeyCode key2){
            this.key1 = key1;
            this.key2 = key2;
            this.has2ndKey = true;
            this.Reset();
        }

        public void Buffer(){
            if( Input.GetKeyDown(this.key1) || 
                (has2ndKey && Input.GetKeyDown(this.key2)) )
            {
                pressed = true;
            }
            else if(Input.GetKeyUp(this.key1) || 
                (has2ndKey && Input.GetKeyUp(this.key2)) )
            {
                released = true;
            }
        }

        public void Reset(){
            pressed = false;
            released = false;
        }

        public override string ToString()
        {
            return string.Format("pressed = {0}, released = {1}", pressed, released);
        }
    }

    // buffer variables
    // Input.GetKeyDown can't be reliably sampled from FixedUpdate(),
    // so instead this object's UpdateInputBuffers call should be called from a Monobehaviour's
    // Update() callback to buffer the appropriate GetKeyDown/Up inputs.
    public PressedRelease space {get; private set;}
    public PressedRelease shift { get; private set; }

    public PressedRelease up { get; private set; }
    public PressedRelease down {get; private set;}
    public PressedRelease left {get; private set;}
    public PressedRelease right {get; private set;}

    // Wrappers around these that represent what action they're used for
    public PressedRelease Jump
    {
        get
        {
            return up;
        }
    }
    public PressedRelease Dash
    {
        get
        {
            return shift;
        }
    }
    public PressedRelease Shoot
    {
        get
        {
            return space;
        }
    }

    public InputParser() {
        this.space = new PressedRelease(KeyCode.Space);
        this.shift = new PressedRelease(KeyCode.LeftShift, KeyCode.RightShift);

        this.up = new PressedRelease(KeyCode.W, KeyCode.UpArrow);
        this.down = new PressedRelease(KeyCode.S, KeyCode.DownArrow);
        this.left = new PressedRelease(KeyCode.A, KeyCode.LeftArrow);
        this.right = new PressedRelease(KeyCode.D, KeyCode.RightArrow);
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
        up.Buffer();

        // Shoot
        space.Buffer();

        // Dash
        shift.Buffer();

        // Fast-fall
        down.Buffer();

        right.Buffer();
        left.Buffer();
    }

    // Call from a "FixedUpdate" after all inputs have been read/consumed
    public void ClearInputBuffers() {
        space.Reset();
        shift.Reset();
        down.Reset();
        right.Reset();
        left.Reset();
        up.Reset();
    }
}