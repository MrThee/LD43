using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueForSeconds<T> {

	private OnForSeconds driver;
	public bool active { get {return driver.active; } }
	public float currentDurationSet { get { return driver.currentDurationSet; } }
	public float onTimer { get { return driver.onTimer; } }
	public WillDid<OnForSeconds.ActivationEventArgs> activationEventHandler {get { return driver.activationEventHandler; } }
	public WillDid<OnForSeconds.ThresholdEventArgs> thresholdEventHandler {get { return driver.thresholdEventHandler; } }

	private readonly T inactiveValue;

	public T output {get; private set;}

	public ValueForSeconds(T lowValue) {
		this.driver = new OnForSeconds(1f); // We will always override this duration.
		this.inactiveValue = lowValue;
		this.output = lowValue;
		this.valueActivatedHandler = new WillDid<T>();

		driver.activationEventHandler.Did.AddCallback(DriverDidActivate);
	}

	void DriverDidActivate(OnForSeconds.ActivationEventArgs args){
		if(args.active == false){
			output = inactiveValue;
		}
	}

	public void UpdateState(float deltaTime) {
		driver.UpdateState(deltaTime);
	}

	public void Activate(T highValue, float duration){
		output = highValue;
		valueActivatedHandler.Will.Invoke(highValue);
		driver.ActivateForDuration(duration);
		valueActivatedHandler.Did.Invoke(highValue);
	}

	public WillDid<T> valueActivatedHandler {get; private set;}
}
