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
		this.activationEventHandler = new WillDid();
		this.deactivationEventHandler = new WillDid();
		this.mk_thresholdActions = new List<ThresholdAction>();
	}

	public void UpdateState(float deltaTime) {
		if(active){
			ThresholdEventArgs teArgs = new ThresholdEventArgs(){
				oldOnTime = onTimer,
				newOnTime = onTimer+deltaTime
			};
			float oldTime = onTimer;
			onTimer += deltaTime;
			float newTime = onTimer;

			mk_thresholdActions.ForEach(ta => {
				if(ta.threshold > oldTime && ta.threshold <= newTime){
					ta.action.Invoke();
				}
			});

			if(onTimer > currentDurationSet) {
				deactivationEventHandler.Will.Invoke();
				active = false;
				onTimer = 0f;
				deactivationEventHandler.Did.Invoke();
			}
		}
	}

	public void ActivateForDefaultDuration() {
		overrideDuration = inactiveDurationValue;
		activationEventHandler.Will.Invoke();
		active = true;
		onTimer=0f;
		activationEventHandler.Did.Invoke();
	}

	public void ActivateForDuration(float overrideTime) {
		overrideDuration = overrideTime;
		activationEventHandler.Will.Invoke();
		active = true;
		onTimer=0f;
		activationEventHandler.Did.Invoke();
	}
	public class ThresholdEventArgs {
		public float oldOnTime;
		public float newOnTime;
	}

	public WillDid activationEventHandler {get; private set;}
	public WillDid deactivationEventHandler {get; private set;}
	
	private List<ThresholdAction> mk_thresholdActions;

	public void AddThresholdAction(System.Action action, float when){
		this.mk_thresholdActions.Add(new ThresholdAction{
			action = action, threshold = when
		});
	}

	private class ThresholdAction {
		public System.Action action;
		public float threshold;
	}

}
