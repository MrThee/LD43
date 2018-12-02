using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnForSeconds {

	public bool active { get; private set; }
	public readonly float defaultDuration;
	public float currentDurationSet {
		get {
			return overrideDuration == inactiveDurationValue ? defaultDuration : overrideDuration;
		}
	}
	public float overrideDuration {get; private set;}
	private const float inactiveDurationValue = -1f;
	public float onTimer { get; private set; }

	public OnForSeconds(float defaultDuration) {
		this.defaultDuration = defaultDuration;
		this.overrideDuration = inactiveDurationValue;
		this.onTimer = 0f;
		this.activationEventHandler = new WillDid<ActivationEventArgs>();
		this.thresholdEventHandler = new WillDid<ThresholdEventArgs>();
	}

	public void UpdateState(float deltaTime) {
		if(active){
			ThresholdEventArgs teArgs = new ThresholdEventArgs(){
				oldOnTime = onTimer,
				newOnTime = onTimer+deltaTime
			};
			thresholdEventHandler.Will.Invoke(teArgs);
			onTimer += deltaTime;
			thresholdEventHandler.Did.Invoke(teArgs);


			if(onTimer > currentDurationSet) {
				var aeArgs = new ActivationEventArgs{active=false};
				activationEventHandler.Will.Invoke(aeArgs);
				active = false;
				onTimer = 0f;
				activationEventHandler.Did.Invoke(aeArgs);
			}
		}
	}

	public void ActivateForDefaultDuration() {
		overrideDuration = inactiveDurationValue;
		ActivationEventArgs aea = new ActivationEventArgs{active=true};
		activationEventHandler.Will.Invoke(aea);
		active = true;
		onTimer=0f;
		activationEventHandler.Did.Invoke(aea);
	}

	public void ActivateForDuration(float overrideTime) {
		overrideDuration = overrideTime;
		ActivationEventArgs aea = new ActivationEventArgs{active=true};
		activationEventHandler.Will.Invoke(aea);
		active = true;
		onTimer=0f;
		activationEventHandler.Did.Invoke(aea);
	}

	public class ActivationEventArgs {
		public bool active;
	}
	public class ThresholdEventArgs {
		public float oldOnTime;
		public float newOnTime;
	}

	public WillDid<ActivationEventArgs> activationEventHandler {get; private set;}
	public WillDid<ThresholdEventArgs> thresholdEventHandler {get; private set;}

}
