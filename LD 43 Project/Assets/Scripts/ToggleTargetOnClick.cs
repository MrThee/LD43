using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleTargetOnClick : MonoBehaviour, IPointerDownHandler {

	public GameObject target;

	public void OnPointerDown(PointerEventData data){
		target.SetActive(!target.activeSelf);
	}
}
