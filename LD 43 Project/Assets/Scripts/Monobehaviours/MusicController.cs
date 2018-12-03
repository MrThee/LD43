using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

    public UnityEngine.Audio.AudioMixerSnapshot Intro;
    public UnityEngine.Audio.AudioMixerSnapshot Simple;
    public UnityEngine.Audio.AudioMixerSnapshot Background1;
    public UnityEngine.Audio.AudioMixerSnapshot Background2;
    public UnityEngine.Audio.AudioMixerSnapshot Background3;
    public UnityEngine.Audio.AudioMixerSnapshot Full;
    public UnityEngine.Audio.AudioMixerSnapshot Scary;

    public UnityEngine.Audio.AudioMixer Mixer;

	// Use this for initialization
	void Start () {
        Mixer.TransitionToSnapshots(new UnityEngine.Audio.AudioMixerSnapshot[] { Background1 }, new float[]{1}, 2);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TransitionTo(UnityEngine.Audio.AudioMixerSnapshot snapshot, float transitionLength = 1)
    {
        Mixer.TransitionToSnapshots(new UnityEngine.Audio.AudioMixerSnapshot[] { snapshot }, new float[] { 1 }, transitionLength);
    }

}
