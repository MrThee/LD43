using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererWrapper : MonoBehaviour {
	public Material redMat;

	private Dictionary<Renderer, Material> mk_defaultBindings;
	private OnForSeconds mk_redFlasher;

	// Use this for initialization
	void Start () {
		this.mk_defaultBindings = new Dictionary<Renderer, Material>();
		
		this.mk_redFlasher = new OnForSeconds(2f);
		this.mk_redFlasher.AddThresholdAction(_ChangeToRedMat, 0.00f);
		this.mk_redFlasher.AddThresholdAction(_ChangeToRedMat, 0.50f);
		this.mk_redFlasher.AddThresholdAction(_ChangeToRedMat, 1.00f);
		this.mk_redFlasher.AddThresholdAction(ChangeToDefaultMat, 0.25f);
		this.mk_redFlasher.AddThresholdAction(ChangeToDefaultMat, 0.75f);
		this.mk_redFlasher.AddThresholdAction(ChangeToDefaultMat, 1.25f);

		this.Rebind();
	}

	void FixedUpdate (){
		float deltaTime = Time.fixedDeltaTime;
		mk_redFlasher.UpdateState(deltaTime);
	}

	public void Rebind(){
		List<Renderer> renderers = new List<Renderer>();
		GetComponentsInChildren<Renderer>(true, renderers);
	}
	
	public void Rebind(List<Renderer> Rs){
		this.mk_defaultBindings.Clear();
		foreach(Renderer r in Rs){
			this.mk_defaultBindings[r] = r.sharedMaterial;
		}
	}

	public void FlashRed(){
		mk_redFlasher.ActivateForDefaultDuration();
	}

	private void _ChangeToRedMat() {
		_ChangeAllToMat(redMat);
	}

	public void ChangeToDefaultMat() {
		foreach(var binding in mk_defaultBindings){
			binding.Key.sharedMaterial = binding.Value;
		}
	}

	private void _ChangeAllToMat(Material mat){
		foreach(var binding in mk_defaultBindings){
			binding.Key.sharedMaterial = mat;
		}
	}
}
